using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace App_Gerenciador_de_Tarefas.Converters;

public class DueDateToBrushConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value is DateTime completedDate && completedDate < DateTime.Today 
			? Brushes.Firebrick 
			: Brushes.Black;
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		throw new NotImplementedException();
	}
}
