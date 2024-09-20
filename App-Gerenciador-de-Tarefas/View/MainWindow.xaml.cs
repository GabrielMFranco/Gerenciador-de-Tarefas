using GerenciadorDeTarefas.ViewModel;
using System.Windows;

namespace App_Gerenciador_de_Tarefas {
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			DataContext = new MainViewModel();
		}
	}
}