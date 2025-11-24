using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DoImageViewer.App.Service;

/// <summary>
/// 缩略图服务，用于生成和缓存图片缩略图
/// </summary>
public class ThumbnailService
{
    private static readonly Dictionary<string, BitmapSource> _thumbnailCache = [];
    private const int MaxCacheSize = 50;
    private const int DefaultThumbnailSize = 200;

    /// <summary>
    /// 生成缩略图
    /// </summary>
    /// <param name="source">原始图片源</param>
    /// <param name="maxSize">缩略图最大尺寸</param>
    /// <returns>缩略图</returns>
    public static BitmapSource? CreateThumbnail(ImageSource? source, int maxSize = DefaultThumbnailSize)
    {
        if (source == null)
            return null;

        // 生成缓存键
        string cacheKey = $"{source.GetHashCode()}_{maxSize}";

        // 检查缓存
        if (_thumbnailCache.TryGetValue(cacheKey, out BitmapSource? cachedThumbnail)) {
            return cachedThumbnail;
        }

        // 生成缩略图
        BitmapSource? thumbnail = GenerateThumbnail(source, maxSize);

        // 添加到缓存
        if (thumbnail != null) {
            // 如果缓存已满，清理最旧的条目
            if (_thumbnailCache.Count >= MaxCacheSize) {
                var firstKey = _thumbnailCache.Keys.First();
                _thumbnailCache.Remove(firstKey);
            }

            _thumbnailCache[cacheKey] = thumbnail;
        }

        return thumbnail;
    }

    /// <summary>
    /// 实际生成缩略图的方法
    /// </summary>
    private static BitmapSource? GenerateThumbnail(ImageSource? source, int maxSize)
    {
        if (source is not BitmapSource bitmapSource)
            return null;

        try {
            // 计算缩放比例
            double scaleX = (double)maxSize / bitmapSource.PixelWidth;
            double scaleY = (double)maxSize / bitmapSource.PixelHeight;
            double scale = Math.Min(scaleX, scaleY);

            // 如果原图已经很小，直接返回
            if (scale >= 1.0)
                return bitmapSource;

            // 计算新的尺寸
            int newWidth = (int)(bitmapSource.PixelWidth * scale);
            int newHeight = (int)(bitmapSource.PixelHeight * scale);

            // 创建缩略图
            var thumbnail = new TransformedBitmap(bitmapSource, new ScaleTransform(scale, scale));

            // 冻结以提高性能
            thumbnail.Freeze();

            return thumbnail;
        }
        catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($"生成缩略图失败: {ex.Message}");
            return null;
        }
    }
}