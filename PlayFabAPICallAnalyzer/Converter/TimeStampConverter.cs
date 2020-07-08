using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace PlayFabAPICallAnalyzer.Converter
{
    public class TimeStampConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var isUTC = true;
            bool para;
            if (bool.TryParse(values[1].ToString(), out para))
            {
                isUTC = para;
                //Debug.WriteLine($"Converter isUTC: {isUTC}");
            }

            double tick;
            if (double.TryParse(values[0].ToString(), out tick))
            {
                return Helper.UnixTimeStampToDateTime(tick, isUTC).ToString();
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
