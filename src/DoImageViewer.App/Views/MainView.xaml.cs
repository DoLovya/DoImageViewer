using System.Windows;
using System.Windows.Media;
using DoImageViewer.App.Helpers;

namespace DoImageViewer.App.Views;

public partial class MainView : Window
{
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
        };
    }
}