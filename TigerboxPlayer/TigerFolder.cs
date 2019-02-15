using Tigerbox.Spec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tigerbox.Objects
{
    public class TigerFolder
    {

        public TigerFolder()
        {
            _medias = new List<TigerMedia>();
        }

        IList<TigerMedia> _medias;
        private string _letter;
        private string _name;
        private string _path;

        /// <summary>
        /// Folder name
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }
        /// <summary>
        /// Folder Path
        /// </summary>
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                char[] splitChar = new char[] { (char)92 };
                string[] dados = _path.Split(splitChar);
                _name = dados[dados.Length - 1];
                _letter = _name.Substring(0, 1);
            }
        }

        private System.Drawing.Imaging.ImageFormat GetImageType()
        {
            if (!string.IsNullOrEmpty(ImagePath))
            {
                if (ImagePath.ToUpper().Contains("JPG") || ImagePath.ToUpper().Contains("JPEG"))
                {
                    return System.Drawing.Imaging.ImageFormat.Jpeg;
                }
                else if (ImagePath.ToUpper().Contains("GIF"))
                {
                    return System.Drawing.Imaging.ImageFormat.Gif;
                }
                else if (ImagePath.ToUpper().Contains("PNG"))
                {
                    return System.Drawing.Imaging.ImageFormat.Png;
                }                
            }
            return null;
        }

        public string GetImageBase64()
        {
            string base64 = string.Empty;
            if (!string.IsNullOrWhiteSpace(ImagePath))
            {
                try
                {
                    var image = System.Drawing.Image.FromFile(ImagePath);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        var type = GetImageType();
                        if (type!=null)
                        {
                            image.Save(ms, type);
                            var buffer = ms.ToArray();
                            base64 = Convert.ToBase64String(buffer);
                        }
                    }
                }
                catch
                {

                    return string.Empty;
                }
            }

            return base64;
        }

        /// <summary>
        /// Folder Image Path
        /// </summary>
        public string ImagePath
        {
            get
            {
                TigerMedia media = _medias
                    .FirstOrDefault(x => x.Type == "jpg" ||
                                         x.Type == "png" ||
                                         x.Type == "jpeg" ||
                                         x.Type == "gif");
                if (media == null)
                {
                    return string.Empty;
                }
                return media.Path;
            }
        }

        /// <summary>
        /// Folder fist letter
        /// </summary>
        public string Letter
        {
            get
            {
                return _letter;
            }
        }

        /// <summary>
        /// Indicate if this folder is selected on the tigerbox software
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Medias folder
        /// </summary>
        public IList<TigerMedia> Medias
        {
            get
            {
                return _medias;
            }
        }

        /// <summary>
        /// Shift a folder to right when sorting
        /// </summary>
        /// <param name="initial">Initial position</param>
        /// <param name="list">List ordering</param>
        private static void ShiftRight(int initial, ref List<TigerFolder> list)
        {
            list.Add(new TigerFolder());
            for (int i = list.Count - 1; i > initial; i--)
            {
                list[i] = list[i - 1];
            }
        }

        /// <summary>
        /// Sort TigerFolders by name
        /// </summary>
        /// <param name="diretorios">TigerFolder list</param>
        public static void SortDirectories(List<TigerFolder> diretorios)
        {
            List<TigerFolder> result = new List<TigerFolder>();
            for (int i = 0; i < diretorios.Count; i++)
            {
                if (i == 0)
                {
                    result.Add(diretorios[i]);
                }
                else
                {
                    for (int j = 0; j < result.Count; j++)
                    {
                        string insertedName = result[j].Name;
                        if (string.Compare(insertedName, diretorios[i].Name, true) < 0)
                        {
                            //O diretório já inserido é menor que o diretório a ser inserido
                            if ((j + 1) < result.Count)
                            {
                                continue;
                            }
                            else
                            {
                                result.Add(diretorios[i]);
                                break;
                            }
                        }
                        else
                        {
                            //O diretório a ser inserido é menor que o diretório já inserido
                            ShiftRight(j, ref result);
                            result[j] = diretorios[i];
                            break;
                        }
                    }
                }
            }
        }
    }
}
