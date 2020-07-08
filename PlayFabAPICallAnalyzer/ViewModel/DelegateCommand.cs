using System;
using System.Windows.Input;

namespace PlayFabAPICallAnalyzer.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _executeAction;

        public DelegateCommand(Action<object> executeAction)
        {
            _executeAction = executeAction;
        }

        public void Execute(object parameter) => _executeAction(parameter);

        public bool CanExecute(object parameter) => true;

        public event EventHandler CanExecuteChanged;
    }
}
