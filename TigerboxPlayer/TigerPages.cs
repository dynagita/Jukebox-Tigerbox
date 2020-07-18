using Tigerbox.Spec;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tigerbox.Exceptions;
using System.Windows.Forms;

namespace Tigerbox.Objects
{
    public class TigerPages
    {

        public IList<TigerPage> Pages
        {
            get
            {
                return _pages;
            }
        }

        /// <summary>
        /// Default constructor invoked by simple injector
        /// </summary>
        /// <param name="_configuration"></param>
        /// <param name="_jsonUtil"></param>
        public TigerPages(IConfigurationService _configuration, IJsonService _jsonUtil)
        {
            _pages = new List<TigerPage>();
            this._configuration = _configuration;
            this._jsonUtil = _jsonUtil;
        }

        /// <summary>
        /// Selected Page
        /// </summary>
        public int SelectedPage { get; set; }


        /// <summary>
        /// Selected Folder Index
        /// </summary>
        public int SelectedFolderIndex
        {
            get
            {
                var pages = _pages.FirstOrDefault(x => x.Page == SelectedPage);
                TigerFolder selected = pages
                                        .Folders.FirstOrDefault(x => x.Selected);
                if (selected == null)
                {
                    return 0;
                }

                return pages.Folders.IndexOf(selected);
            }
        }

        /// <summary>
        /// Page's count
        /// </summary>
        public int Count
        {
            get
            {
                return _pages.Count;
            }
        }

        /// <summary>
        /// Count number of folders into selected page
        /// </summary>
        public int SelectedFolderCount
        {
            get
            {
                return _pages.ElementAt(SelectedPage - 1).Folders.Count();
            }
        }

        /// <summary>
        /// Return's the Selected Folder
        /// </summary>
        public TigerFolder SelectedFolder
        {
            get
            {
                var pages = _pages.FirstOrDefault(x => x.Page == SelectedPage);
                TigerFolder selected = pages
                                        .Folders.FirstOrDefault(x => x.Selected);

                return selected;
            }
        }

        private IConfigurationService _configuration;

        private IJsonService _jsonUtil;

        private IList<TigerPage> _pages;
        
        /// <summary>
        /// Insert a new Page into Page
        /// </summary>
        /// <param name="page">Page Object to be inserted</param>
        private void InsertNewPage(TigerPage page)
        {
            if (_pages.Any(x => x.Page == page.Page))
            {
                throw new PageAlreadyCreatedException();
            }

            page.Page = _pages.Count + 1;

            _pages.Add(page);

           _pages = _pages.OrderBy(x => x.Page).ToList();
        }

