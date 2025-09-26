using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DoImageViewer.App.Controls;

public partial class PathButton : Button
{
    #region 依赖属性

    /// <summary>
    /// Path几何数据
    /// </summary>
    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(
            nameof(Data),
            typeof(Geometry),
            typeof(PathButton),
            new PropertyMetadata(null));

    /// <summary>
    /// Path填充颜色
    /// </summary>
    public static readonly DependencyProperty FillProperty =
        DependencyProperty.Register(
            nameof(Fill),
            typeof(Brush),
            typeof(PathButton),
            new PropertyMetadata(Brushes.Black));

    /// <summary>
    /// Path描边颜色
    /// </summary>
    public static readonly DependencyProperty StrokeProperty =
        DependencyProperty.Register(
            nameof(Stroke),
            typeof(Brush),
            typeof(PathButton),
            new PropertyMetadata(null));

    /// <summary>
    /// Path描边粗细
    /// </summary>
    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(
            nameof(StrokeThickness),
            typeof(double),
            typeof(PathButton),
            new PropertyMetadata(0.1));

    /// <summary>
    /// Path拉伸方式
    /// </summary>
    public static readonly DependencyProperty StretchProperty =
        DependencyProperty.Register(
            nameof(Stretch),
            typeof(Stretch),
            typeof(PathButton),
            new PropertyMetadata(Stretch.Uniform));

    #endregion

    #region 属性

    public Geometry Data
    {
        get => (Geometry)GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public Brush Fill
    {
        get => (Brush)GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    public Brush Stroke
    {
        get => (Brush)GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public Stretch Stretch
    {
        get => (Stretch)GetValue(StretchProperty);
        set => SetValue(StretchProperty, value);
    }

    #endregion

    public PathButton()
    {
        InitializeComponent();
    }
}
