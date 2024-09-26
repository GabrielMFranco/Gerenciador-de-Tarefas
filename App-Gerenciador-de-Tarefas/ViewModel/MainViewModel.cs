#nullable disable

using App_Gerenciador_de_Tarefas.Foundation;
using GerenciadorDeTarefas.Model;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp1.Foundation;

namespace GerenciadorDeTarefas.ViewModel {
	public class MainViewModel : ObservableObject {
		public MainViewModel() {
			AddToDoItemCommand = new RelayCommand(AdicionarTarefa);
			RemoveToDoItemCommand = new RelayCommand<ToDoItem>(RemoveCommand);
			LoadDataCommand = new RelayCommand(LoadData);
			SaveDataCommand = new RelayCommand(SaveData);
			NewCompleted = DateTime.Today;
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
		public DateTime NewCompleted {
			get => Get<DateTime>();
			set => Set(value);
		}
		public string TitleFilter {
			get => Get<string>();
			set {
				if (Set(value)) {
					NotifyPropertyChanged(nameof(Tasks));
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
			if (!string.IsNullOrWhiteSpace(NewTitle) && !string.IsNullOrWhiteSpace(NewDescription) && NewCompleted != default) {
				ToDoItem item = new();
				item.Title = NewTitle;
				item.Description = NewDescription;
				item.Create = DateTime.Today;
				item.Completed = NewCompleted.Date;
				item.PropertyChanged += TodoItemPropertyChanged;

				_allItems.Add(item);
				NotifyPropertyChanged(nameof(Tasks));

				NewTitle = NewDescription = string.Empty;
				NewCompleted = DateTime.Today;

				NotifyPropertyChanged(nameof(UnsavedItems));

				return;
			}
			MessageBox.Show("Preencha todos os campos brow!");
		}
		private void TodoItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
			NotifyPropertyChanged(nameof(UnsavedItems));
		}
		private void RemoveCommand(ToDoItem item) {
			if (item != null && _allItems.Remove(item)) {
				NotifyPropertyChanged(nameof(Tasks));
				NotifyPropertyChanged(nameof(UnsavedItems));
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
				return;
			}
			MessageBox.Show("Arquivo de dados não encontrado.");
		}
		private void SaveData() {
			string content = JsonSerializer.Serialize(_allItems);
			File.WriteAllText("data.json", content);
			_lastSavedContent = content;
			NotifyPropertyChanged(nameof(UnsavedItems));
			MessageBox.Show("Salvo com sucesso!", "Salvou");
		}

		private int _redToRGB;
		private int _greenToRGB;
		private int _blueToRGB;
		public int RedToRGB {
			get => _redToRGB;
			set {
				if (_redToRGB != value) {
					_redToRGB = value;
					NotifyPropertyChanged(nameof(RedToRGB));
					UpdateColorResource();
				}
			}
		}
		public int GreenToRGB {
			get => _greenToRGB;
			set {
				if (_greenToRGB != value) {
					_greenToRGB = value;
					NotifyPropertyChanged(nameof(GreenToRGB));
					UpdateColorResource();
				}
			}
		}
		public int BlueToRGB {
			get => _blueToRGB;
			set {
				if (_blueToRGB != value) {
					_blueToRGB = value;
					NotifyPropertyChanged(nameof(BlueToRGB));
					UpdateColorResource();
				}
			}
		}
		private void UpdateColorResource() {
			var red = (byte)RedToRGB;
			var green = (byte)GreenToRGB;
			var blue = (byte)BlueToRGB;
			var newColor = new SolidColorBrush(Color.FromRgb(red, green, blue));

			const string colorResourceKey = "CompletionColor";
			Application.Current.Resources.Remove(colorResourceKey);
			Application.Current.Resources.Add(colorResourceKey, newColor);
		}
	}
}