        /// <summary>
        /// Load pages from jsonDataBase
        /// </summary>
        public void LoadPages()
        {
            try
            {
                string dbFIle = _configuration
                    .GetConfiguration<string>(Constants.DatabasePathConfigName);

                if (!File.Exists(dbFIle))
                {
                    throw new DatabaseNotExistsException();
                }

                string database = File.ReadAllText(dbFIle);

                _pages = _jsonUtil.FileToJson<IList<TigerPage>>(database);

            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Create or update json database
        /// </summary>
        public void CreateUpdateDatabase()
        {
            //Get paths to read
            string[] paths = _configuration.GetConfiguration<string>(Constants.MainFolderConfigName).Split(';');

            //If hasn't found paths throw exception
            if (paths == null || paths.Length == 0 || paths[0].Equals("YOUR_PATHS"))
            {
                throw new ConfigPathNameNotSetted();
            }

            //Initialize musicDirectoriesPath
            List<TigerFolder> musicDirectoriesPath = new List<TigerFolder>();

            //Read all Directories
            foreach (var item in paths)
            {
                foreach (var dir in Directory.GetDirectories(item))
                {
                    TigerFolder f = new TigerFolder();
                    f.Path = dir;
                    musicDirectoriesPath.Add(f);
                }
            }

            //Sort directories
            musicDirectoriesPath = TigerFolder.SortDirectories(musicDirectoriesPath);
            
            Console.Clear();

            TigerPage page = new TigerPage();
            
            List<string> corruptedImages = new List<string>();

            for (int i = 0; i < musicDirectoriesPath.Count; i++)
            {
                var dir = musicDirectoriesPath[i];
                Console.WriteLine(string.Format("Updating TigerboxDatabase - Directory: {0}", dir.Name));
                //Get directories files
                var files = Directory.GetFiles(dir.Path);
                for (int k = 0; k < files.Length; k++)
                {
                    var file = files[k];
                    Console.WriteLine(string.Format("Updating TigerboxDatabase - File: {0}", file));
                    TigerMedia tMedia = new TigerMedia();

                    tMedia.Path = file;

                    if ((tMedia.Type.ToLower().Contains("mp3")) || 
                        (tMedia.Type.ToLower().Contains("mp4")) || 
                        (tMedia.Type.ToLower().Contains("avi")) || 
                        (tMedia.Type.ToLower().Contains("mpeg")) || 
                        (tMedia.Type.ToLower().Contains("mpg")) || 
                        (tMedia.Type.ToLower().Contains("wmv")) || 
                        (tMedia.Type.ToLower().Contains("wmv")) || 
                        (tMedia.Type.ToLower().Contains("png")) || 
                        (tMedia.Type.ToLower().Contains("jpg")) || 
                        (tMedia.Type.ToLower().Contains("jpeg")) ||
                        (tMedia.Type.ToLower().Contains("gif"))
                        )
                    {

                        if (tMedia.IsImage && !IsImageOk(tMedia.Path))
                        {
                            corruptedImages.Add(tMedia.Path);
                        }
                        else
                        {
                            dir.Medias.Add(tMedia);
                        }                        
                    }
                }

                page.Folders.Add(dir);

                if (page.FolderQuantity == Constants.QuantityPerPage)
                {
                    InsertNewPage(page);
                    page = new TigerPage();
                }
            }
            if (page.FolderQuantity < Constants.QuantityPerPage)
            {
                InsertNewPage(page);
            }
            
            Console.Clear();

            if (corruptedImages.Any())
            {
                Console.WriteLine($"*******************************************************************");
                Console.WriteLine($"***********************Corrupted files found***********************");
                Console.WriteLine($"*******************************************************************");

                foreach (var item in corruptedImages)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine($"We just found {corruptedImages.Count} image(s) corrupted. Would you like to delete them? Y - Yes / N - No");
                
                var answer = Console.ReadLine();
                if (answer.Equals("Y", StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach (var item in corruptedImages)
                    {
                        try
                        {
                            System.IO.File.Delete(item);
                            Console.WriteLine($"Deleted {item}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error deleting {item}: {ex.Message}");
                        }                        
                    }
                }
            }
        }

        /// <summary>
        /// Save database into a json object
        /// </summary>
        public void SaveDataBase()
        {
            try
            {
                if (_pages==null || _pages.Count == 0)
                {
                    CreateUpdateDatabase();
                }
                string database = _jsonUtil.SerializeJson(_pages);
                string databasePath = _configuration.GetConfiguration<string>(Constants.DatabasePathConfigName);
                File.WriteAllText(databasePath, database);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get all folders from selected page
        /// </summary>
        /// <returns>Folder's list page</returns>
        public IList<TigerFolder> GetFolders()
        {
            var folders = _pages.FirstOrDefault(x => x.Page == SelectedPage).Folders;

            if (folders == null || folders.Count == 0)
            {
                throw new PageNotFoundException();
            }

            return folders;
        }

        /// <summary>
        /// Find page number by specific letter
        /// </summary>
        /// <param name="letter">Letter to be searched</param>
        /// <returns>First page number who has a folder starting with specified letter </returns>
        public int FindPageByLetter(string letter)
        {
            var pages = _pages.Where(x => x.Folders.Where(y => y.Letter.Contains(letter)).Any());
            if (!pages.Any())
            {
                return -1;
            }

            var page = pages.Min(x => x.Page);

            return page;
        }

        /// <summary>
        /// Find index of first folder who starts with specified letter
        /// </summary>
        /// <param name="pageNumber">Page number who will be checked</param>
        /// <param name="letter">Letter searched</param>
        public void FindFolderByIndexAndLetter(int pageNumber, string letter)
        {
            UnselectSelectedFoldersInPagesNotSearched(pageNumber);
            var page = _pages.FirstOrDefault(x => x.Page == pageNumber);
            for (int i = 0; i < page.Folders.Count; i++)
            {
                if (page.Folders[i].Letter.Equals(letter))
                {
                    UnselectActualFolder();
                    page.Folders[i].Selected = true;
                    return;
                }
            }
        }


        private void UnselectSelectedFoldersInPagesNotSearched(int pageNumber)
        {
            var pages = _pages.Where(x => x.Page != pageNumber);
            if (pages.Any())
            {
                var pagesWithSelected = pages.Where(x => x.Folders.Where(y => y.Selected).Any());
                if (pagesWithSelected.Any())
                {
                    foreach (var item in pagesWithSelected)
                    {
                        var folders = item.Folders.Where(x => x.Selected);
                        if (folders.Any())
                        {
                            foreach (var f in folders)
                            {
                                f.Selected = false;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Unselect actual selected folder
        /// </summary>
        private void UnselectActualFolder()
        {
            _pages.FirstOrDefault(x => x.Folders.Where(y => y.Selected).Any())
                .Folders.FirstOrDefault(x => x.Selected).Selected = false;
        }

        /// <summary>
        /// Move an Entire page
        /// </summary>
        public void MoveEntirePage()
        {
            UnselectActualFolder();

            SelectedPage++;

            if (SelectedPage > _pages.Count())
            {
                SelectedPage = 1;
            }

            _pages.FirstOrDefault(x => x.Page == SelectedPage).Folders[0].Selected = true;
        }

        private static bool IsImageOk(string url)
        {
            bool result = true;

            try
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Load(url);
            }
            catch (Exception ex)
            {
                result = false;
            }                       

            return result;
        }
    }
}
