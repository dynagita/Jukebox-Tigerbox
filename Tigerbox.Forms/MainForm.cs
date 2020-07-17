using Tigerbox.Spec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tiger.Player;
using Tigerbox.Objects;
using System.IO;
using static System.Windows.Forms.ImageList;
using Tigerbox.Exceptions;
using static Tigerbox.Objects.Enums;
using System.Timers;
using System.Messaging;

namespace Tigerbox.Forms
{

    public delegate void StopThreadManager();

    public class MainForm : Form
    {
        #region attributes
        private System.Timers.Timer _timer;

        private DateTime _lastFormOperation;

        private PictureBox _selectedPictureBox;

        private TigerPages _pages;

        private Panel _generalPanel;

        private PictureBox _picSelectedFolder;

        private PictureBox _picFolder12;

        private PictureBox _picFolder6;

        private PictureBox _picFolder11;

        private PictureBox _picFolder5;

        private PictureBox _picFolder10;

        private PictureBox _picFolder4;

        private PictureBox _picFolder9;

        private PictureBox _picFolder3;

        private PictureBox _picFolder8;

        private PictureBox _picFolder2;

        private PictureBox _picFolder7;

        private PictureBox _picFolder1;

        private TigerSharedData _sharedList;

        private TigerPlayer _player;

        private ListView _listViewFolderSongs;

        private string _defaultImagePath;

        private ListView _listViewSelectedSongs;

        private ColumnHeader columnHeader1;

        private LetterPanel.LetterPanel letterPanel1;

        private Label _lbSelectedFolder;

        private Label _lbCredits;

        private ColumnHeader columnHeader2;

        public StopThreadManager StopThreadManager;

        private bool debug = false;

        IConfigurationService _configurationService;

        System.Timers.Timer _queueTimer;
        #endregion

        #region Creating Form

        /// <summary>
        /// Custom form initialization
        /// </summary>
        private void CustomInitializeComponent(IConfigurationService configuration, IJsonService jsonUtil)
        {
            _configurationService = configuration;

            if (!debug)
            {
                this.TopMost = true;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }

            this._pages = new TigerPages(configuration, jsonUtil);

            this._sharedList = new TigerSharedData(configuration, jsonUtil);

            this._pages.LoadPages();

            InitializeComponent();

            this._listViewFolderSongs.KeyDown += ListView_KeyEvent;
            this._listViewFolderSongs.KeyUp += ListView_KeyEvent;


            //Player
            this._player = new TigerPlayer();

            this._player.PlayerStateEvent = new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(ChangePlayerStateEvent);

            this._player.SetChangeEvent();
            this._generalPanel.Controls.Add(_player);
            this._player.SetBounds(340, 343, 334, 355);
            this._player.SetTigerPlayerBounds(340, 343, 334, 355);
            this._player.SetPlayerVisible(true);

            this._defaultImagePath = configuration.GetConfiguration<string>(Constants.DefaultImagePath);

            string musicIconPath = configuration.GetConfiguration<string>(Constants.MusicIconPath);
            string videoIconPath = configuration.GetConfiguration<string>(Constants.VideoIconPath);

            System.Drawing.Image musicIcon = System.Drawing.Image.FromFile(musicIconPath);
            System.Drawing.Image videoIcon = System.Drawing.Image.FromFile(videoIconPath);

            this._listViewFolderSongs.SmallImageList = new ImageList();
            this._listViewFolderSongs.SmallImageList.Images.Add(musicIcon);
            this._listViewFolderSongs.SmallImageList.Images.Add(videoIcon);

            this._listViewSelectedSongs.SmallImageList = new ImageList();
            this._listViewSelectedSongs.SmallImageList.Images.Add(musicIcon);
            this._listViewSelectedSongs.SmallImageList.Images.Add(videoIcon);

            //Set first page
            this._pages.SelectedPage = 1;

            this.LoadPage();

            this.letterPanel1.Close();

            this.LoadSharedData();

            this._listViewFolderSongs.SelectedIndices.Add(0);

            StartFullScreenTimer();

            this._listViewSelectedSongs.Scrollable = true;

            _queueTimer = new System.Timers.Timer(500);
            _queueTimer.Elapsed += new ElapsedEventHandler(ReadFromQueue);
            _queueTimer.AutoReset = true;
            _queueTimer.Enabled = true;
        }

