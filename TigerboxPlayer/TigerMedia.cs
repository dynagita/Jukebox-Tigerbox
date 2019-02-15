using Tigerbox.Spec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigerbox.Objects
{
    public class TigerMedia
    {
        private string _name;
        private string _path;
        private string _type;
        private int _listViewIndex = -1;
        /// <summary>
        /// Media name
        /// </summary>
        public string Name
        {
            get
            {
                return this._name.Trim();
            }
        }

        /// <summary>
        /// Media type
        /// Suposed to be: JPG, GIF, PNG, JPEG, MP3, MP4, WMV, AVI
        /// </summary>
        public string Type
        {
            get
            {
                return _type.Trim();
            }
        }

        /// <summary>
        /// Media path
        /// </summary>
        public string Path
        {
            get
            {
                return _path.Trim();
            }
            set
            {
                _path = value;
                char[] charSplit = new char[] { (char)92 };
                string[] data = _path.Split(charSplit);
                charSplit = new char[] { '.' };
                data = data[data.Length - 1].Split(charSplit);
                if (data.Length > 2)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (i < data.Length - 1)
                        {
                            if (string.IsNullOrEmpty(_name))
                            {
                                _name += " ";
                            }
                            _name += data[i];
                        }
                    }
                    _type = data[data.Length - 1];
                }
                else
                {
                    _name = data[data.Length - 2];
                    _type = data[data.Length - 1];
                }

            }
        }

        /// <summary>
        /// Check if the media is an image
        /// </summary>
        public bool IsImage
        {
            get
            {
                return ((_type.ToUpper().Equals("JPG")) || 
                        (_type.ToUpper().Equals("GIF")) || 
                        (_type.ToUpper().Equals("PNG")) || 
                        (_type.ToUpper().Equals("JPEG")));
            }
        }

        /// <summary>
        /// Media index on FolderSongsList in TigerBox system
        /// </summary>
        public int ListViewIndex
        {
            get
            {
                return _listViewIndex;
            }
            set
            {
                if (!IsImage)
                {
                    _listViewIndex = value;
                }
                
            }
        }

        /// <summary>
        /// Override method from objects base
        /// </summary>
        /// <returns>TigerMedia's name</returns>
        public override string ToString()
        {
            return _name.Trim();
        }
    }
}
