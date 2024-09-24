using App_Gerenciador_de_Tarefas.Foundation;
using System.ComponentModel;

namespace GerenciadorDeTarefas.Model {
	public class ToDoItem : ObservableObject {
		public string Title {
			get => Get<string>();
			set => Set(value);
		}
		public string Description {
			get => Get<string>();
			set => Set(value);
		}
		public string Create {
			get => Get<string>();
			set => Set(value);
		}
		public string Completed {
			get => Get<string>();
			set => Set(value);
		}
		public bool IsCompleted {
			get => Get<bool>();
			set => Set(value);
		}
	}
}