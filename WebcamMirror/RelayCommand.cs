using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebcamMirror
{
    using System.Windows.Input;

    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = _ => execute();
            this.canExecute = _ => canExecute();
        }

        public RelayCommand(Action<object> execute)
        {
            this.execute = execute;
            this.canExecute = _ => true;
        }

        public RelayCommand(Action execute)
        {
            this.execute = _ => execute();
            this.canExecute = _ => true;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
