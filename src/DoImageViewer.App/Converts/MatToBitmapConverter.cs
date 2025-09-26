using System.Globalization;
using System.Windows.Data;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace DoImageViewer.App.Converts;

public class MatToBitmapConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Mat mat || mat.Empty())
            return null;

        try {
            return mat.ToBitmapSource();
        }
        catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($"Mat转换为BitmapImage失败: {ex.Message}");
            return null;
        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException("MatToBitmapConverter 不支持反向转换");
    }
}
