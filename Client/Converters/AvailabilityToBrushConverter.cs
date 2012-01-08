using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Client.Converters
{
    public class AvailabilityToBrushConverter : IValueConverter
    {
        private readonly Brush _availableBrush = Brushes.Green;
        private readonly Brush _notAvailableBrush = Brushes.Red;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isAvailable = (bool) value;

            if (isAvailable)
            {
                return _availableBrush;
            }
            else
            {
                return _notAvailableBrush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brush = (Brush) value;

            if (brush == _availableBrush)
            {
                return true;
            }

            if (brush == _notAvailableBrush)
            {
                return false;
            }

            throw new ArgumentException();
        }
    }
}
