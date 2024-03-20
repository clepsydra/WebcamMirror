using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WebcamMirror
{
    using System.Windows.Input;

    public class BitmapSourceKeeper : ViewModelBase
    {
        public BitmapSourceKeeper(int index, Action<int> selectedCallback)
        {
            this.Index = index;
            this.selectedCallback = selectedCallback;
            this.SelectedCommand = new RelayCommand(this.ExecuteSelected);
        }

        #region ImageSource
        private BitmapSource imageSource;
        private readonly Action<int> selectedCallback;

        public BitmapSource ImageSource
        {
            get => this.imageSource;

            set
            {
                this.imageSource = value;
                this.OnPropertyChanged();
            }
        }
        #endregion

        public int Index { get; }

        #region SelectedCommand
        public ICommand SelectedCommand { get; }
        
        private void ExecuteSelected()
        {
            this.selectedCallback(this.Index);
        }
        #endregion

    }
}
