using DoImageViewer.App.Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DoImageViewer.App.Controls
{
    public partial class ZoomableImageControl : UserControl, ITransform
    {
        #region 私有字段
        private Point _lastMousePosition;
        private bool _isDragging = false;
        private double _currentScale = 1.0;
        private const double _zoomFactor = 1.2;
        private TransformService _transformService;
        #endregion

        #region 依赖属性

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ZoomableImageControl),
                new PropertyMetadata(null, OnImageSourceChanged));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public double TranslateX => translateTransform.X;
        public double TranslateY => translateTransform.Y;

        private static void OnImageSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZoomableImageControl control) {
                control.ImageElement.Source = e.NewValue as ImageSource;
                control.ResetView();
            }
        }

        #endregion

        public ZoomableImageControl()
        {
            InitializeComponent();

            // 初始化变换服务
            _transformService = new TransformService(this);
        }

        #region 事件处理程序
        private void OnUserControlLoaded(object sender, RoutedEventArgs e)
        {
            FitToView();
        }

        private void ZoomableImageControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // 控件大小变化时，调整图像位置
            CenterImage();
        }

        private void OnImageCanvasMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point mousePosition = e.GetPosition(scrollViewer);
            double scaleFactor = e.Delta > 0 ? _zoomFactor : 1 / _zoomFactor;
            _currentScale = _transformService.ZoomAtPoint(_currentScale, mousePosition, scaleFactor);
            OnTransformChanged();
            e.Handled = true;
        }

        private void OnImageMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _lastMousePosition = e.GetPosition(scrollViewer);
            _isDragging = true;
            ImageElement.CaptureMouse();
            e.Handled = true;
        }

        private void OnImageElementMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // 结束拖拽
            _isDragging = false;
            ImageElement.ReleaseMouseCapture();
            e.Handled = true;
        }

        private void OnImageElementMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging) {
                // 计算鼠标移动距离
                Point currentPosition = e.GetPosition(scrollViewer);
                _transformService.Translate(_lastMousePosition, currentPosition);
                _lastMousePosition = currentPosition;
                OnTransformChanged();
                e.Handled = true;
            }
        }
        #endregion

        #region 公共属性和方法

        /// <summary>
        /// 获取当前缩放比例
        /// </summary>
        public double CurrentScale => _currentScale;

        /// <summary>
        /// 获取ScaleTransform引用
        /// </summary>
        public ScaleTransform ScaleTransform => scaleTransform;

        /// <summary>
        /// 获取TranslateTransform引用
        /// </summary>
        public TranslateTransform TranslateTransform => translateTransform;

        /// <summary>
        /// 变换改变事件
        /// </summary>
        public event EventHandler TransformChanged;

        /// <summary>
        /// 触发变换改变事件
        /// </summary>
        private void OnTransformChanged()
        {
            TransformChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 重置视图到初始状态
        /// </summary>
        public void ResetView()
        {
            CenterImage();
            FitToView();
        }

        /// <summary>
        /// 居中显示图像
        /// </summary>
        private void CenterImage()
        {
            if (ImageElement.Source == null)
                return;

            scaleTransform.ScaleX = 1.0;
            scaleTransform.ScaleY = 1.0;
            translateTransform.X = 0;
            translateTransform.Y = 0;
            _currentScale = 1.0;
        }

        public void SetScale(double scaleX, double scaleY)
        {
            scaleTransform.ScaleX = scaleX;
            scaleTransform.ScaleY = scaleY;
            _currentScale = scaleX;
        }

        public void AddTranslate(Vector delta)
        {
            AddTranslate(delta.X, delta.Y);
        }
        public void AddTranslate(double translateX, double translateY)
        {
            translateTransform.X += translateX;
            translateTransform.Y += translateY;
        }
        public void FitToView()
        {
            if (ImageSource is null) return;
            if (scrollViewer is null) return;

            double scaleX = scrollViewer.ActualWidth / ImageSource.Width;
            double scaleY = scrollViewer.ActualHeight / ImageSource.Height;
            double minScale = Math.Min(scaleX, scaleY);
            SetScale(minScale, minScale);

            double tx = scrollViewer.ActualWidth - ImageSource.Width * minScale;
            double ty = scrollViewer.ActualHeight - ImageSource.Height * minScale;
            AddTranslate(tx / 2, ty / 2);
        }
        #endregion
    }
}