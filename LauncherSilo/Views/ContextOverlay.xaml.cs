using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

using LauncherSilo.Core.Models;
using LauncherSilo.Models;
using LauncherSilo.Utility;

namespace LauncherSilo.Views
{
    /// <summary>
    /// ContextOverlay.xaml の相互作用ロジック
    /// </summary>
    public partial class ContextOverlay : Overlay
    {
        public Dictionary<System.Windows.Controls.Button, MenuTreeNode> MenuNodeDictionary { get; set; } = new Dictionary<System.Windows.Controls.Button, MenuTreeNode>();

        private ImageBrush CommandImageBrush { get; set; } = null;
        private ImageBrush FolderImageBrush { get; set; } = null;
        private ImageBrush UnknownFileImageBrush { get; set; } = null;
        private ImageBrush ConfigImageBrush { get; set; } = null;
        private ImageBrush SystemImageBrush { get; set; } = null;


        public ContextOverlay() : base()
        {
            CommandImageBrush = new ImageBrush()
            {
                Stretch = Stretch.Uniform,
                Viewport = new Rect(0.25, 0.25, 0.5, 0.5),
                ImageSource = MaterialDesignIconSourceStorage.FindPackIconImage(MaterialDesignThemes.Wpf.PackIconKind.Console, new SolidColorBrush(Colors.White), 0),
            };
            FolderImageBrush = new ImageBrush()
            {
                Stretch = Stretch.Uniform,
                Viewport = new Rect(0.25, 0.25, 0.5, 0.5),
                ImageSource = MaterialDesignIconSourceStorage.FindPackIconImage(MaterialDesignThemes.Wpf.PackIconKind.Folder, new SolidColorBrush(Colors.White), 0)
            };
            UnknownFileImageBrush = new ImageBrush()
            {
                Stretch = Stretch.Uniform,
                Viewport = new Rect(0.25, 0.25, 0.5, 0.5),
                ImageSource = MaterialDesignIconSourceStorage.FindPackIconImage(MaterialDesignThemes.Wpf.PackIconKind.File, new SolidColorBrush(Colors.White), 0),
            };
            SystemImageBrush = new ImageBrush()
            {
                Stretch = Stretch.Uniform,
                Viewport = new Rect(0.25, 0.25, 0.5, 0.5),
                ImageSource = MaterialDesignIconSourceStorage.FindPackIconImage(MaterialDesignThemes.Wpf.PackIconKind.ExitToApp, new SolidColorBrush(Colors.White), 0),
            };

            InitializeComponent();


        }
        protected override void OnPreShowOverlay() 
        {
            BuildMainPanel();
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
                if (SystemMenuNode.IconSource != null)
                {
                    ImageBrush iconImageBrush = new ImageBrush()
                    {
                        Stretch = Stretch.Uniform,
                        Viewport = new Rect(0.25, 0.25, 0.5, 0.5),
                        ImageSource = SystemMenuNode.IconSource,
                    };
                    NewButton.Background = iconImageBrush;
                }
                else
                {
                    NewButton.Background = SystemImageBrush;
                }
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

    }
}
