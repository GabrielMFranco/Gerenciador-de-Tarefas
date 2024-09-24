#nullable disable

using App_Gerenciador_de_Tarefas.Foundation;
using GerenciadorDeTarefas.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WpfApp1.Foundation;

namespace GerenciadorDeTarefas.ViewModel {
	
	public class MainViewModel : ObservableObject {
		private int unsavedCount = 0;
		public ObservableCollection<string> Selector { get; set; }
		public MainViewModel() {
			AddToDoItemCommand = new RelayCommand(AdicionarTarefa);
			RemoveToDoItemCommand = new RelayCommand<ToDoItem>(RemoveCommand);
			LoadDataCommand = new RelayCommand(LoadData);
			SaveDataCommand = new RelayCommand(SaveData);
			LoadData();
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
		public string OnlyCompleted {
			get => Get<string>();
			set {
				if (Set(value)) {
					NotifyPropertyChanged(nameof(Tasks));
				}
			}
		}
		public bool UnsavedItems {
			get => Get<bool>();
			set {
				if (Set(value)) {
					NotifyPropertyChanged(nameof(UnsavedItems));
					NotifyPropertyChanged(nameof(CircleVisibility));
				}
			}
		}

		public Visibility CircleVisibility => UnsavedItems ? Visibility.Visible : Visibility.Collapsed;
		public IList<ToDoItem> Tasks {
			get {
				var filterTitle = _allItems.AsEnumerable();
				if (!string.IsNullOrWhiteSpace(OnlyCompleted)) {
					filterTitle = filterTitle.Where(item => item.Title.Contains(OnlyCompleted, StringComparison.OrdinalIgnoreCase));
				}
				return filterTitle.ToList();
			}
		}
		//public List<ToDoItem> Tasks
		//	=> OnlyCompleted
		//		? _allItems.Where(item => item.IsCompleted).ToList()
		//		: _allItems;
		public ICommand AddToDoItemCommand { get; }
		public ICommand RemoveToDoItemCommand { get; }
		public ICommand LoadDataCommand { get; }
		public ICommand SaveDataCommand { get; }
 
		private readonly List<ToDoItem> _allItems = [];

		private void AdicionarTarefa() {
			if (!string.IsNullOrWhiteSpace(NewTitle) && !string.IsNullOrWhiteSpace(NewDescription) && !string.IsNullOrWhiteSpace(NewCompleted)) {
				ToDoItem tarefa = new();
				tarefa.Title = NewTitle;
				tarefa.Description = $"Descrição {NewDescription}";
				tarefa.Create = $"Data de criação: {DateTime.Now.ToString("dd/MM/yyyy")}";
				tarefa.Completed = $"Data de conclusão: {NewCompleted}";

				_allItems.Add(tarefa);
				NotifyPropertyChanged(nameof(Tasks));

				unsavedCount++;
				UpdateUnsavedStatus();
				

				NewTitle = NewDescription = NewCompleted = string.Empty;
			}
			else {
				MessageBox.Show("Preencha todos os campos brow!");
			}
		}
		private void RemoveCommand(ToDoItem tarefa) {
			if (tarefa != null) {
				if (_allItems.Contains(tarefa)) {
					_allItems.Remove(tarefa);
					NotifyPropertyChanged(nameof(Tasks));

					if (unsavedCount > 0)
						unsavedCount--;
					UpdateUnsavedStatus();
				}
				else {
					MessageBox.Show("Item não encontrado na lista.");
				}
			}
		}
		private void LoadData() {
			string conteudo = File.ReadAllText("data.json");
			ToDoItem[] items = JsonSerializer.Deserialize<ToDoItem[]>(conteudo);
			_allItems.Clear();
			foreach (ToDoItem item in items) {
				_allItems.Add(item);
			}
		}
		private void SaveData() {
			File.WriteAllText("data.json", JsonSerializer.Serialize(Tasks));
			MessageBox.Show("Salvo com sucesso!", "Salvou");
			unsavedCount = 0;
			UpdateUnsavedStatus();
		}

		private void UpdateUnsavedStatus() {
			UnsavedItems = unsavedCount > 0;
		}
	}
}