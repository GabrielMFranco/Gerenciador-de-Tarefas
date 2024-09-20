using App_Gerenciador_de_Tarefas.Foundation;
using GerenciadorDeTarefas.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Foundation;

namespace GerenciadorDeTarefas.ViewModel {
	public class MainViewModel : ObservableObject {
		public MainViewModel() {
			Tasks = new ObservableCollection<ToDoItem>();
			AddToDoItemCommand = new RelayCommand(AdicionarTarefa);
			RemoveToDoItemCommand = new RelayCommand<ToDoItem>(RemoveCommand);
		}

		public string NewTitle {
			get => Get<string>();
			set => Set(value);
		}
		public string NewDescription {
			get => Get<string>();
			set => Set(value);
		}
		public string NewCompleted {
			get => Get<string>();
			set => Set(value);
		}
		public ObservableCollection<ToDoItem> Tasks {
			get;
			set;
		}
		public ICommand AddToDoItemCommand { get; }
        public ICommand RemoveToDoItemCommand { get; }
        private void AdicionarTarefa() {
			if (!string.IsNullOrWhiteSpace(NewTitle) && !string.IsNullOrWhiteSpace(NewDescription) && !string.IsNullOrWhiteSpace(NewCompleted)) {
				ToDoItem tarefa = new();
				tarefa.Title = NewTitle;
				tarefa.Description = $"Descrição {NewDescription}";
				tarefa.Create = $"Data de criação: {DateTime.Now.ToString("dd/MM/yyyy")}";
				tarefa.Completed = $"Data de conclusão: {NewCompleted}";

				Tasks.Add(tarefa);

				NewTitle = NewDescription = NewCompleted = string.Empty;
			}
			else {
				MessageBox.Show("Preencha todos os campos brow!");
			}
		}
		private void RemoveCommand(ToDoItem tarefa) {
			if (tarefa != null && Tasks.Contains(tarefa)) {
				Tasks.Remove(tarefa);
			}
		}

		
	}
}