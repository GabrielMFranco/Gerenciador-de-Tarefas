using GerenciadorDeTarefas.ViewModel;
using System.Windows;

namespace App_Gerenciador_de_Tarefas {
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
			DataContext = new MainViewModel();
		}

		private void ListBox_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e) {

		}
	}
}