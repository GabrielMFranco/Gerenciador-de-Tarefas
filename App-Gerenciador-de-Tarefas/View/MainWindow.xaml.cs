using GerenciadorDeTarefas.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace App_Gerenciador_de_Tarefas {
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			DataContext = new MainViewModel();
		}
	}
}