using Emgu.CV;
using System.Windows;
using System.Windows.Input;

namespace WebcamMirror
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int selectedIndex;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ChooseCameraDialog dialog = new ChooseCameraDialog();
            var result = dialog.ShowDialog();

            if (result != true)
            {
                this.Close();
                return;
            }

            this.selectedIndex = dialog.SelectedIndex;

            Task.Run(CaptureInfinite);
        }

        private bool isClosing = false;

        private void CaptureInfinite()
        {
            using (var capture = new VideoCapture(this.selectedIndex, VideoCapture.API.DShow))
            {
                while (!isClosing)
                {
                    if (!capture.Grab())
                    {
                        continue;
                    }

                    Mat mat = new Mat();
                    if (!capture.Retrieve(mat))
                    {
                        continue;
                    }

                    var bitmap = mat.ToBitmap();

                    bitmap = Utils.CutToSquare(bitmap);

                    Dispatcher.Invoke(() => CamImage.Source = Utils.ConvertBitmapToBitmapSource(bitmap));

                    Thread.Sleep(50);
                }
            }

            this.Dispatcher.Invoke(this.Close);
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            this.DragMove();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.isClosing = true;
                return;
            }

            if (e.Key == Key.Add && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                this.CamImage.Width *= 1.1;
                this.CamImage.Height *= 1.1;

                var newCornerRadius = new CornerRadius(Mask.CornerRadius.TopLeft * 1.1, Mask.CornerRadius.TopRight * 1.1, Mask.CornerRadius.BottomRight * 1.1, Mask.CornerRadius.BottomLeft * 1.1);
                Mask.CornerRadius = newCornerRadius;
                ImageBorder.CornerRadius = newCornerRadius;
                return;
            }

            if (e.Key == Key.Subtract && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                this.CamImage.Width /= 1.1;
                this.CamImage.Height /= 1.1;

                var newCornerRadius = new CornerRadius(Mask.CornerRadius.TopLeft / 1.1, Mask.CornerRadius.TopRight / 1.1, Mask.CornerRadius.BottomRight / 1.1, Mask.CornerRadius.BottomLeft / 1.1);
                Mask.CornerRadius = newCornerRadius;
                ImageBorder.CornerRadius = newCornerRadius;
                return;
            }

            if (e.Key == Key.R && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                var newCornerRadius = new CornerRadius(Mask.CornerRadius.TopRight, Mask.CornerRadius.BottomRight, Mask.CornerRadius.BottomLeft, Mask.CornerRadius.TopLeft);
                Mask.CornerRadius = newCornerRadius;
                ImageBorder.CornerRadius = newCornerRadius;
            }
        }
    }
}