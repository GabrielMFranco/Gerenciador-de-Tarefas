using GerenciadorDeTarefas.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace App_Gerenciador_de_Tarefas.Converters;

public class BoolToVisibilityConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
		=> value is bool val && val
			? Visibility.Visible 
			: Visibility.Collapsed;
	
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotImplementedException(); 
	}
}