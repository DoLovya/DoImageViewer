using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DoImageViewer.Controls;

public partial class ImageButton : Button
{
    #region 依赖属性

    public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register(
            nameof(Source),
            typeof(string),
            typeof(ImageButton),
            new PropertyMetadata(null));

    /// <summary>
    /// Path填充颜色
    /// </summary>
    public static readonly DependencyProperty FillProperty =
        DependencyProperty.Register(
            nameof(Fill),
            typeof(Brush),
            typeof(ImageButton),
            new PropertyMetadata(Brushes.Black));

    /// <summary>
    /// Path描边颜色
    /// </summary>
    public static readonly DependencyProperty StrokeProperty =
        DependencyProperty.Register(
            nameof(Stroke),
            typeof(Brush),
            typeof(ImageButton),
            new PropertyMetadata(null));

    /// <summary>
    /// Path描边粗细
    /// </summary>
    public static readonly DependencyProperty StrokeThicknessProperty =
        DependencyProperty.Register(
            nameof(StrokeThickness),
            typeof(double),
            typeof(ImageButton),
            new PropertyMetadata(0.1));

    /// <summary>
    /// Path拉伸方式
    /// </summary>
    public static readonly DependencyProperty StretchProperty =
        DependencyProperty.Register(
            nameof(Stretch),
            typeof(Stretch),
            typeof(ImageButton),
            new PropertyMetadata(Stretch.Uniform));

    #endregion

    #region 属性

    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
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

    public ImageButton()
    {
        InitializeComponent();
    }
}
