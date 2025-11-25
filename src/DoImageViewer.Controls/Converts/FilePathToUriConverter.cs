using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace DoImageViewer.Controls.Converts;

public class FilePathToUriConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null) return null;
        if (value is string str) {
            string baseDir = AppContext.BaseDirectory;
            string path = Path.IsPathRooted(str) ? str : Path.Combine(baseDir, str);
            return new Uri(path, UriKind.Absolute);
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
