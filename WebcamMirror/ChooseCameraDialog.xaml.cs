using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WebcamMirror
{
    /// <summary>
    /// Interaction logic for ChooseCameraDialog.xaml
    /// </summary>
    public partial class ChooseCameraDialog : Window
    {
        public ChooseCameraDialog()
        {
            this.InitializeComponent();
            this.Closing += ChooseCameraDialog_Closing;
            this.DataContext = new ChooseCameraViewModel(this.CloseDialog);

            this.Loaded += ChooseCameraDialog_Loaded;
        }

        private void CloseDialog(bool result)
        {
            this.DialogResult = result;
            this.Close();
        }

        private void ChooseCameraDialog_Loaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Initialize();
        }

        private ChooseCameraViewModel ViewModel => this.DataContext as ChooseCameraViewModel;

        private void ChooseCameraDialog_Closing(object? sender, CancelEventArgs e)
        {
            if (!this.ViewModel.IsCapturing)
            {
                return;
            }

            this.ViewModel.StopCapturing();
            e.Cancel = true;
        }

        public int SelectedIndex => this.ViewModel.SelectedIndex;
    }
}
