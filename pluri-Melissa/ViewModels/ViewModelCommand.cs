


//relay command from v to vm

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.ViewModels
{
    class ViewModelCommand : ICommand
    {


        /*

        //fields 
        private readonly Action<object> _executeAction;
        private readonly Predicate<object> _canExecuteAction;
        private Action sendEmail;

        public ViewModelCommand(Action<object> value)
        {
        }

        //constructors
        /*public ViewModelCommand(Action<object> executeAction, Predicate<object> canExecuteAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        public ViewModelCommand(Action<object> executeAction, object canExecuteSendEmailCommand)
        {
            _executeAction = executeAction;
            _canExecuteAction = null;
        }

        /*public ViewModelCommand(Action sendEmail)
        {
            this.sendEmail = sendEmail;
        }


        //event to notify the GUI when a command's executability changes (like enabling or disabling a button)
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }



        //methods
        public bool CanExecute(object? parameter)
        {
            return _canExecuteAction == null ? true : _canExecuteAction(parameter);
        }

        public void Execute(object? parameter)
        {
            _executeAction(parameter);
        }
    

        */







        private readonly Func<object, bool> canExecute;
        private readonly Action<object?> execute;

        public ViewModelCommand(Action<object?> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public event EventHandler? SCanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            execute(parameter);
        }

    }
}