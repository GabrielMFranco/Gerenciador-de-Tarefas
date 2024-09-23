using App_Gerenciador_de_Tarefas.Foundation;
using System.ComponentModel;

namespace GerenciadorDeTarefas.Model {
	public class ToDoItem : MyObservableObject {
		public string Title { get; set; }
		public string Description { get; set; }
		public string Create { get; set; }
		public string Completed { get; set; }

		private bool _isCompleted;
		public bool IsCompleted {
			get => _isCompleted;
			set {
				Set(ref _isCompleted, value, nameof(IsCompleted));
				OnPropertyChanged(nameof(IsCompleted));
			}
		}
	}

	public class MyObservableObject : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;

		protected bool Set<T>(ref T field, T value, string propertyName) {
			if (!EqualityComparer<T>.Default.Equals(field, value)) {
				field = value;
				OnPropertyChanged(propertyName);
				return true;
			}
			return false;
		}

		protected void OnPropertyChanged(string propertyName) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

