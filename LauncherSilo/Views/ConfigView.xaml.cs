using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using Misc;
using MahApps.Metro.Controls;

namespace LauncherSilo.Views
{
    /// <summary>
    /// ConfigView.xaml の相互作用ロジック
    /// </summary>
    public partial class ConfigView : MetroWindow
    {
        private System.Windows.Forms.KeysConverter converter = new System.Windows.Forms.KeysConverter();

        public ConfigView()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
        private void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            var callback = new DispatcherOperationCallback(obj =>
            {
                ((DispatcherFrame)obj).Continue = false;
                return null;
            });
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);
            Dispatcher.PushFrame(frame);
        }

        private void WatermarkTextBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void WatermarkTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void WatermarkTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            try
            {
                converter.ConvertFromString(e.Text.ToUpper());
                textBox.Text = string.Empty;
            }
            catch(Exception ex)
            {
                LogStatics.Debug(ex.ToString());
                e.Handled = false;
                textBox.Text = string.Empty;
            }
        }

        private void WatermarkTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = e.Text.ToUpper();
        }

        private void WatermarkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.Text = textBox.Text.ToUpper();
            //textBox.Text = e.Changes;
        }
    }
}
