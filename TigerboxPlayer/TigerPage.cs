using Tigerbox.Spec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigerbox.Objects
{
    public class TigerPage
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public TigerPage()
        {
            Folders = new List<TigerFolder>();
        }

        private int _page;

        
        private IList<TigerFolder> _folder;

        /// <summary>
        /// Page Number
        /// </summary>
        public int Page
        {
            get
            {
                return _page;
            }
            set
            {
                _page = value;
            }
        }

        /// <summary>
        /// Folder's count
        /// </summary>
        public int FolderQuantity
        {
            get
            {
                return _folder.Count;
            }
        }

        /// <summary>
        /// Folders into page
        /// </summary>
        public IList<TigerFolder> Folders
        {
            get
            {
                return _folder;
            }
            set
            {
                _folder = value;
            }
        }
    }
}
