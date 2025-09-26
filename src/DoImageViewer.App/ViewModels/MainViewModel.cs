using DoImageViewer.App.Models;
using OpenCvSharp;
using Prism.Commands;
using Prism.Mvvm;
using System.IO;
using System.Windows.Input;
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

    public ICommand RunMatOperatorCommand { get; init; }

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

        RunMatOperatorCommand = new DelegateCommand<IMatOperator>(RunMatOperator);
    }

    private void RunMatOperator(IMatOperator @operator)
    {
        MatSource = @operator?.Run(MatSource);
    }
}
