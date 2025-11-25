using System.Windows;
using System.Windows.Media;
using DoImageViewer.App.Helpers;
using DoImageViewer.App.Service;
using DoImageViewer.Controls;

namespace DoImageViewer.App.Views;

public partial class MainView : Window
{
    private bool _isUpdatingViewport = false;

    public MainView()
    {
        InitializeComponent();

        SourceInitialized += (s, e) =>
        {
            TitleBarHelper.SetTitleBarBackgroundColor(this,
                Color.FromRgb(0x20, 0x20, 0x20));
        };

        this.SizeChanged += (s, e) =>
        {
            // 强制立即更新布局
            this.UpdateLayout();
            UpdateBirdsEyeViewport();
        };

        // 监听主图片控件的变换变化
        MainImageControl.Loaded += (s, e) => UpdateBirdsEyeViewport();
        MainImageControl.TransformChanged += (s, e) => UpdateBirdsEyeViewport();
    }

    /// <summary>
    /// 鸟瞰图视口改变事件处理
    /// </summary>
    private void OnBirdsEyeViewportChanged(object sender, ViewportChangedEventArgs e)
    {
        if (_isUpdatingViewport || MainImageControl.ImageSource == null)
            return;

        _isUpdatingViewport = true;

        try {
            // 计算新的变换参数
            var transform = ViewportCalculator.CalculateTransform(
                MainImageControl.ImageSource.Width,
                MainImageControl.ImageSource.Height,
                MainImageControl.ActualWidth,
                MainImageControl.ActualHeight,
                MainImageControl.ScaleTransform.ScaleX,
                e.X, e.Y);

            // 应用变换
            MainImageControl.TranslateTransform.X = transform.TranslateX;
            MainImageControl.TranslateTransform.Y = transform.TranslateY;
        }
        finally {
            _isUpdatingViewport = false;
        }
    }

    /// <summary>
    /// 更新鸟瞰图的视口显示
    /// </summary>
    private void UpdateBirdsEyeViewport()
    {
        if (_isUpdatingViewport || MainImageControl.ImageSource == null)
            return;

        _isUpdatingViewport = true;

        try {
            // 计算当前视口参数
            var viewport = ViewportCalculator.CalculateViewport(
                MainImageControl.ImageSource.Width,
                MainImageControl.ImageSource.Height,
                MainImageControl.ActualWidth,
                MainImageControl.ActualHeight,
                MainImageControl.ScaleTransform.ScaleX,
                MainImageControl.TranslateTransform.X,
                MainImageControl.TranslateTransform.Y);

            // 更新鸟瞰图视口
            BirdsEyeControl.ViewportX = viewport.X;
            BirdsEyeControl.ViewportY = viewport.Y;
            BirdsEyeControl.ViewportWidth = viewport.Width;
            BirdsEyeControl.ViewportHeight = viewport.Height;
        }
        finally {
            _isUpdatingViewport = false;
        }
    }
}