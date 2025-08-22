using System.Windows.Input;

namespace TransStarterTest.Commands
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool>? _canExecute;
        private bool _isRunning;

        public AsyncRelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
        {
            _execute = execute; _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => !_isRunning && (_canExecute?.Invoke() ?? true);
        public event EventHandler? CanExecuteChanged;
        public async void Execute(object? parameter)
        {
            if (_isRunning) return;
            _isRunning = true; CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            try { await _execute(); }
            finally { _isRunning = false; CanExecuteChanged?.Invoke(this, EventArgs.Empty); }
        }
    }

}