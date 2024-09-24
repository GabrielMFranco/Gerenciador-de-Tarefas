using App_Gerenciador_de_Tarefas.Foundation;
using System.ComponentModel;

namespace GerenciadorDeTarefas.Model {
	public class ToDoItem : ObservableObject {
		public string Title { get; set; }
		public string Description { get; set; }
		public string Create { get; set; }
		public string Completed { get; set; }
		public bool IsCompleted {
			get => Get<bool>();
			set => Set(value);
		}
		public bool IsSaved { get; set; }
		public string Id { get; private set; } = Guid.NewGuid().ToString();
	}
}