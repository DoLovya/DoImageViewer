using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace DoImageViewer.App.Helpers
{
    public static class TitleBarHelper
    {
        // Windows API 常量
        private const int WM_NCPAINT = 0x0085;
        private const int WM_DWMCOLORIZATIONCOLORCHANGED = 0x0320;
        private const int DWMWA_CAPTION_COLOR = 35;

        // DWM API 函数
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        /// <summary>
        /// 设置窗口标题栏的背景颜色
        /// </summary>
        /// <param name="window">要修改的窗口</param>
        /// <param name="color">标题栏背景颜色</param>
        public static void SetTitleBarBackgroundColor(Window window, Color color)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            // 确保窗口已初始化
            if (PresentationSource.FromVisual(window) == null) {
                window.SourceInitialized += (s, e) => SetTitleBarColor(window, color);
                return;
            }

            SetTitleBarColor(window, color);
        }

        private static void SetTitleBarColor(Window window, Color color)
        {
            // 获取窗口句柄
            IntPtr hwnd = new WindowInteropHelper(window).Handle;
            if (hwnd == IntPtr.Zero)
                return;

            // 转换颜色为COLORREF格式 (0x00BBGGRR)
            int colorRef = (color.R) | (color.G << 8) | (color.B << 16);

            // 设置标题栏颜色
            DwmSetWindowAttribute(hwnd, DWMWA_CAPTION_COLOR, ref colorRef, Marshal.SizeOf(typeof(int)));
        }
    }
}