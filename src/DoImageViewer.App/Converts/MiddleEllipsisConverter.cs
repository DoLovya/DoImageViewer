using System.Globalization;
using System.Windows.Data;

namespace DoImageViewer.App.Converts;

public class MiddleEllipsisConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string s || string.IsNullOrEmpty(s)) return value;

        int maxLength = 20;
        if (parameter is string p && int.TryParse(p, out int parsed)) {
            maxLength = parsed;
        }

        if (s.Length <= maxLength) return s;

        int ellipsisLen = 3;
        int keep = maxLength - ellipsisLen;
        int head = keep / 2;
        int tail = keep - head;

        if (head < 1) head = 1;
        if (tail < 1) tail = 1;

        string prefix = s.Substring(0, head);
        string suffix = s.Substring(s.Length - tail, tail);
        return prefix + new string('.', ellipsisLen) + suffix;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
