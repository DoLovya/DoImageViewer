using System.Windows;

namespace DoImageViewer.App.Service;

public class TransformService
{
    private const double _minScale = 0.01;
    private const double _maxScale = 32.0;
    private readonly ITransform _transform;

    public TransformService(ITransform transform)
    {
        _transform = transform;
    }

    public void Translate(Point point, Point targetPoint)
    {
        if (_transform is null) return;

        Vector delta = targetPoint - point;
        _transform?.AddTranslate(delta);
    }
    public double ZoomAtPoint(double currentScale, Point point, double scaleFactor)
    {
        if (_transform is null) return currentScale;

        // 计算新的缩放值
        double newScale = currentScale * scaleFactor;

        // 限制缩放范围
        if (newScale < _minScale || newScale > _maxScale)
            return currentScale;

        // 计算缩放前的点相对于变换原点的位置
        Point pointRelativeToOrigin = new Point(
            point.X - _transform.TranslateX,
            point.Y - _transform.TranslateY);

        // 计算缩放后的偏移量
        double deltaX = pointRelativeToOrigin.X * (1 - scaleFactor);
        double deltaY = pointRelativeToOrigin.Y * (1 - scaleFactor);

        // 直接应用变换，无动画
        _transform.SetScale(newScale, newScale);
        _transform?.AddTranslate(deltaX, deltaY);
        return newScale;
    }

}
