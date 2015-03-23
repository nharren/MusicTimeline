using System;
using System.Windows.Input;

namespace NathanHarrenstein.Input
{
    public sealed class DelegateCommand : ICommand
    {
        private readonly Predicate<object> canExecute;
        private readonly Action<object> executeAction;
        private bool canExecuteCache;

        public DelegateCommand(Action<object> executeAction)
            : this(executeAction, o => true)
        {
        }

        public DelegateCommand(Action<object> executeAction, Predicate<object> canExecute)
        {
            this.executeAction = executeAction;
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

        public void Execute(object parameter)
        {
            executeAction(parameter);
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