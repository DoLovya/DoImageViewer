
using System.Windows;

namespace DoImageViewer.App.Service;

public interface ITransform
{
    double TranslateX { get; }
    double TranslateY { get; }
    void SetScale(double scaleX, double scaleY);
    void AddTranslate(Vector delta);
    void AddTranslate(double translateX, double translateY);
}
