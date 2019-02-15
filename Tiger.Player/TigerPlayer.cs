using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tigerbox.Spec;
using Tigerbox.Objects;
using Tigerbox.Exceptions;
using AxWMPLib;
using System.Threading;

namespace Tiger.Player
{
    public partial class TigerPlayer : UserControl
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public TigerPlayer()
        {
            InitializeComponent();

            _mediaPlayer.uiMode = "none";

            _mediaPlayer.settings.autoStart = false;

        }

        /// <summary>
        /// Delegate for player state event
        /// </summary>
        public AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler PlayerStateEvent;

        /// <summary>
        /// Set the play state event to the delegated method, should be called after defining PlayerStateEvent delegate
        /// </summary>
        public void SetChangeEvent()
        {
            if (PlayerStateEvent==null)
            {
                throw new Exception("You aren't able to set PlayStateChange event, because you haven't defined the PlayerStateEvent delegate.");
            }
            _mediaPlayer.PlayStateChange += PlayerStateEvent;
        }

        /// <summary>
        /// Check if the player is in fullscreen state
        /// </summary>
        public bool IsFullScreen
        {
            get
            {
                if (_mediaPlayer != null)
                {
                    return _mediaPlayer.fullScreen;
                }
                return false;
            }
        }

        /// <summary>
        /// Check if media player is playing
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                if (_mediaPlayer != null)
                {
                    return this._mediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying;
                }

                return false;
            }
        }


        /// <summary>
        /// Insert a new media into playlist
        /// </summary>
        /// <param name="tigerMedia">TigerMedia who will be inserted into playlist</param>
        public void AddMediaToPlayList(TigerMedia tigerMedia)
        {
            AddMediaToPlayList(tigerMedia.Path);
        }

        /// <summary>
        /// Insert a new media into playlist
        /// </summary>
        /// <param name="path">Song's path who will be inserted into playlist</param>
        public void AddMediaToPlayList(string path)
        {
            if (_mediaPlayer == null)
            {
                throw new Exception("It was impossible insert a new song because player is not running.");
            }

            var media = this._mediaPlayer.newMedia(path);
            this._mediaPlayer.currentPlaylist.appendItem(media);
        }

        /// <summary>
        /// Start _mediaPlayer play
        /// </summary>
        public void Play()
        {
            try
            {
                _mediaPlayer.Ctlcontrols.play();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// If is playing, stop the curret song
        /// </summary>
        public void Stop()
        {
            try
            {
                if (IsPlaying)
                {
                    var item = this._mediaPlayer.Ctlcontrols.currentItem;
                    this._mediaPlayer.currentPlaylist.removeItem(item);
                    this._mediaPlayer.Ctlcontrols.play();
                }
            }
            catch{}
        }

        /// <summary>
        /// Set the component size and position
        /// </summary>
        /// <param name="x">left position</param>
        /// <param name="y">top position</param>
        /// <param name="width">width size</param>
        /// <param name="height">height size</param>
        public void SetTigerPlayerBounds(int x, int y, int width, int height)
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.SetBounds(x, y, width, height, BoundsSpecified.None);
                _mediaPlayer.Width = width;
                _mediaPlayer.Height = height;
            }            
        }

        /// <summary>
        /// Set component visible
        /// </summary>
        /// <param name="visible">visible</param>
        public void SetPlayerVisible(bool visible)
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Visible = visible;
                this.Visible = visible;
                if (visible)
                {
                    _mediaPlayer.Show();
                    this.Show();
                }
                else
                {
                    this._mediaPlayer.Hide();
                    this.Hide();
                }
            }            
        }

        /// <summary>
        /// Set component fullscreen when it's not in fullscreen or
        /// set component to size configured when it's fullscreen
        /// </summary>
        public void SetFullScreen()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.fullScreen = !IsFullScreen;
            }            
        }

        /// <summary>
        /// Increase volume
        /// </summary>
        public void IncreaseVolume()
        {
            if (_mediaPlayer != null)
            {
                if (this._mediaPlayer.settings.volume >= 5)
                {
                    this._mediaPlayer.settings.volume -= 5;
                }
            }
        }

        /// <summary>
        /// Decrease volume
        /// </summary>
        public void DecreaseVolume()
        {
            if (_mediaPlayer != null)
            {
                if (this._mediaPlayer.settings.volume <= 100)
                {
                    this._mediaPlayer.settings.volume += 5;
                }
            }            
        }

        public void DisposePlayer()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Ctlcontrols.stop();
                _mediaPlayer.Dispose();
                _mediaPlayer = null;
            }
        }

        public void ClearPlayList()
        {
            if (_mediaPlayer.currentPlaylist.count > 0)
            {
                for (int i = (_mediaPlayer.currentPlaylist.count-1); i >= 0; i--)
                {
                    _mediaPlayer.currentPlaylist.removeItem(_mediaPlayer.currentPlaylist.Item[i]);
                }
            }
        }
    }
}