        /// <summary>
        /// Default windows form initialization
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._generalPanel = new System.Windows.Forms.Panel();
            this._lbCredits = new System.Windows.Forms.Label();
            this._lbSelectedFolder = new System.Windows.Forms.Label();
            this.letterPanel1 = new Tigerbox.LetterPanel.LetterPanel();
            this._listViewSelectedSongs = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._listViewFolderSongs = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._picFolder12 = new System.Windows.Forms.PictureBox();
            this._picFolder6 = new System.Windows.Forms.PictureBox();
            this._picFolder11 = new System.Windows.Forms.PictureBox();
            this._picFolder5 = new System.Windows.Forms.PictureBox();
            this._picFolder10 = new System.Windows.Forms.PictureBox();
            this._picFolder4 = new System.Windows.Forms.PictureBox();
            this._picFolder9 = new System.Windows.Forms.PictureBox();
            this._picFolder3 = new System.Windows.Forms.PictureBox();
            this._picFolder8 = new System.Windows.Forms.PictureBox();
            this._picFolder2 = new System.Windows.Forms.PictureBox();
            this._picFolder7 = new System.Windows.Forms.PictureBox();
            this._picFolder1 = new System.Windows.Forms.PictureBox();
            this._picSelectedFolder = new System.Windows.Forms.PictureBox();
            this._generalPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._picSelectedFolder)).BeginInit();
            this.SuspendLayout();
            // 
            // _generalPanel
            // 
            this._generalPanel.Controls.Add(this._lbCredits);
            this._generalPanel.Controls.Add(this._lbSelectedFolder);
            this._generalPanel.Controls.Add(this.letterPanel1);
            this._generalPanel.Controls.Add(this._listViewSelectedSongs);
            this._generalPanel.Controls.Add(this._listViewFolderSongs);
            this._generalPanel.Controls.Add(this._picFolder12);
            this._generalPanel.Controls.Add(this._picFolder6);
            this._generalPanel.Controls.Add(this._picFolder11);
            this._generalPanel.Controls.Add(this._picFolder5);
            this._generalPanel.Controls.Add(this._picFolder10);
            this._generalPanel.Controls.Add(this._picFolder4);
            this._generalPanel.Controls.Add(this._picFolder9);
            this._generalPanel.Controls.Add(this._picFolder3);
            this._generalPanel.Controls.Add(this._picFolder8);
            this._generalPanel.Controls.Add(this._picFolder2);
            this._generalPanel.Controls.Add(this._picFolder7);
            this._generalPanel.Controls.Add(this._picFolder1);
            this._generalPanel.Controls.Add(this._picSelectedFolder);
            this._generalPanel.Location = new System.Drawing.Point(-1, 0);
            this._generalPanel.Name = "_generalPanel";
            this._generalPanel.Size = new System.Drawing.Size(1010, 730);
            this._generalPanel.TabIndex = 0;
            // 
            // _lbCredits
            // 
            this._lbCredits.AutoSize = true;
            this._lbCredits.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbCredits.ForeColor = System.Drawing.Color.WhiteSmoke;
            this._lbCredits.Location = new System.Drawing.Point(677, 319);
            this._lbCredits.Name = "_lbCredits";
            this._lbCredits.Size = new System.Drawing.Size(72, 22);
            this._lbCredits.TabIndex = 19;
            this._lbCredits.Text = "Credits:";
            // 
            // _lbSelectedFolder
            // 
            this._lbSelectedFolder.AutoSize = true;
            this._lbSelectedFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lbSelectedFolder.ForeColor = System.Drawing.Color.WhiteSmoke;
            this._lbSelectedFolder.Location = new System.Drawing.Point(35, 318);
            this._lbSelectedFolder.Name = "_lbSelectedFolder";
            this._lbSelectedFolder.Size = new System.Drawing.Size(0, 22);
            this._lbSelectedFolder.TabIndex = 18;
            // 
            // letterPanel1
            // 
            this.letterPanel1.Location = new System.Drawing.Point(152, 266);
            this.letterPanel1.Name = "letterPanel1";
            this.letterPanel1.Size = new System.Drawing.Size(716, 42);
            this.letterPanel1.TabIndex = 17;
            // 
            // _listViewSelectedSongs
            // 
            this._listViewSelectedSongs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this._listViewSelectedSongs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._listViewSelectedSongs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this._listViewSelectedSongs.Location = new System.Drawing.Point(676, 343);
            this._listViewSelectedSongs.MultiSelect = false;
            this._listViewSelectedSongs.Name = "_listViewSelectedSongs";
            this._listViewSelectedSongs.Size = new System.Drawing.Size(300, 355);
            this._listViewSelectedSongs.TabIndex = 16;
            this._listViewSelectedSongs.UseCompatibleStateImageBehavior = false;
            this._listViewSelectedSongs.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 275;
            // 
            // _listViewFolderSongs
            // 
            this._listViewFolderSongs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this._listViewFolderSongs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._listViewFolderSongs.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this._listViewFolderSongs.Location = new System.Drawing.Point(38, 343);
            this._listViewFolderSongs.MultiSelect = false;
            this._listViewFolderSongs.Name = "_listViewFolderSongs";
            this._listViewFolderSongs.Size = new System.Drawing.Size(300, 355);
            this._listViewFolderSongs.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this._listViewFolderSongs.TabIndex = 15;
            this._listViewFolderSongs.UseCompatibleStateImageBehavior = false;
            this._listViewFolderSongs.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 275;
            // 
            // _picFolder12
            // 
            this._picFolder12.Location = new System.Drawing.Point(866, 150);
            this._picFolder12.Name = "_picFolder12";
            this._picFolder12.Size = new System.Drawing.Size(110, 110);
            this._picFolder12.TabIndex = 12;
            this._picFolder12.TabStop = false;
            // 
            // _picFolder6
            // 
            this._picFolder6.Location = new System.Drawing.Point(866, 35);
            this._picFolder6.Name = "_picFolder6";
            this._picFolder6.Size = new System.Drawing.Size(110, 110);
            this._picFolder6.TabIndex = 11;
            this._picFolder6.TabStop = false;
            // 
            // _picFolder11
            // 
            this._picFolder11.Location = new System.Drawing.Point(750, 150);
            this._picFolder11.Name = "_picFolder11";
            this._picFolder11.Size = new System.Drawing.Size(110, 110);
            this._picFolder11.TabIndex = 10;
            this._picFolder11.TabStop = false;
            // 
            // _picFolder5
            // 
            this._picFolder5.Location = new System.Drawing.Point(750, 35);
            this._picFolder5.Name = "_picFolder5";
            this._picFolder5.Size = new System.Drawing.Size(110, 110);
            this._picFolder5.TabIndex = 9;
            this._picFolder5.TabStop = false;
            // 
            // _picFolder10
            // 
            this._picFolder10.Location = new System.Drawing.Point(634, 150);
            this._picFolder10.Name = "_picFolder10";
            this._picFolder10.Size = new System.Drawing.Size(110, 110);
            this._picFolder10.TabIndex = 8;
            this._picFolder10.TabStop = false;
            // 
            // _picFolder4
            // 
            this._picFolder4.Location = new System.Drawing.Point(634, 35);
            this._picFolder4.Name = "_picFolder4";
            this._picFolder4.Size = new System.Drawing.Size(110, 110);
            this._picFolder4.TabIndex = 7;
            this._picFolder4.TabStop = false;
            // 
            // _picFolder9
            // 
            this._picFolder9.Location = new System.Drawing.Point(518, 150);
            this._picFolder9.Name = "_picFolder9";
            this._picFolder9.Size = new System.Drawing.Size(110, 110);
            this._picFolder9.TabIndex = 6;
            this._picFolder9.TabStop = false;
            // 
            // _picFolder3
            // 
            this._picFolder3.Location = new System.Drawing.Point(518, 35);
            this._picFolder3.Name = "_picFolder3";
            this._picFolder3.Size = new System.Drawing.Size(110, 110);
            this._picFolder3.TabIndex = 5;
            this._picFolder3.TabStop = false;
            // 
            // _picFolder8
            // 
            this._picFolder8.Location = new System.Drawing.Point(402, 150);
            this._picFolder8.Name = "_picFolder8";
            this._picFolder8.Size = new System.Drawing.Size(110, 110);
            this._picFolder8.TabIndex = 4;
            this._picFolder8.TabStop = false;
            // 
            // _picFolder2
            // 
            this._picFolder2.Location = new System.Drawing.Point(402, 35);
            this._picFolder2.Name = "_picFolder2";
            this._picFolder2.Size = new System.Drawing.Size(110, 110);
            this._picFolder2.TabIndex = 3;
            this._picFolder2.TabStop = false;
            // 
            // _picFolder7
            // 
            this._picFolder7.Location = new System.Drawing.Point(286, 150);
            this._picFolder7.Name = "_picFolder7";
            this._picFolder7.Size = new System.Drawing.Size(110, 110);
            this._picFolder7.TabIndex = 2;
            this._picFolder7.TabStop = false;
            // 
            // _picFolder1
            // 
            this._picFolder1.Location = new System.Drawing.Point(286, 35);
            this._picFolder1.Name = "_picFolder1";
            this._picFolder1.Size = new System.Drawing.Size(110, 110);
            this._picFolder1.TabIndex = 1;
            this._picFolder1.TabStop = false;
            // 
            // _picSelectedFolder
            // 
            this._picSelectedFolder.Location = new System.Drawing.Point(38, 35);
            this._picSelectedFolder.Name = "_picSelectedFolder";
            this._picSelectedFolder.Size = new System.Drawing.Size(225, 225);
            this._picSelectedFolder.TabIndex = 0;
            this._picSelectedFolder.TabStop = false;
            // 
            // MainForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this._generalPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Padding = new System.Windows.Forms.Padding(5, 30, 5, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this._generalPanel.ResumeLayout(false);
            this._generalPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._picFolder1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._picSelectedFolder)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Default constructor who is invoked by dependency injections
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="jsonUtil"></param>
        public MainForm(IConfigurationService configuration, IJsonService jsonUtil)
        {
            CustomInitializeComponent(configuration, jsonUtil);
        }
        #endregion

        #region Form Events Methods        

        /// <summary>
        /// Initialize the keyboard buffer reader
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            KeyboardListener.s_KeyEventHandler += new EventHandler(KeyboardListener_s_KeyEventHandler);
        }

        /// <summary>
        /// Get Tiger action
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private TigerActions GetActionsByKeyPressed(ushort key)
        {
            TigerActions result = TigerActions.Undefined;
            if (key == 37)
            {
                result = TigerActions.MoveBackward;
            }
            else if (key == 39)
            {
                result = TigerActions.MoveForward;
            }
            else if (key == 38)
            {
                result = TigerActions.MoveSongListUp;
            }
            else if (key == 40)
            {
                result = TigerActions.MoveSongListDown;
            }
            else if (key == 107)
            {
                result = TigerActions.SelectSong;
            }
            else if (key == 109)
            {
                result = TigerActions.Stop;
            }
            else if (key == 13)
            {
                result = TigerActions.MoveEntirePage;
            }
            else if (key == 112)
            {
                result = TigerActions.IncreaseCredit;
            }
            else if (key == 35)
            {
                result = TigerActions.ShowLetterPanel;
                StartLetterPanelTimer();
            }
            else if (key == 66)
            {
                result = TigerActions.IncreaseVolume;
            }
            else if (key == 78)
            {
                result = TigerActions.DecreaseVolume;
            }
            return result;
        }

        /// <summary>
        /// Listen the keyboard buffer and invoke form actions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyboardListener_s_KeyEventHandler(object sender, EventArgs e)
        {
            KeyboardListener.UniversalKeyEventArgs eventArgs = (KeyboardListener.UniversalKeyEventArgs)e;
            if (eventArgs.m_Msg == 256)//Execute only into keyDown (257 == KeyUp)
            {
                var key = eventArgs.m_Key;
                ExecuteFormActions(GetActionsByKeyPressed(key));
            }
        }

        /// <summary>
        /// Invoke action to save system state (credits && playlist) when closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _player.Stop();
                _player.DisposePlayer();
                _player.Dispose();
                SaveSharedData();
                if (StopThreadManager != null)
                {
                    StopThreadManager();
                    System.Threading.Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// This method cancels ListViewFolder key events, because there is a keyboard buffer reader.
        /// But anyway the keys events of this component was triggered anyway.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_KeyEvent(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            return;
        }
        #endregion

        #region Player Methods
        /// <summary>
        /// It's the delegated method responsable for listen the _player._mediaPlayer 
        /// playlistChangeState and refresh form comps when change state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePlayerStateEvent(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 3)
            {
                this.TakeOutFirstSelectedSong();
                _player.UpdatePlayList();
            }
            else if (e.newState == 8 && this._listViewSelectedSongs.Items.Count == 0)
            {
                _player.ClearPlayList();
                SetSongsFocus();
            }
            return;
        }

        /// <summary>
        /// Play medias
        /// </summary>
        private void Play()
        {
            if (!this._player.IsPlaying)
            {
                _player.Play();
            }
        }

        /// <summary>
        /// Stops a song playing
        /// </summary>
        private void Stop()
        {
            _player.Stop();

            SetSongsFocus();
        }

        /// <summary>
        /// Set player to fullscreen
        /// </summary>
        private void SetFullScreen()
        {
            _player.SetFullScreen();
            if (!_player.IsFullScreen)
            {
                SetSongsFocus();
            }

        }

        /// <summary>
        /// Increase volume 
        /// </summary>
        private void IncreaseVolume()
        {
            _player.IncreaseVolume();
        }

        /// <summary>
        /// Decrease volume 
        /// </summary>
        private void DecreaseVolume()
        {
            _player.DecreaseVolume();
        }
        #endregion


        #region Form Methods
        /// <summary>
        /// Find and the page and select first album on page who starts with letter informed
        /// </summary>
        /// <param name="letter">Letter who gotta be found</param>
        private void FindPageByLetter(string letter)
        {
            var page = _pages.FindPageByLetter(letter);
            if (page > -1)
            {
                _pages.SelectedPage = page;
                this.LoadPage();
                _pages.FindFolderByIndexAndLetter(page, letter);
                this.SetSelectedFolder();
            }
        }

        /// <summary>
        /// Move folder forward
        /// </summary>
        private void MoveFolderForward()
        {
            int selectedFolderIndex = _pages.SelectedFolderIndex;

            int selectedPage = _pages.SelectedPage;

            _pages.SelectedFolder.Selected = false;

            if ((selectedFolderIndex == Constants.QuantityPerPage - 1) ||
                selectedFolderIndex == _pages.SelectedFolderCount - 1)
            {
                if (selectedPage == _pages.Count)
                {
                    _pages.SelectedPage = 1;
                }
                else
                {
                    _pages.SelectedPage = _pages.SelectedPage + 1;
                }
                this.LoadPage();
            }
            else
            {
                selectedFolderIndex++;
                _pages.GetFolders().ElementAt(selectedFolderIndex).Selected = true;
                this.SetSelectedFolder();
            }
        }

        /// <summary>
        /// Move folder backward
        /// </summary>
        private void MoveFolderBackward()
        {
            int selectedFolderIndex = _pages.SelectedFolderIndex;

            int selectedPage = _pages.SelectedPage;

            _pages.SelectedFolder.Selected = false;

            if (selectedFolderIndex == 0)
            {
                if (selectedPage == 1)
                {
                    _pages.SelectedPage = _pages.Count;
                }
                else
                {
                    _pages.SelectedPage = _pages.SelectedPage - 1;
                }
                this.LoadPage(true);
            }
            else
            {
                selectedFolderIndex--;
                _pages.GetFolders().ElementAt(selectedFolderIndex).Selected = true;
                this.SetSelectedFolder();
            }

        }

        /// <summary>
        /// Move song list down
        /// </summary>
        private void MoveSongListDown()
        {
            int index = this._listViewFolderSongs.SelectedIndices[0];

            index++;

            if (index < this._listViewFolderSongs.Items.Count)
            {
                this._listViewFolderSongs.SelectedIndices.Add(index);
                //Means that max visible item has been hitten
                if (this._listViewFolderSongs.Items[index].Position.Y > 323)
                {
                    this._listViewFolderSongs.TopItem = this._listViewFolderSongs.Items[index];
                }
            }
        }

        /// <summary>
        /// Move song list up
        /// </summary>
        private void MoveSongListUp()
        {
            int index = this._listViewFolderSongs.SelectedIndices[0];

            index--;

            if (index >= 0)
            {
                this._listViewFolderSongs.SelectedIndices.Add(index);
                if (this._listViewFolderSongs.Items[index].Position.Y < 0)
                {
                    this._listViewFolderSongs.TopItem = this._listViewFolderSongs.Items[index];
                }

            }
        }

        /// <summary>
        /// Move an entire page
        /// </summary>
        private void MoveEntirePage()
        {
            this._pages.MoveEntirePage();
            this.LoadPage();
            this.SetSelectedFolder();
        }

        /// <summary>
        /// Take the playing song out of selected list
        /// </summary>
        private void TakeOutFirstSelectedSong()
        {
            if (this._listViewSelectedSongs.Items.Count > 0)
            {
                this._listViewSelectedSongs.Items.RemoveAt(0);
                this._sharedList.SharedList.RemoveAt(0);
            }
        }

        /// <summary>
        /// Set the selected folder
        /// </summary>
        private void SetSelectedFolder()
        {
            if (this._selectedPictureBox != null)
            {
                this._selectedPictureBox.BorderStyle = BorderStyle.None;
            }

            TigerFolder folder = _pages.SelectedFolder;

            var selected = GetSelectedPictureBox();

            selected.BorderStyle = BorderStyle.Fixed3D;

            this._selectedPictureBox = selected;

            string compName = "_picSelectedFolder";

            var selectedPictureFolder = this.FindPictureBoxByName(compName);

            this.SetPicture(selectedPictureFolder, folder.ImagePath);

            this.SetMedias(folder.Medias);

            this._lbSelectedFolder.Text = folder.Name;
        }

        /// <summary>
        /// Find the picturebox component by it's index
        /// </summary>
        /// <param name="number">picture box index</param>
        /// <returns>PictureBox who has the informed indice</returns>
        private PictureBox FindPictureBoxByNumber(int number)
        {
            string compName = string.Format("_picFolder{0}", number);
            return this.FindPictureBoxByName(compName);
        }

        /// <summary>
        /// Find the picturebox component by it's name
        /// </summary>
        /// <param name="compName">picture box name</param>
        /// <returns>PictureBox who has the informed name</returns>
        private PictureBox FindPictureBoxByName(string compName)
        {
            var pictureBox = this._generalPanel.Controls.Find(compName, false)
                                .Where(x => x.GetType() == typeof(PictureBox))
                                .FirstOrDefault() as PictureBox;
            return pictureBox;
        }

        /// <summary>
        /// Get's the picture box who is selected
        /// </summary>
        /// <returns>PictureBox who is selected</returns>
        private PictureBox GetSelectedPictureBox()
        {
            int selectedFolderNumber = _pages.SelectedFolderIndex + 1;

            var selected = FindPictureBoxByNumber(selectedFolderNumber);

            return selected;
        }

        /// <summary>
        /// Set PictureBox image
        /// </summary>
        /// <param name="pictureBox">PictureBox who has to load image</param>
        /// <param name="url">Image's url who will be loaded</param>
        private void SetPicture(PictureBox pictureBox, string url)
        {
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            if (File.Exists(url))
            {
                pictureBox.Load(url);
            }
            else
            {
                pictureBox.Load(_defaultImagePath);
            }
        }

        /// <summary>
        /// Set medias into _listViewFolderSong
        /// </summary>
        /// <param name="medias"></param>
        private void SetMedias(IList<TigerMedia> medias)
        {
            this._listViewFolderSongs.Items.Clear();
            int index = 0;
            foreach (var item in medias.Where(x => !x.IsImage))
            {
                ListViewItem lv = GetMediaItem(item, index);
                this._listViewFolderSongs.Items.Insert(index, lv);
                index++;
            }
            this._listViewFolderSongs.SelectedIndices.Add(0);
        }

        /// <summary>
        /// Set selected _listViewFolderSong's song to be played
        /// </summary>
        private void SelectMedia()
        {
            try
            {
                _sharedList.DecreaseCredit();

                var mediaIndex = this._listViewFolderSongs.SelectedIndices[0];

                var media = this._pages.SelectedFolder
                                .Medias
                                .FirstOrDefault(x => x.ListViewIndex == mediaIndex);

                _sharedList.InsertNewMedia<TigerMedia>(media);

                ListViewItem lv = this._listViewFolderSongs
                                      .Items[mediaIndex];

                InsertMediaToSelectedList((ListViewItem)lv.Clone());
                _player.AddMediaToPlayList(media);
                this.LoadCredits();
                if (!_player.IsPlaying)
                {
                    this.Play();
                }
            }
            catch (NoCreditsException ex)
            {
                this._lbCredits.Text = ex.Message;
            }
        }

        /// <summary>
        /// insert media selected into the _listViewSelectedSong
        /// </summary>
        /// <param name="item">Selected ListViewItem to be loaded</param>
        private void InsertMediaToSelectedList(ListViewItem item)
        {
            this._listViewSelectedSongs.Items.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="media"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private ListViewItem GetMediaItem(TigerMedia media, int index)
        {
            ListViewItem lv = new ListViewItem(media.Name);
            if (media.Type.ToUpper().Contains("MP3"))
            {
                lv.ImageIndex = 0;
            }
            else
            {
                lv.ImageIndex = 1;
            }
            media.ListViewIndex = index;

            return lv;
        }

        /// <summary>
        /// Insert a credit into Jukebox
        /// </summary>
        private void InsertCredits()
        {
            this._sharedList.IncreaseCredit();
            this.LoadCredits();
        }

        /// <summary>
        /// Load credits and list's backup when starts program
        /// </summary>
        private void LoadSharedData()
        {
            LoadCredits();
            LoadSelectedList();
        }

        /// <summary>
        /// Load the selected list when starts program
        /// </summary>
        private void LoadSelectedList()
        {
            int index = 0;
            foreach (var item in _sharedList.SharedList)
            {
                _player.AddMediaToPlayList(item);
                var lv = GetMediaItem(item, index);
                this._listViewSelectedSongs.Items.Insert(index, lv);
                index++;
            }
            if (_sharedList.SharedList.Any())
            {
                this.Play();
            }
        }

        /// <summary>
        /// Load credits when starts program
        /// </summary>
        private void LoadCredits()
        {
            this._lbCredits.Text = string.Format("Credits: {0}", _sharedList.Credits);
        }

        /// <summary>
        /// Save program state into a file
        /// </summary>
        private void SaveSharedData()
        {
            _sharedList.SaveSharedData();
        }

        /// <summary>
        /// Invoke form actions when invoked from other communication way
        /// </summary>
        /// <param name="action"></param>
        public void ExecuteFormActions(TigerActions action)
        {
            switch (action)
            {
                case TigerActions.MoveForward:
                    if (!letterPanel1.IsVisible)
                    {
                        MoveFolderForward();
                    }
                    else
                    {
                        letterPanel1.MoveLetterForward();
                    }
                    break;
                case TigerActions.MoveBackward:
                    if (!letterPanel1.IsVisible)
                    {
                        MoveFolderBackward();
                    }
                    else
                    {
                        letterPanel1.MoveLetterBackward();
                    }
                    break;
                case TigerActions.MoveSongListDown:
                    MoveSongListDown();
                    break;
                case TigerActions.MoveSongListUp:
                    MoveSongListUp();
                    break;
                case TigerActions.IncreaseCredit:
                    InsertCredits();
                    break;
                case TigerActions.SelectSong:
                    SelectMedia();
                    break;
                case TigerActions.ShowLetterPanel:
                    if (!this.letterPanel1.IsVisible)
                    {
                        this.letterPanel1.Start();
                        StartLetterPanelTimer();
                    }
                    break;
                case TigerActions.MoveEntirePage:
                    MoveEntirePage();
                    break;
                case TigerActions.Stop:
                    this.Stop();
                    break;
                case TigerActions.IncreaseVolume:
                    this.IncreaseVolume();
                    break;
                case TigerActions.DecreaseVolume:
                    this.DecreaseVolume();
                    break;
                default:
                    break;
            }
            if (_player.IsFullScreen)
            {
                SetFullScreen();
            }
            _lastFormOperation = DateTime.Now;
        }

        /// <summary>
        /// Load a page into software
        /// </summary>
        /// <param name="selectLastOnPage"></param>
        private void LoadPage(bool selectLastOnPage = false)
        {
            int page = this._pages.SelectedPage;

            var folders = this._pages.GetFolders();

            _pages.SelectedPage = page;

            if (!selectLastOnPage)
            {
                if (folders.Count > 0)
                {
                    folders[0].Selected = true;
                }
            }
            else
            {
                if (folders.Count > 0)
                {
                    folders[folders.Count - 1].Selected = true;
                }
            }

            for (int i = 0; i < Constants.QuantityPerPage; i++)
            {
                var pictureBox = this.FindPictureBoxByNumber(i + 1);
                if (i < folders.Count)
                {
                    var folder = folders.ElementAt(i);
                    pictureBox.BackColor = System.Drawing.Color.Red;
                    pictureBox.Visible = true;
                    SetPicture(pictureBox, folder.ImagePath);
                }
                else
                {
                    pictureBox.Visible = false;
                }
            }

            this.SetSelectedFolder();
        }

        /// <summary>
        /// Select a song by remote control
        /// </summary>
        /// <param name="media"></param>
        public void SelectRemoteMedia(TigerMedia media)
        {
            try
            {
                _sharedList.DecreaseCredit();

                var mediaIndex = -1;

                _sharedList.InsertNewMedia<TigerMedia>(media);

                ListViewItem lv = GetMediaItem(media, mediaIndex);

                InsertMediaToSelectedList((ListViewItem)lv.Clone());
                _player.AddMediaToPlayList(media);
                this.LoadCredits();
                if (!_player.IsPlaying)
                {
                    this.Play();
                }
                _lastFormOperation = DateTime.Now;
            }
            catch (NoCreditsException ex)
            {
                this._lbCredits.Text = ex.Message;
            }
        }

        #endregion

        #region Timer Methods
        /// <summary>
        /// Start full screen timer manager
        /// </summary>
        private void StartFullScreenTimer()
        {
            _timer = new System.Timers.Timer(4500);
            _timer.Elapsed += PlayerTimerMethod;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        /// <summary>
        /// Start letter panel timer manager
        /// </summary>
        private void StartLetterPanelTimer()
        {
            _timer = new System.Timers.Timer(2000);
            _timer.Elapsed += PanelLetterTimerMethod;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        /// <summary>
        /// full screen timer event
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void PlayerTimerMethod(Object source, ElapsedEventArgs e)
        {
            if (_player.IsPlaying && !debug)
            {
                if (DateTime.Now.Subtract(_lastFormOperation).Seconds > 7)
                {
                    if (!_player.IsFullScreen)
                    {
                        this.BeginInvoke((Action)(() =>
                        {
                            this.SetFullScreen();
                        }));
                    }

                }
            }
        }

        /// <summary>
        /// Letter panel timer event
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void PanelLetterTimerMethod(Object source, ElapsedEventArgs e)
        {
            if (DateTime.Now.Subtract(letterPanel1.LastOperation).Seconds >= 3)
            {
                this.BeginInvoke((Action)(() =>
                {
                    if (letterPanel1.IsVisible)
                    {
                        string letter = letterPanel1.Close();
                        FindPageByLetter(letter);
                        StartFullScreenTimer();
                    }

                }));

            }
        }

        private void SetSongsFocus()
        {
            this._listViewFolderSongs.Focus();
            if (this._listViewFolderSongs.Items.Count > 0)
            {
                this._listViewFolderSongs.TopItem = this._listViewFolderSongs.Items[0];
            }
            
        }

        #endregion

        #region Queue
        private void ReadFromQueue(object sender, ElapsedEventArgs e)
        {
            var queue = new MessageQueue(_configurationService.GetConfiguration<string>("PrivateQueue"));
            TigerBoxMessage message = new TigerBoxMessage();
            Object o = new Object();
            System.Type[] arrTypes = new System.Type[2];
            arrTypes[0] = message.GetType();
            arrTypes[1] = o.GetType();
            queue.Formatter = new XmlMessageFormatter(arrTypes);
            var readed = queue.Receive();
            if (readed != null)
            {
                message = ((TigerBoxMessage)readed.Body);
                if (message.MessageType == Enums.MessageType.Media)
                {
                    this.BeginInvoke((Action)(() =>
                    {
                        SelectRemoteMedia(message.Media);
                    }));

                }
                else
                {
                    this.BeginInvoke((Action)(() =>
                    {
                        ExecuteFormActions(message.Action);
                    }));
                }

            }


        }
        #endregion
    }
}
