using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Web;
using Tigerbox.Objects;
using Tigerbox.SiteControl.Models;
using Tigerbox.Spec;
using static Tigerbox.Objects.Enums;

namespace Tigerbox.SiteControl.Services
{
    public class HomeService
    {
        private IConfigurationService configurationService;

        private IJsonService jsonService;

        public HomeService(IConfigurationService configurationService, IJsonService jsonService)
        {
            this.configurationService = configurationService;
            this.jsonService = jsonService;

            Models.Database.LoadConfData(configurationService, jsonService);
        }

        public string GetIMageUrlBase()
        {
            return configurationService.GetConfiguration<string>(Constants.ImageUrlBase);
        }

        public bool IsAdminUser(string hostName)
        {
            return GetAdminList().Contains(hostName);
        }

        private IList<string> GetAdminList()
        {
            var adminConfig = configurationService.GetConfiguration<string>(Constants.AdminUsers);

            var adminList = adminConfig.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            return adminList;
        }

        private IList<TigerFolder> ListAllFolders()
        {
            IList<TigerFolder> folders = new List<TigerFolder>();

            foreach (var item in Models.Database.Page.Pages)
            {
                foreach (var folder in item.Folders)
                {
                    folders.Add(folder);
                }
            }

            return folders;
        }        

        public IList<TigerFolder> ListFolders()
        {
            return ListFolders(string.Empty);
        }

        public IList<TigerFolder> ListFoldersByPage(int page)
        {
            
            var folders = ListFolders();
            return folders.Skip(page * 30).Take(30).ToList();
        }

        public IList<TigerFolder> ListFolders(string nameSearched)
        {
            var folders = ListAllFolders();

            if (string.IsNullOrWhiteSpace(nameSearched))
            {
                return ListAllFolders();
            }
            else
            {
                var result = ListAllFolders().Where(x => x.Name.ToUpper().Contains(nameSearched.ToUpper()));

                if (result == null)
                {
                    result = new List<TigerFolder>();
                }

                return result.ToList();
            }
        }

        public IList<TigerMedia> ListMedias(string folderName)
        {
            IList<TigerMedia> medias = new List<TigerMedia>();

            var folder = GetFolderByName(folderName);

            foreach (var item in folder.Medias)
            {
                medias.Add(item);
            }
            return medias;
        }

        private TigerFolder GetFolderByName(string folderName)
        {
            return ListAllFolders().First(x => x.Name == folderName);
        }

        public TigerMedia GetMediaByName(string folderName, string mediaName)
        {
            var folder = GetFolderByName(folderName);

            return folder.Medias.First(x => x.Name == mediaName);
        }

        public void SendMusicToTigerBox(string folderName, string musicName)
        {
            TigerBoxMessage message = new TigerBoxMessage();
            message.MessageType = Enums.MessageType.Media;
            message.Media = GetMediaByName(folderName, musicName);

            QueueMedia(message);
        }

        public void SendActionToTigerBox(string action)
        {
            if (action.ToUpper().Equals("STOP"))
            {
                SendActionToTigerBox(TigerActions.Stop);
            }
            else if (action.ToUpper().Equals("CREDITS"))
            {
                SendActionToTigerBox(TigerActions.IncreaseCredit);
            }
            else if (action.ToUpper().Equals("VOLUMEUP"))
            {
                SendActionToTigerBox(TigerActions.IncreaseVolume);
            }
            else if (action.ToUpper().Equals("VOLUMEDOWN"))
            {
                SendActionToTigerBox(TigerActions.DecreaseVolume);
            }
            else
            {
                throw new Exception(string.Format("Action '{0}' is not implemented.", action));
            }
        }

        private void SendActionToTigerBox(TigerActions action)
        {
            TigerBoxMessage message = new TigerBoxMessage();
            message.MessageType = Enums.MessageType.Action;
            message.Action = action;

            QueueMedia(message);
        }

        private TigerNetworkData GetDataToSend(TigerMedia media, TigerActions action)
        {
            TigerNetworkData networkData = new TigerNetworkData();

            networkData.Action = action;

            networkData.Media = media;

            return networkData;
        }

        private void SendDataToServer(TigerNetworkData data)
        {
            try
            {
                TigerTCPClient tcpClient = new TigerTCPClient(configurationService, jsonService);
                tcpClient.SendData(data);
            }
            catch
            {

                throw;
            }
        }

        public string GetFirstFolderName()
        {
            return ListFolders()[0].Name;
        }

        public string GetNoImageBase64()
        {
            string path = configurationService.GetConfiguration<string>(Constants.DefaultImagePath);
            return GetBase64(path);
        }

        public string GetVideoIconBase64()
        {
            string path = configurationService.GetConfiguration<string>(Constants.VideoIconPath);
            return GetBase64(path);
        }

        public string GetMusicIconBase64()
        {
            string path = configurationService.GetConfiguration<string>(Constants.MusicIconPath);
            return GetBase64(path);
        }

        private string GetBase64(string path)
        {
            try
            {
                string base64 = string.Empty;
                var image = System.Drawing.Image.FromFile(path);
                using (MemoryStream ms = new MemoryStream())
                {
                    var type = GetImageType(path);
                    if (type != null)
                    {
                        image.Save(ms, type);
                        var buffer = ms.ToArray();
                        base64 = Convert.ToBase64String(buffer);
                    }
                }
                return base64;
            }
            catch
            {
                return null;
            }
        }

        private System.Drawing.Imaging.ImageFormat GetImageType(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                if (imagePath.ToUpper().Contains("JPG") || imagePath.ToUpper().Contains("JPEG"))
                {
                    return System.Drawing.Imaging.ImageFormat.Jpeg;
                }
                else if (imagePath.ToUpper().Contains("GIF"))
                {
                    return System.Drawing.Imaging.ImageFormat.Gif;
                }
                else if (imagePath.ToUpper().Contains("PNG"))
                {
                    return System.Drawing.Imaging.ImageFormat.Png;
                }
            }
            return null;
        }

        private void QueueMedia(TigerBoxMessage message)
        {
            var qMessage = new MessageQueue(configurationService.GetConfiguration<string>("PrivateQueue"));
            qMessage.Send(message);

        }
    }
}