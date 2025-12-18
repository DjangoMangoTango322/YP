using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Desktop.Converters
{
    public class IdToNameConverter : IValueConverter
    {
        public Dictionary<int, string> NameDictionary { get; set; } = new Dictionary<int, string>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int id && NameDictionary.TryGetValue(id, out string name))
            {
                return name;
            }
            return "Неизвестно";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}