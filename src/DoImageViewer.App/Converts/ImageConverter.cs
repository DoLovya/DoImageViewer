using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace DoImageViewer.App.Converts;

public class ImageConverter : IValueConverter
{
    public required string BaseUri { get; set; }


    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is null) return null;

        if (value is string str) {
            BitmapImage? bitmapImage;
            try {
                bitmapImage = new BitmapImage(new Uri(@$"{BaseUri}{str}", UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.ToString());
                throw;
            }
            return bitmapImage;
        }
        return null;
    }


    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // 不支持反向转换
        throw new NotImplementedException("StringToImageConverter不支持反向转换");
    }
}