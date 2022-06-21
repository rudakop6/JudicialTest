using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace JudicialTest
{
    public class PeriodMonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Dictionaries.Instance.GetMonth((MonthsOfYearEnum)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)Dictionaries.Instance.MonthsDictionary.FirstOrDefault(item => item.Value == value.ToString()).Key;
        }
    }
}
