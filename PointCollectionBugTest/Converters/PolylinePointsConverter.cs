using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Point = System.Windows.Point;

namespace PointCollectionBugTest.Converters
{
    public class PolylinePointsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PointCollection points = new PointCollection();
            foreach (var item in (value as ObservableCollection<Point>))
                points.Add(item);
            return points;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
