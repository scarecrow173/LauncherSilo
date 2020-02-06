namespace LauncherSilo.Views
{
    partial class MainNotifyIconView
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainNotifyIconView));
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.MainContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.ContextMenuStrip = this.MainContextMenuStrip;
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "LauncherSilo";
            this.NotifyIcon.Visible = true;
            // 
            // MainContextMenuStrip
            // 
            this.MainContextMenuStrip.Name = "MainContextMenuStrip";
            this.MainContextMenuStrip.Size = new System.Drawing.Size(61, 4);
            this.MainContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.MainContextMenuStrip_Opening);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon NotifyIcon;
        public System.Windows.Forms.ContextMenuStrip MainContextMenuStrip;
    }
}
