namespace Tiger.Player
{
    partial class TigerPlayer
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TigerPlayer));
            this._mediaPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)(this._mediaPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // _mediaPlayer
            // 
            this._mediaPlayer.Enabled = true;
            this._mediaPlayer.Location = new System.Drawing.Point(0, 0);
            this._mediaPlayer.Name = "_mediaPlayer";
            this._mediaPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("_mediaPlayer.OcxState")));
            this._mediaPlayer.Size = new System.Drawing.Size(592, 450);
            this._mediaPlayer.TabIndex = 0;
            // 
            // TigerPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._mediaPlayer);
            this.Name = "TigerPlayer";
            this.Size = new System.Drawing.Size(592, 450);
            ((System.ComponentModel.ISupportInitialize)(this._mediaPlayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer _mediaPlayer;
    }
}
