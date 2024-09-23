using App_Gerenciador_de_Tarefas.Foundation;
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
public class RelayCommand<T> : ICommand {
	public RelayCommand(Action<T> execute, Predicate<T> canExecute = null) {
		//Action metodoSemRetornoESemParametro; // void Acao();
		//Action<int> metodoSemRetornoComUmParametroInt; // void Seila(int x);
		//Action<decimal, int> metodoSemRetComDoisParametosUmDecUmInt; // void Somar(decimal x, int y);

		//Func<int> metodoQueRetornaIntSemParametros; // int Ler();
		//Func<decimal, int> metodoQueRetornaIntComParamDec; // int Ler(decimal x);
		//Func<string, decimal, int> sdgfsag; // int Ler(string x, decimal y)

		//Predicate<T> aaa; // é o mesmo que Func<T, bool>
		_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		_canExecute = canExecute;
	}

	private readonly Action<T> _execute;
	private readonly Predicate<T> _canExecute;

	public event EventHandler CanExecuteChanged {
		add { CommandManager.RequerySuggested += value; }
		remove { CommandManager.RequerySuggested -= value; }
	}

	public bool CanExecute(object parameter) {
		return _canExecute == null || _canExecute((T)parameter);
	}

	public void Execute(object parameter) {

		T transformado = (T)parameter;

		_execute(transformado);
	}
}