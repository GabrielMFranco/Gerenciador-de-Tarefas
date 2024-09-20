using System.Windows.Input;

namespace WpfApp1.Foundation;

public class RelayCommand : ICommand {
	public RelayCommand(Action actionToPerform) : this(actionToPerform, null) { }
	public RelayCommand(Action actionToPerform, Func<bool>? whenCanPerform) {
		_actionToPerform = actionToPerform;
		_whenCanPerform = whenCanPerform;
	}

	public event EventHandler CanExecuteChanged {
		add => CommandManager.RequerySuggested += value;
		remove => CommandManager.RequerySuggested -= value;
	}

	private readonly Action _actionToPerform;
	private readonly Func<bool>? _whenCanPerform;

	public bool CanExecute()
		=> CanExecute(null);
	public bool CanExecute(object? parameter)
		=> _whenCanPerform?.Invoke() ?? true;
	public void Execute()
		=> Execute(null);
	public void Execute(object? parameter)
		=> _actionToPerform?.Invoke();
}