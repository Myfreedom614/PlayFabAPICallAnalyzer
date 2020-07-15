
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PlayFabAPICallAnalyzer.Converter
{
    public class DateTimeAxisDTConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double days;
            if (double.TryParse(value.ToString(), out days))
            {
                return DateTimeAxis.ToDateTime(days);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
