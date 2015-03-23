using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NathanHarrenstein.Input
{
    public sealed class AsyncDelegateCommand : ICommand
    {
        public Predicate<object> canExecute;
        public Func<object, Task> executeActionAsync;
        private bool canExecuteCache;

        public AsyncDelegateCommand(Func<object, Task> executeActionAsync)
            : this(executeActionAsync, o => true)
        {
        }

        public AsyncDelegateCommand(Func<object, Task> executeActionAsync, Predicate<object> canExecute)
        {
            this.executeActionAsync = executeActionAsync;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            bool temp = canExecute(parameter);

            if (canExecuteCache != temp)
            {
                canExecuteCache = temp;

                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }

            return canExecuteCache;
        }

        public async void Execute(object parameter)
        {
            await executeActionAsync(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}