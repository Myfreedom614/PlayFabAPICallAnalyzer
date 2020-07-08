using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PlayFabAPICallAnalyzer.Converter
{
	public class DGHeaderConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var isUTC = true;
			bool para;
			if (bool.TryParse(value.ToString(), out para))
			{
				isUTC = para;
				//Debug.WriteLine($"Converter isUTC: {isUTC}");
			}

			if (isUTC)
				return "TimeStamp (UTC)";
			return "TimeStamp (Local Time)";
		}

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
