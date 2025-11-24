using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DoImageViewer.App.Service;

namespace DoImageViewer.App.Controls
{
    /// <summary>
    /// 鸟瞰图控件，提供图片的缩略图视图和导航功能
    /// </summary>
    public partial class BirdsEyeView : UserControl
    {
        #region 依赖属性

        /// <summary>
        /// 原始图片源
        /// </summary>
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(BirdsEyeView),
                new PropertyMetadata(null, OnImageSourceChanged));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        /// <summary>
        /// 当前视口的X偏移量（相对于原图的比例，0-1）
        /// </summary>
        public static readonly DependencyProperty ViewportXProperty =
            DependencyProperty.Register("ViewportX", typeof(double), typeof(BirdsEyeView),
                new PropertyMetadata(0.0, OnViewportChanged));

        public double ViewportX
        {
            get { return (double)GetValue(ViewportXProperty); }
            set { SetValue(ViewportXProperty, value); }
        }

        /// <summary>
        /// 当前视口的Y偏移量（相对于原图的比例，0-1）
        /// </summary>
        public static readonly DependencyProperty ViewportYProperty =
            DependencyProperty.Register("ViewportY", typeof(double), typeof(BirdsEyeView),
                new PropertyMetadata(0.0, OnViewportChanged));

        public double ViewportY
        {
            get { return (double)GetValue(ViewportYProperty); }
            set { SetValue(ViewportYProperty, value); }
        }

        /// <summary>
        /// 当前视口的宽度（相对于原图的比例，0-1）
        /// </summary>
        public static readonly DependencyProperty ViewportWidthProperty =
            DependencyProperty.Register("ViewportWidth", typeof(double), typeof(BirdsEyeView),
                new PropertyMetadata(1.0, OnViewportChanged));

        public double ViewportWidth
        {
            get { return (double)GetValue(ViewportWidthProperty); }
            set { SetValue(ViewportWidthProperty, value); }
        }

        /// <summary>
        /// 当前视口的高度（相对于原图的比例，0-1）
        /// </summary>
        public static readonly DependencyProperty ViewportHeightProperty =
            DependencyProperty.Register("ViewportHeight", typeof(double), typeof(BirdsEyeView),
                new PropertyMetadata(1.0, OnViewportChanged));

        public double ViewportHeight
        {
            get { return (double)GetValue(ViewportHeightProperty); }
            set { SetValue(ViewportHeightProperty, value); }
        }

        #endregion

        #region 事件

        /// <summary>
        /// 视口位置改变事件
        /// </summary>
        public event EventHandler<ViewportChangedEventArgs> ViewportChanged;

        #endregion

        #region 私有字段

        private bool _isDragging = false;
        private Point _lastMousePosition;
        private double _thumbnailScale = 1.0;

        #endregion

