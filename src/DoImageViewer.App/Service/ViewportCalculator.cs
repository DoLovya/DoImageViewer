namespace DoImageViewer.App.Service;

/// <summary>
/// 视口计算器，用于计算主查看器和鸟瞰图之间的视口映射关系
/// </summary>
public static class ViewportCalculator
{
    /// <summary>
    /// 根据主查看器的变换状态计算鸟瞰图的视口参数
    /// </summary>
    /// <param name="imageWidth">原图宽度</param>
    /// <param name="imageHeight">原图高度</param>
    /// <param name="viewerWidth">查看器宽度</param>
    /// <param name="viewerHeight">查看器高度</param>
    /// <param name="scale">当前缩放比例</param>
    /// <param name="translateX">X轴偏移量</param>
    /// <param name="translateY">Y轴偏移量</param>
    /// <returns>视口参数</returns>
    public static ViewportInfo CalculateViewport(
        double imageWidth, double imageHeight,
        double viewerWidth, double viewerHeight,
        double scale, double translateX, double translateY)
    {
        if (imageWidth <= 0 || imageHeight <= 0 || scale <= 0)
            return new ViewportInfo(0, 0, 1, 1);

        // 计算缩放后的图片尺寸
        double scaledImageWidth = imageWidth * scale;
        double scaledImageHeight = imageHeight * scale;

        // 计算视口在原图中的位置和大小（比例）
        double viewportX = Math.Max(0, -translateX / scaledImageWidth);
        double viewportY = Math.Max(0, -translateY / scaledImageHeight);

        double viewportWidth = Math.Min(1, viewerWidth / scaledImageWidth);
        double viewportHeight = Math.Min(1, viewerHeight / scaledImageHeight);

        // 确保视口不超出边界
        viewportX = Math.Max(0, Math.Min(1 - viewportWidth, viewportX));
        viewportY = Math.Max(0, Math.Min(1 - viewportHeight, viewportY));

        return new ViewportInfo(viewportX, viewportY, viewportWidth, viewportHeight);
    }

    /// <summary>
    /// 根据鸟瞰图的视口参数计算主查看器应该的变换状态
    /// </summary>
    /// <param name="imageWidth">原图宽度</param>
    /// <param name="imageHeight">原图高度</param>
    /// <param name="viewerWidth">查看器宽度</param>
    /// <param name="viewerHeight">查看器高度</param>
    /// <param name="scale">当前缩放比例</param>
    /// <param name="viewportX">视口X位置（比例）</param>
    /// <param name="viewportY">视口Y位置（比例）</param>
    /// <returns>变换参数</returns>
    public static TransformInfo CalculateTransform(
        double imageWidth, double imageHeight,
        double viewerWidth, double viewerHeight,
        double scale, double viewportX, double viewportY)
    {
        if (imageWidth <= 0 || imageHeight <= 0 || scale <= 0)
            return new TransformInfo(0, 0);

        // 计算缩放后的图片尺寸
        double scaledImageWidth = imageWidth * scale;
        double scaledImageHeight = imageHeight * scale;

        // 计算变换偏移量
        double translateX = -viewportX * scaledImageWidth;
        double translateY = -viewportY * scaledImageHeight;

        return new TransformInfo(translateX, translateY);
    }

    /// <summary>
    /// 计算适合视图的缩放比例
    /// </summary>
    /// <param name="imageWidth">原图宽度</param>
    /// <param name="imageHeight">原图高度</param>
    /// <param name="viewerWidth">查看器宽度</param>
    /// <param name="viewerHeight">查看器高度</param>
    /// <returns>适合的缩放比例</returns>
    public static double CalculateFitToViewScale(
        double imageWidth, double imageHeight,
        double viewerWidth, double viewerHeight)
    {
        if (imageWidth <= 0 || imageHeight <= 0 || viewerWidth <= 0 || viewerHeight <= 0)
            return 1.0;

        double scaleX = viewerWidth / imageWidth;
        double scaleY = viewerHeight / imageHeight;
        return Math.Min(scaleX, scaleY);
    }
}

/// <summary>
/// 视口信息
/// </summary>
public record ViewportInfo(double X, double Y, double Width, double Height);

/// <summary>
/// 变换信息
/// </summary>
public record TransformInfo(double TranslateX, double TranslateY);