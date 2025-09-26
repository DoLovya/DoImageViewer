using DoImageViewer.App.Service;
using OpenCvSharp;
using Prism.Commands;
using Prism.Mvvm;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xaml;
namespace DoImageViewer.App.ViewModels;

public class MainViewModel : BindableBase
{
    private string _fileNameTitle = "DoImageViewer";
    private Mat? _matSource;

    public string FileNameTitle
    {
        get => _fileNameTitle;
        set => SetProperty(ref _fileNameTitle, value);
    }
    public Mat? MatSource
    {
        get => _matSource;
        set => SetProperty(ref _matSource, value);
    }

    public ICommand RotateLeftCommand { get; init; }
    public ICommand RotateRightCommand { get; init; }

    public MainViewModel()
    {
        // 检查默认图片路径是否存在
        string imagePath = @"C:\Users\zh306\Pictures\测试集\autumn-decor-wallpaper-3840x2160-woodland-scenery-landscape-artwork-26734.jpg";

        if (File.Exists(imagePath)) {
            try {
                FileNameTitle = Path.GetFileName(imagePath);
                MatSource = Cv2.ImRead(imagePath);
            }
            catch (Exception ex) {
                // 如果加载失败，保持默认状态
                System.Diagnostics.Debug.WriteLine($"加载默认图片失败: {ex.Message}");
                FileNameTitle = "DoImageViewer";
                MatSource = null;
            }
        }
        else {
            // 文件不存在时保持默认状态
            FileNameTitle = "DoImageViewer";
            MatSource = null;
        }

        RotateLeftCommand = new DelegateCommand(RotateLeft);
        RotateRightCommand = new DelegateCommand(RotateRight);
    }

    private void RotateLeft()
    {
        ImageRotationService imageRotationService = new();
        MatSource = ImageRotationService.RotateLeft90(MatSource);
    }
    private void RotateRight()
    {
        ImageRotationService imageRotationService = new();
        MatSource = ImageRotationService.RotateRight90(MatSource);
    }
}
