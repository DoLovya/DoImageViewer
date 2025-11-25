using Microsoft.Xaml.Behaviors;
using OpenCvSharp;
using System.IO;
using System.Windows;

namespace DoImageViewer.Controls.Behaviors;

/// <summary>
/// 图片拖放行为，为控件添加图片拖放功能
/// </summary>
public class ImageDragDropBehavior : Behavior<FrameworkElement>
{
    #region 依赖属性

    /// <summary>
    /// 目标图片源属性
    /// </summary>
    public static readonly DependencyProperty MatSourceProperty =
        DependencyProperty.Register(nameof(MatSource), typeof(Mat), typeof(ImageDragDropBehavior));

    public Mat MatSource
    {
        get => (Mat)GetValue(MatSourceProperty);
        set => SetValue(MatSourceProperty, value);
    }

    /// <summary>
    /// 支持的图片扩展名
    /// </summary>
    public static readonly DependencyProperty SupportedExtensionsProperty =
        DependencyProperty.Register(nameof(SupportedExtensions), typeof(string), typeof(ImageDragDropBehavior),
            new PropertyMetadata(".jpg,.jpeg,.png,.bmp,.gif,.tiff,.webp"));

    public string SupportedExtensions
    {
        get => (string)GetValue(SupportedExtensionsProperty);
        set => SetValue(SupportedExtensionsProperty, value);
    }

    /// <summary>
    /// 拖放时的透明度
    /// </summary>
    public static readonly DependencyProperty DragOverOpacityProperty =
        DependencyProperty.Register(nameof(DragOverOpacity), typeof(double), typeof(ImageDragDropBehavior),
            new PropertyMetadata(0.8));

    public double DragOverOpacity
    {
        get => (double)GetValue(DragOverOpacityProperty);
        set => SetValue(DragOverOpacityProperty, value);
    }

    #endregion

    #region 私有字段

    private double _originalOpacity = 1.0;
    private string[] _supportedExtensionsArray;

    #endregion

    #region 重写方法

    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject != null) {
            AssociatedObject.AllowDrop = true;
            AssociatedObject.DragEnter += OnDragEnter;
            AssociatedObject.DragOver += OnDragOver;
            AssociatedObject.DragLeave += OnDragLeave;
            AssociatedObject.Drop += OnDrop;

            _originalOpacity = AssociatedObject.Opacity;
            UpdateSupportedExtensions();
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null) {
            AssociatedObject.AllowDrop = false;
            AssociatedObject.DragEnter -= OnDragEnter;
            AssociatedObject.DragOver -= OnDragOver;
            AssociatedObject.DragLeave -= OnDragLeave;
            AssociatedObject.Drop -= OnDrop;
        }

        base.OnDetaching();
    }

    #endregion

    #region 事件处理

    private void OnDragEnter(object sender, DragEventArgs e)
    {
        HandleDragEvent(e);

        if (e.Effects == DragDropEffects.Copy) {
            ApplyDragOverEffect();
        }
    }

    private void OnDragOver(object sender, DragEventArgs e)
    {
        HandleDragEvent(e);
    }

    private void OnDragLeave(object sender, DragEventArgs e)
    {
        RemoveDragOverEffect();
    }

    private void OnDrop(object sender, DragEventArgs e)
    {
        RemoveDragOverEffect();

        if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0) {
                var imageFile = files.FirstOrDefault(IsImageFile);
                if (!string.IsNullOrEmpty(imageFile)) {
                    LoadImage(imageFile);
                }
            }
        }
        e.Handled = true;
    }

    #endregion

    #region 辅助方法

    private void HandleDragEvent(DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length > 0 && files.Any(IsImageFile)) {
                e.Effects = DragDropEffects.Copy;
            }
            else {
                e.Effects = DragDropEffects.None;
            }
        }
        else {
            e.Effects = DragDropEffects.None;
        }
        e.Handled = true;
    }

    private bool IsImageFile(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
            return false;

        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return _supportedExtensionsArray?.Contains(extension) == true;
    }

    private void LoadImage(string filePath)
    {
        try {
            if (File.Exists(filePath)) {
                MatSource = Cv2.ImRead(filePath);
            }
        }
        catch (Exception ex) {
            // 可以在这里添加错误处理逻辑，比如显示错误消息
            System.Diagnostics.Debug.WriteLine($"加载图片失败: {ex.Message}");
        }
    }

    private void ApplyDragOverEffect()
    {
        if (AssociatedObject != null) {
            AssociatedObject.Opacity = DragOverOpacity;
        }
    }

    private void RemoveDragOverEffect()
    {
        if (AssociatedObject != null) {
            AssociatedObject.Opacity = _originalOpacity;
        }
    }

    private void UpdateSupportedExtensions()
    {
        if (!string.IsNullOrEmpty(SupportedExtensions)) {
            _supportedExtensionsArray = SupportedExtensions
                .Split(',')
                .Select(ext => ext.Trim().ToLowerInvariant())
                .Where(ext => !string.IsNullOrEmpty(ext))
                .ToArray();
        }
    }

    #endregion
}