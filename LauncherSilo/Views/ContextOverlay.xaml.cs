using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

using LauncherSilo.Core.Models;
using LauncherSilo.Models;

namespace LauncherSilo.Views
{
    /// <summary>
    /// ContextOverlay.xaml の相互作用ロジック
    /// </summary>
    public partial class ContextOverlay : Window
    {
        #region DependencyProperties

        #region AltF4Cancel

        public bool AltF4Cancel
        {
            get { return (bool)GetValue(AltF4CancelProperty); }
            set { SetValue(AltF4CancelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AltF4Cancel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AltF4CancelProperty =
            DependencyProperty.Register(
                "AltF4Cancel",
                typeof(bool),
                typeof(ContextOverlay),
                new PropertyMetadata(true));

        #endregion

        #region ShowSystemMenu

        public bool ShowSystemMenu
        {
            get { return (bool)GetValue(ShowSystemMenuProperty); }
            set { SetValue(ShowSystemMenuProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowSystemMenu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowSystemMenuProperty =
            DependencyProperty.Register(
                "ShowSystemMenu",
                typeof(bool),
                typeof(ContextOverlay),
                new PropertyMetadata(
                    false,
                    ShowSystemMenuPropertyChanged));

        private static void ShowSystemMenuPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is ContextOverlay window)
            {

                window.SetShowSystemMenu((bool)e.NewValue);

            }

        }

        #endregion

        #region ClickThrough

        public bool ClickThrough
        {
            get { return (bool)GetValue(ClickThroughProperty); }
            set { SetValue(ClickThroughProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ClickThrough.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClickThroughProperty =
            DependencyProperty.Register(
                "ClickThrough",
                typeof(bool),
                typeof(ContextOverlay),
                new PropertyMetadata(
                    false,
                    ClickThroughPropertyChanged));

        private static void ClickThroughPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ContextOverlay window)
            {
                window.SetClickThrough((bool)e.NewValue);
            }

        }

        #endregion

        #endregion

        #region const values

        private const int GWL_STYLE = (-16); // ウィンドウスタイル
        private const int GWL_EXSTYLE = (-20); // 拡張ウィンドウスタイル

        private const int WS_SYSMENU = 0x00080000; // システムメニュを表示する
        private const int WS_EX_TRANSPARENT = 0x00000020; // 透過ウィンドウスタイル

        private const int WM_SYSKEYDOWN = 0x0104; // Alt + 任意のキー の入力

        private const int VK_F4 = 0x73;

        #endregion

        #region Win32Apis

        [DllImport("user32")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwLong);

        #endregion

        public Dictionary<System.Windows.Controls.Button, MenuTreeNode> MenuNodeDictionary { get; set; } = new Dictionary<System.Windows.Controls.Button, MenuTreeNode>();

        private ImageBrush CommandImageBrush { get; set; } = null;
        private ImageBrush FolderImageBrush { get; set; } = null;
        private ImageBrush UnknownFileImageBrush { get; set; } = null;
        private ImageBrush ConfigImageBrush { get; set; } = null;
        private ImageBrush SystemImageBrush { get; set; } = null;


        public ContextOverlay()
        {

            //// 透過背景
            //this.WindowStyle = WindowStyle.None;
            //this.AllowsTransparency = true;
            //this.Background = new SolidColorBrush(Colors.Transparent);

            //// 全画面表示
            //this.WindowState = WindowState.Maximized;

            // 最前面表示
            this.Topmost = true;

            //タスクバーに表示しない
            this.ShowInTaskbar = false;

            CommandImageBrush = new ImageBrush()
            {
                Stretch = Stretch.Uniform,
                Viewport = new Rect(0.25, 0.25, 0.5, 0.5),
                ImageSource = CreateImageSource(MaterialDesignThemes.Wpf.PackIconKind.Console, new SolidColorBrush(Colors.White), 0),
            };
            FolderImageBrush = new ImageBrush()
            {
                Stretch = Stretch.Uniform,
                Viewport = new Rect(0.25, 0.25, 0.5, 0.5),
                ImageSource = CreateImageSource(MaterialDesignThemes.Wpf.PackIconKind.Folder, new SolidColorBrush(Colors.White), 0)
            };
            UnknownFileImageBrush = new ImageBrush()
            {
                Stretch = Stretch.Uniform,
                Viewport = new Rect(0.25, 0.25, 0.5, 0.5),
                ImageSource = CreateImageSource(MaterialDesignThemes.Wpf.PackIconKind.File, new SolidColorBrush(Colors.White), 0),
            };
            SystemImageBrush = new ImageBrush()
            {
                Stretch = Stretch.Uniform,
                Viewport = new Rect(0.25, 0.25, 0.5, 0.5),
                ImageSource = CreateImageSource(MaterialDesignThemes.Wpf.PackIconKind.ExitToApp, new SolidColorBrush(Colors.White), 0),
            };

            InitializeComponent();


        }
        public void ShowCurrentDisplayOverlay()
        {
            BuildMainPanel();
            System.Drawing.Point dp = System.Windows.Forms.Cursor.Position;
            System.Windows.Point wp = new System.Windows.Point(dp.X, dp.Y);
            WindowState = WindowState.Normal;
            Top = wp.Y;
            Left = wp.X;
            Width = 0;
            Height = 0;
            WindowState = WindowState.Maximized;
            Show();
        }
        public void BuildMainPanel()
        {
            MainPanel.Children.Clear();
            MenuNodeDictionary.Clear();
            foreach (var MenuNode in App.AppConfig.ContextMenuNodes)
            {
                var NodeItems = BuildMainPanelItem(MenuNode);
                if (NodeItems != null)
                {
                    foreach (var NodeItem in NodeItems)
                    {
                        MainPanel.Children.Add(NodeItem);
                    }
                }
            }
            foreach (var SystemMenuNode in App.SystemMenuNodes)
            {
                System.Windows.Controls.Button NewButton = new System.Windows.Controls.Button();
                MenuNodeDictionary.Add(NewButton, SystemMenuNode);
                NewButton.Width = System.Windows.SystemParameters.WorkArea.Width / App.AppConfig.HorizonalTileSize;
                NewButton.Height = NewButton.Width;
                NewButton.VerticalContentAlignment = VerticalAlignment.Bottom;
                NewButton.Content = SystemMenuNode.Name;
                NewButton.Click += SystemButton_Click;
                NewButton.Background = SystemImageBrush;
                NewButton.Background.Opacity = 1.0;
                MainPanel.Children.Add(NewButton);
            }
        }
        public UIElement[] BuildMainPanelItem(MenuTreeNode Node)
        {
            if (!Node.IsVisible)
            {
                return null;
            }
            List<UIElement> NewItems = new List<UIElement>();
            LauncherCommandInfo Command = App.AppConfig.FindCommandInfo(Node.CommandInfoID);
            if (Command != null && Command.Type != CommandExecuteType.None)
            {
                string[] Commands = App.Executer.ParseCommandList(Command);
                if (Commands.Length != 0)
                {
                    System.Windows.Controls.Button NewButton = new System.Windows.Controls.Button();
                    MenuNodeDictionary.Add(NewButton, Node);
                    NewButton.Click += NewButton_Click;

                    NewButton.Width = System.Windows.SystemParameters.WorkArea.Width / App.AppConfig.HorizonalTileSize;
                    NewButton.Height = NewButton.Width;
                    NewButton.VerticalContentAlignment = VerticalAlignment.Bottom;
                    NewButton.Content = Command.Name;
                    NewButton.ToolTip = Command.Description;

                    if (Command.Type == CommandExecuteType.Command)
                    {
                        NewButton.Background = CommandImageBrush;
                        NewButton.Background.Opacity = 1.0;
                    }
                    else if (Command.Type == CommandExecuteType.FileOpen)
                    {
                        App.Executer.ParseFileCommand(Commands[0], out string FilePath, out string Args);
                        if (System.IO.File.Exists(FilePath))
                        {
                            ImageBrush iconImageBrush = new ImageBrush()
                            {
                                Stretch = Stretch.Uniform,
                                Viewport = new Rect(0.25, 0.25, 0.5, 0.5),
                                ImageSource = Misc.IconImageStorage.FindSystemIconImage(FilePath)
                            };
                            NewButton.Background = iconImageBrush;
                            NewButton.Background.Opacity = 1.0;
                        }
                        else if (System.IO.Directory.Exists(FilePath))
                        {
                            NewButton.Background = FolderImageBrush;
                            NewButton.Background.Opacity = 1.0;
                        }
                        else
                        {
                            NewButton.Background = UnknownFileImageBrush;
                            NewButton.Background.Opacity = 1.0;
                        }
                    }

                    NewItems.Add(NewButton);
                }
            }
            foreach (var Child in Node.Children)
            {
                var ChildItems = BuildMainPanelItem(Child);
                if (ChildItems != null)
                {
                    NewItems.AddRange(ChildItems);
                }
            }

            return NewItems.ToArray();

        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MenuTreeNode ClickedNode = null;
            System.Windows.Controls.Button ClickedButton = (System.Windows.Controls.Button)sender;
            MenuNodeDictionary.TryGetValue(ClickedButton, out ClickedNode);
            if (ClickedNode != null)
            {
                LauncherCommandInfo Command = App.AppConfig.FindCommandInfo(ClickedNode.CommandInfoID);
                if (Command != null)
                {
                    App.Executer.Execute(Command);
                }
            }
        }
        private void SystemButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MenuTreeNode ClickedNode = null;
            System.Windows.Controls.Button ClickedButton = (System.Windows.Controls.Button)sender;
            MenuNodeDictionary.TryGetValue(ClickedButton, out ClickedNode);
            SystemMenuTreeNode SystemMenuNode = ClickedNode as SystemMenuTreeNode;
            if (SystemMenuNode != null)
            {
                SystemMenuNode.Execute();
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {

            //システムメニュを非表示
            this.SetShowSystemMenu(this.ShowSystemMenu);

            //クリックをスルー
            this.SetClickThrough(this.ClickThrough);

            //Alt + F4 を無効化
            var handle = new WindowInteropHelper(this).Handle;
            var hwndSource = HwndSource.FromHwnd(handle);
            hwndSource.AddHook(WndProc);

            base.OnSourceInitialized(e);

        }

        protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr IParam, ref bool handled)
        {

            //Alt + F4 が入力されたら
            if (msg == WM_SYSKEYDOWN && wParam.ToInt32() == VK_F4)
            {

                if (this.AltF4Cancel)
                {

                    //処理済みにセットする
                    //(Windowは閉じられなくなる)
                    handled = true;

                }

            }

            return IntPtr.Zero;

        }

        /// <summary>
        /// システムメニュの表示を切り替える
        /// </summary>
        /// <param name="value"><see langword="true"/> = 表示, <see langword="false"/> = 非表示</param>
        protected void SetShowSystemMenu(bool value)
        {

            try
            {

                var handle = new WindowInteropHelper(this).Handle;

                int windowStyle = GetWindowLong(handle, GWL_STYLE);

                if (value)
                {
                    windowStyle |= WS_SYSMENU; //フラグの追加
                }
                else
                {
                    windowStyle &= ~WS_SYSMENU; //フラグを消す
                }

                SetWindowLong(handle, GWL_STYLE, windowStyle);

            }
            catch
            {

            }

        }

        /// <summary>
        /// クリックスルーの設定
        /// </summary>
        /// <param name="value"><see langword="true"/> = クリックをスルー, <see langword="false"/>=クリックを捉える</param>
        protected void SetClickThrough(bool value)
        {

            try
            {

                var handle = new WindowInteropHelper(this).Handle;

                int extendStyle = GetWindowLong(handle, GWL_EXSTYLE);

                if (value)
                {
                    extendStyle |= WS_EX_TRANSPARENT; //フラグの追加
                }
                else
                {
                    extendStyle &= ~WS_EX_TRANSPARENT; //フラグを消す
                }

                SetWindowLong(handle, GWL_EXSTYLE, extendStyle);

            }
            catch
            {

            }

        }

        private void Window_MouseButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Point MousePos = e.GetPosition(this);
            
            //App.NnotifyIcon.ShowMainContextMenu(MousePos);

            this.Hide();
        }

        public static ImageSource CreateImageSource(object value, Brush foregroundBrush, double penThickness)
        {
            var packIcon = new MaterialDesignThemes.Wpf.PackIcon { Kind = (MaterialDesignThemes.Wpf.PackIconKind)value };

            var geometryDrawing = new GeometryDrawing
            {
                Geometry = Geometry.Parse(packIcon.Data),
                Brush = foregroundBrush,
                Pen = new Pen(foregroundBrush, penThickness)
            };

            var drawingGroup = new DrawingGroup { Children = { geometryDrawing }, Transform = new ScaleTransform(1, -1) };

            return new DrawingImage { Drawing = drawingGroup };
        }
    }
}
