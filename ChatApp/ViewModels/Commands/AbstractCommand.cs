using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ChatApp.ViewModels.Commands
{
    abstract class AbstractCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public event EventHandler Executed;

        protected void RaiseCanExecuteChanged(EventArgs e)
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, e);
        }

        protected void RaiseExecuted(EventArgs e)
        {
            if (Executed != null)
                Executed(this, e);
        }

        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);
    }
}
