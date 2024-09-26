using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace App_Gerenciador_de_Tarefas.Converters;

public class BoolToYesOrNoStringConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
		=> (bool)value
			? "Sim" : "Não";

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) 
		=> ((string)value).Equals("sim", StringComparison.OrdinalIgnoreCase);
}