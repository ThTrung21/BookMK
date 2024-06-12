using System;
using System.Collections.Generic;
using System.Windows.Input;
using Serilog;

namespace BookMK.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        private static readonly ILogger _logger = Log.ForContext(typeof(RelayCommand));

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            bool result = _canExecute == null || _canExecute();
            _logger.Information("CanExecute: {CanExecute}", result);
            return result;
        }

        public void Execute(object parameter)
        {
            _execute();
            _logger.Information("Command executed.");
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;
        private static readonly ILogger _logger = Log.ForContext(typeof(RelayCommand<T>));

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            bool result = _canExecute == null || _canExecute((T)parameter);
            _logger.Information("CanExecute: {CanExecute}", result);
            return result;
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
            _logger.Information("Command executed.");
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
