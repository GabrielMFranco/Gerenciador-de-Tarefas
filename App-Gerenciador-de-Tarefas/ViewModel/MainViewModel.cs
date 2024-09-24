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
		public ObservableCollection<ToDoItem> Selector { get; set; } = new ObservableCollection<ToDoItem>();
		private List<string> idJson = new List<string>();
		public MainViewModel() {
			AddToDoItemCommand = new RelayCommand(AdicionarTarefa);
			RemoveToDoItemCommand = new RelayCommand<ToDoItem>(RemoveCommand);
			LoadDataCommand = new RelayCommand(LoadData);
			SaveDataCommand = new RelayCommand(SaveData);
			LoadData();

			UnsavedItems = false;
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
					NotifyPropertyChanged(nameof(CircleVisibility));
				}
			}
		}
		public bool UnsavedItems {	
			get => Get<bool>();
			private set {
				if (Set(value)) {
					NotifyPropertyChanged(nameof(CircleVisibility));
				}
			}
		}
		public Visibility CircleVisibility => UnsavedItems ? Visibility.Visible : Visibility.Collapsed;
		public IList<ToDoItem> Tasks => _allItems.AsEnumerable()
			.Where(item => string.IsNullOrWhiteSpace(OnlyCompleted) || item.Title.Contains(OnlyCompleted, StringComparison.OrdinalIgnoreCase))
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
		private void AdicionarTarefa() {
			if (!string.IsNullOrWhiteSpace(NewTitle) && !string.IsNullOrWhiteSpace(NewDescription) && !string.IsNullOrWhiteSpace(NewCompleted)) {
				ToDoItem tarefa = new();
				tarefa.Title = NewTitle;
				tarefa.Description = $"Descrição {NewDescription}";
				tarefa.Create = $"Data de criação: {DateTime.Now.ToString("dd/MM/yyyy")}";
				tarefa.Completed = $"Data de conclusão: {NewCompleted}";

				_allItems.Add(tarefa);
				Selector.Add(tarefa);
				NotifyPropertyChanged(nameof(Tasks));

				UnsavedItems = true;

				NewTitle = NewDescription = NewCompleted = string.Empty;
			}
			else {
				MessageBox.Show("Preencha todos os campos brow!");
			}
		}
		private void RemoveCommand(ToDoItem tarefa) {
			if (tarefa != null && _allItems.Remove(tarefa)) {
				Selector.Remove(tarefa);
				NotifyPropertyChanged(nameof(Tasks));

				UpdateUnsavedStatus();
			} else{
				MessageBox.Show("Item não encontrado na lista.");
				}
			}
		private void UpdateUnsavedStatus() {
			bool hasSameCount = _allItems.Count == idJson.Count;
			bool hasSameIds = _allItems.Select(item => item.Id).OrderBy(id => id).SequenceEqual(idJson.OrderBy(id => id));

			UnsavedItems = !(hasSameCount && hasSameIds);
		}
		private void LoadData() {
			if (File.Exists("data.json")) {
				string conteudo = File.ReadAllText("data.json");
				var loadedItems = JsonSerializer.Deserialize<List<ToDoItem>>(conteudo) ?? new List<ToDoItem>();
				_allItems.Clear();
				_allItems.AddRange(loadedItems);
				Selector.Clear();
				foreach (var item in loadedItems) {
					Selector.Add(item);
				}
				idJson = loadedItems.Select(item => item.Id).ToList();
				UpdateUnsavedStatus();
				
			}
			else {
				MessageBox.Show("Arquivo de dados não encontrado.");
			}
		}
		
		private void SaveData() {
			File.WriteAllText("data.json", JsonSerializer.Serialize(_allItems));
			MessageBox.Show("Salvo com sucesso!", "Salvou");
			idJson = _allItems.Select(item => item.Id).ToList();
			UnsavedItems = false;
		}
	}
}