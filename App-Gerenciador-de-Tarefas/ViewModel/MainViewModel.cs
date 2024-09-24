#nullable disable

using App_Gerenciador_de_Tarefas.Foundation;
using GerenciadorDeTarefas.Model;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Foundation;

namespace GerenciadorDeTarefas.ViewModel {
	public class MainViewModel : ObservableObject {
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
		public string TitleFilter {
			get => Get<string>();
			set {
				if (Set(value)) {
					NotifyPropertyChanged(nameof(Tasks));
					NotifyPropertyChanged(nameof(CircleVisibility));
				}
			}
		}
		public bool OnlyCompleted {
			get => Get<bool>();
			set {
				if (Set(value)) {
					NotifyPropertyChanged(nameof(Tasks));
				}
			}
		}
		public bool UnsavedItems
			=> JsonSerializer.Serialize(_allItems) != _lastSavedContent;
		public Visibility CircleVisibility => UnsavedItems ? Visibility.Visible : Visibility.Collapsed;
		public IList<ToDoItem> Tasks 
			=> _allItems
				.Where(item => 
					(string.IsNullOrWhiteSpace(TitleFilter) || item.Title.Contains(TitleFilter, StringComparison.OrdinalIgnoreCase))
					&& (!OnlyCompleted || item.IsCompleted)
				)
				.ToList();
		//public List<ToDoItem> Tasks
		//	=> OnlyCompleted
		//		? _allItems.Where(item => item.IsCompleted).ToList()
		//		: _allItems;
		public ICommand AddToDoItemCommand { get; }
		public ICommand RemoveToDoItemCommand { get; }
		public ICommand LoadDataCommand { get; }
		public ICommand SaveDataCommand { get; }
		 
		private readonly List<ToDoItem> _allItems = [];
		private string _lastSavedContent = string.Empty;

		private void AdicionarTarefa() {
			if (!string.IsNullOrWhiteSpace(NewTitle) && !string.IsNullOrWhiteSpace(NewDescription) && !string.IsNullOrWhiteSpace(NewCompleted)) {
				ToDoItem item = new();
				item.Title = NewTitle;
				item.Description = $"Descrição {NewDescription}";
				item.Create = $"Data de criação: {DateTime.Now.ToString("dd/MM/yyyy")}";
				item.Completed = $"Data de conclusão: {NewCompleted}";
				item.PropertyChanged += TodoItemPropertyChanged;

				_allItems.Add(item);
				NotifyPropertyChanged(nameof(Tasks));

				NewTitle = NewDescription = NewCompleted = string.Empty;
				NotifyPropertyChanged(nameof(UnsavedItems));
				NotifyPropertyChanged(nameof(CircleVisibility));
				NotifyPropertyChanged(nameof(CheckOnlyCompleted));
				
				return;
			}
			MessageBox.Show("Preencha todos os campos brow!");
		}
		private void TodoItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			NotifyPropertyChanged(nameof(UnsavedItems));
			NotifyPropertyChanged(nameof(CircleVisibility));
		}
		private void RemoveCommand(ToDoItem item) {
			if (item != null && _allItems.Remove(item)) {
				NotifyPropertyChanged(nameof(Tasks));
				NotifyPropertyChanged(nameof(UnsavedItems));
				NotifyPropertyChanged(nameof(CircleVisibility));
				item.PropertyChanged -= TodoItemPropertyChanged;
				return;
			}
			MessageBox.Show("Item não encontrado na lista.");
		}
		private void LoadData() {
			if (File.Exists("data.json")) {
				string conteudo = File.ReadAllText("data.json");
				_lastSavedContent = conteudo;
				List<ToDoItem> loadedItems = JsonSerializer.Deserialize<List<ToDoItem>>(conteudo) ?? new List<ToDoItem>();

				foreach (ToDoItem item in _allItems) {
					item.PropertyChanged -= TodoItemPropertyChanged;
				}
				_allItems.Clear();
				
				foreach (var item in loadedItems) {
					_allItems.Add(item);
					item.PropertyChanged += TodoItemPropertyChanged;
				}
				NotifyPropertyChanged(nameof(Tasks));
				NotifyPropertyChanged(nameof(UnsavedItems));
				NotifyPropertyChanged(nameof(CircleVisibility));
				return;
			}
			MessageBox.Show("Arquivo de dados não encontrado.");
		}		
		private void SaveData() {
			string content = JsonSerializer.Serialize(_allItems);
			File.WriteAllText("data.json", content);
			_lastSavedContent = content;
			NotifyPropertyChanged(nameof(UnsavedItems));
			NotifyPropertyChanged(nameof(CircleVisibility));
			MessageBox.Show("Salvo com sucesso!", "Salvou");
		}
	}
}