        public BirdsEyeView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
        }

        #region 事件处理

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateThumbnail();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateThumbnail();
        }

        private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BirdsEyeView control)
            {
                control.UpdateThumbnail();
            }
        }

        private static void OnViewportChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BirdsEyeView control)
            {
                control.UpdateViewportIndicator();
            }
        }

        private void OnCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            _lastMousePosition = e.GetPosition(ThumbnailCanvas);
            ThumbnailCanvas.CaptureMouse();
            
            // 立即更新视口位置
            UpdateViewportFromMousePosition(_lastMousePosition);
            e.Handled = true;
        }

        private void OnCanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            ThumbnailCanvas.ReleaseMouseCapture();
            e.Handled = true;
        }

        private void OnCanvasMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Point currentPosition = e.GetPosition(ThumbnailCanvas);
                UpdateViewportFromMousePosition(currentPosition);
                _lastMousePosition = currentPosition;
                e.Handled = true;
            }
        }

        private void OnCanvasMouseEnter(object sender, MouseEventArgs e)
        {
            // 鼠标进入时，增强视口指示器的显示
            if (ViewportIndicator.Visibility == Visibility.Visible)
            {
                ViewportIndicator.StrokeThickness = 3;
            }
        }

        private void OnCanvasMouseLeave(object sender, MouseEventArgs e)
        {
            // 鼠标离开时，恢复视口指示器的正常显示
            if (ViewportIndicator.Visibility == Visibility.Visible)
            {
                ViewportIndicator.StrokeThickness = 2;
            }
            
            // 如果正在拖拽，停止拖拽
            if (_isDragging)
            {
                _isDragging = false;
                ThumbnailCanvas.ReleaseMouseCapture();
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 更新缩略图显示
        /// </summary>
        private void UpdateThumbnail()
        {
            if (ImageSource == null || ActualWidth == 0 || ActualHeight == 0)
            {
                ThumbnailImage.Source = null;
                ViewportIndicator.Visibility = Visibility.Collapsed;
                return;
            }

            // 使用缩略图服务生成高质量缩略图
            var thumbnail = ThumbnailService.CreateThumbnail(ImageSource, (int)Math.Max(ActualWidth, ActualHeight));
            ThumbnailImage.Source = thumbnail ?? ImageSource;
            
            // 计算缩放比例以适应控件大小
            double scaleX = ActualWidth / ImageSource.Width;
            double scaleY = ActualHeight / ImageSource.Height;
            _thumbnailScale = Math.Min(scaleX, scaleY);

            // 设置缩略图大小和位置
            double thumbnailWidth = ImageSource.Width * _thumbnailScale;
            double thumbnailHeight = ImageSource.Height * _thumbnailScale;
            
            ThumbnailImage.Width = thumbnailWidth;
            ThumbnailImage.Height = thumbnailHeight;
            
            Canvas.SetLeft(ThumbnailImage, (ActualWidth - thumbnailWidth) / 2);
            Canvas.SetTop(ThumbnailImage, (ActualHeight - thumbnailHeight) / 2);

            UpdateViewportIndicator();
        }

        /// <summary>
        /// 更新视口指示器
        /// </summary>
        private void UpdateViewportIndicator()
        {
            if (ImageSource == null || _thumbnailScale == 0)
            {
                ViewportIndicator.Visibility = Visibility.Collapsed;
                DarkMask.Visibility = Visibility.Collapsed;
                return;
            }

            // 计算缩略图的实际位置和大小
            double thumbnailWidth = ImageSource.Width * _thumbnailScale;
            double thumbnailHeight = ImageSource.Height * _thumbnailScale;
            double thumbnailLeft = (ActualWidth - thumbnailWidth) / 2;
            double thumbnailTop = (ActualHeight - thumbnailHeight) / 2;

            // 计算视口指示器的位置和大小
            double indicatorLeft = thumbnailLeft + ViewportX * thumbnailWidth;
            double indicatorTop = thumbnailTop + ViewportY * thumbnailHeight;
            double indicatorWidth = ViewportWidth * thumbnailWidth;
            double indicatorHeight = ViewportHeight * thumbnailHeight;

            // 确保指示器不超出缩略图边界
            indicatorLeft = Math.Max(thumbnailLeft, Math.Min(thumbnailLeft + thumbnailWidth - indicatorWidth, indicatorLeft));
            indicatorTop = Math.Max(thumbnailTop, Math.Min(thumbnailTop + thumbnailHeight - indicatorHeight, indicatorTop));
            indicatorWidth = Math.Min(indicatorWidth, thumbnailWidth);
            indicatorHeight = Math.Min(indicatorHeight, thumbnailHeight);

            // 设置视口指示器
            Canvas.SetLeft(ViewportIndicator, indicatorLeft);
            Canvas.SetTop(ViewportIndicator, indicatorTop);
            ViewportIndicator.Width = Math.Max(0, indicatorWidth);
            ViewportIndicator.Height = Math.Max(0, indicatorHeight);
            
            // 更新遮罩层几何形状
            UpdateDarkMask(thumbnailLeft, thumbnailTop, thumbnailWidth, thumbnailHeight, 
                          indicatorLeft, indicatorTop, indicatorWidth, indicatorHeight);
            
            // 只有当视口小于整个图片时才显示指示器和遮罩
            bool shouldShow = (ViewportWidth < 1.0 || ViewportHeight < 1.0);
            ViewportIndicator.Visibility = shouldShow ? Visibility.Visible : Visibility.Collapsed;
            DarkMask.Visibility = shouldShow ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// 更新遮罩层几何形状
        /// </summary>
        private void UpdateDarkMask(double thumbnailLeft, double thumbnailTop, double thumbnailWidth, double thumbnailHeight,
                                   double viewportLeft, double viewportTop, double viewportWidth, double viewportHeight)
        {
            // 设置完整区域几何形状（缩略图区域）
            FullAreaGeometry.Rect = new Rect(thumbnailLeft, thumbnailTop, thumbnailWidth, thumbnailHeight);
            
            // 设置视口区域几何形状（需要高亮的区域）
            ViewportGeometry.Rect = new Rect(viewportLeft, viewportTop, viewportWidth, viewportHeight);
        }

        /// <summary>
        /// 根据鼠标位置更新视口
        /// </summary>
        private void UpdateViewportFromMousePosition(Point mousePosition)
        {
            if (ImageSource == null || _thumbnailScale == 0)
                return;

            // 计算缩略图的实际位置和大小
            double thumbnailWidth = ImageSource.Width * _thumbnailScale;
            double thumbnailHeight = ImageSource.Height * _thumbnailScale;
            double thumbnailLeft = (ActualWidth - thumbnailWidth) / 2;
            double thumbnailTop = (ActualHeight - thumbnailHeight) / 2;

            // 将鼠标位置转换为相对于缩略图的比例
            double relativeX = (mousePosition.X - thumbnailLeft) / thumbnailWidth;
            double relativeY = (mousePosition.Y - thumbnailTop) / thumbnailHeight;

            // 限制在有效范围内
            relativeX = Math.Max(0, Math.Min(1, relativeX));
            relativeY = Math.Max(0, Math.Min(1, relativeY));

            // 计算新的视口位置（以视口中心为基准）
            double newViewportX = relativeX - ViewportWidth / 2;
            double newViewportY = relativeY - ViewportHeight / 2;

            // 确保视口不超出边界
            newViewportX = Math.Max(0, Math.Min(1 - ViewportWidth, newViewportX));
            newViewportY = Math.Max(0, Math.Min(1 - ViewportHeight, newViewportY));

            // 更新视口位置
            ViewportX = newViewportX;
            ViewportY = newViewportY;

            // 触发事件
            ViewportChanged?.Invoke(this, new ViewportChangedEventArgs(ViewportX, ViewportY));
        }

        #endregion
    }

    /// <summary>
    /// 视口改变事件参数
    /// </summary>
    public class ViewportChangedEventArgs : EventArgs
    {
        public double X { get; }
        public double Y { get; }

        public ViewportChangedEventArgs(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}