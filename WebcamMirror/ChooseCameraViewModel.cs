namespace WebcamMirror
{
    using Emgu.CV;
    using System.Collections.ObjectModel;

    public class ChooseCameraViewModel : ViewModelBase
    {
        private readonly Action<bool> close;

        public ChooseCameraViewModel(Action<bool> close)
        {
            this.close = close;
        }

        public void Initialize()
        {
            this.IsLoading = true;
            Task.Run(StartReading);
        }

        #region IsLoading
        private bool isLoading;

        public bool IsLoading
        {
            get => this.isLoading;

            set
            {
                this.isLoading = value;
                this.OnPropertyChanged();
            }
        }
        #endregion

        private List<VideoCapture> Captures = new List<VideoCapture>();

        public ObservableCollection<BitmapSourceKeeper> BitmapSources { get; } = new ObservableCollection<BitmapSourceKeeper>();

        private void StartReading()
        {
            this.IsCapturing = true;

            for (int index = 0; index < 10; index++)
            {
                try
                {
                    var capture = new VideoCapture(index, VideoCapture.API.DShow);
                    if (capture.Width == 0)
                    {
                        capture.Dispose();
                        continue;
                    }

                    this.Captures.Add(capture);
                    RunOnUiThread(() => this.BitmapSources.Add(new BitmapSourceKeeper(index, this.CameraSelected)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            this.IsLoading = false;

            Task.Run(this.CaptureInfinite);
        }

        private bool isStopCapturing = false;

        public bool IsCapturing { get; set; }

        public void StopCapturing()
        {
            this.isStopCapturing = true;
        }

        private void CaptureInfinite()
        {
            while (!this.isStopCapturing)
            {
                for (int index = 0; index < this.Captures.Count; index++)
                {
                    var capture = this.Captures[index];

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
                    var squaredBitmap = Utils.CutToSquare(bitmap);

                    RunOnUiThread(() => this.BitmapSources[index].ImageSource = Utils.ConvertBitmapToBitmapSource(squaredBitmap));
                }

                Thread.Sleep(10);
            }

            foreach (var capture in this.Captures)
            {
                capture.Dispose();
            }

            this.IsCapturing = false;

            RunOnUiThread(() => this.close(this.SelectedIndex != -1));
        }

        private void CameraSelected(int selectedIndex)
        {
            this.IsCameraSelected = true;
            this.SelectedIndex = selectedIndex;
            this.StopCapturing();
        }

        #region IsCameraSelected
        private bool isCameraSelected;

        public bool IsCameraSelected
        {
            get => this.isCameraSelected;

            set
            {
                this.isCameraSelected = value;
                this.OnPropertyChanged();
            }
        }
        #endregion

        public int SelectedIndex { get; private set; } = -1;
    }
}
