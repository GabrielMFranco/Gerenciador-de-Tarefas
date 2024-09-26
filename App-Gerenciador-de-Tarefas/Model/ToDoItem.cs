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
		public DateTime Create {
			get => Get<DateTime>();
			set => Set(value);
		}
		public DateTime Completed {
			get => Get<DateTime>();
			set => Set(value);
		}
		public bool IsCompleted {
			get => Get<bool>();
			set => Set(value);
		}
	}
}