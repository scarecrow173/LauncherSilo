using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using MahApps.Metro.Controls;

namespace LauncherSilo.FileFinderPlugin
{
    /// <summary>
    /// FileFinderView.xaml の相互作用ロジック
    /// </summary>
    public partial class FileFinderView : MetroWindow
    {
        private FileFinderViewModel FileFinderVM = null;
        public FileFinderView(FileFinderViewModel ViewModel)
        {
            FileFinderVM = ViewModel;
            DataContext = FileFinderVM;
            InitializeComponent();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FileFinderVM.RunSelectedItem();
            Hide();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                var direction = FocusNavigationDirection.Next;
                (FocusManager.GetFocusedElement(this) as FrameworkElement)?.MoveFocus(new TraversalRequest(direction));
            }
        }

        private void MetroWindow_Deactivated(object sender, EventArgs e)
        {
            Hide();
        }


    }
}
