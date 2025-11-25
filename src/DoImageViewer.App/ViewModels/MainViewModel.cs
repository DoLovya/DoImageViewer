using DoImageViewer.App.Models;
using DoImageViewer.App.Service;
using OpenCvSharp;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
namespace DoImageViewer.App.ViewModels;

public class MainViewModel : BindableBase
{
    private string _fileNameTitle = "DoImageViewer";
    private Mat? _matSource;
    private ObservableCollection<ImageItemModel> _imageItems;
    private ImageItemModel _selectedImageItem;

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

    public ObservableCollection<ImageItemModel> ImageItems
    {
        get => _imageItems;
        set => SetProperty(ref _imageItems, value);
    }
    public ImageItemModel SelectedImageItem
    {
        get => _selectedImageItem;
        set
        {
            SetProperty(ref _selectedImageItem, value);
            MatSource = Cv2.ImRead(value.ImagePath);
            FileNameTitle = value.ImageName;
        }
    }

    public ICommand RunMatOperatorCommand { get; init; }

    public MainViewModel()
    {
        string imagePath = @"C:\Users\zh306\Pictures\测试集\autumn-decor-wallpaper-3840x2160-woodland-scenery-landscape-artwork-26734.jpg";

        if (File.Exists(imagePath)) {
            try {
                FileNameTitle = Path.GetFileName(imagePath);
                MatSource = Cv2.ImRead(imagePath);
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"加载默认图片失败: {ex.Message}");
                FileNameTitle = "DoImageViewer";
                MatSource = null;
            }
        }
        else {
            FileNameTitle = "DoImageViewer";
            MatSource = null;
        }

        string? folderPath = Path.GetDirectoryName(imagePath);
        if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath)) {
            var collection = new ObservableCollection<ImageItemModel>();
            foreach (var file in Directory.EnumerateFiles(folderPath)) {
                string ext = Path.GetExtension(file).ToLowerInvariant();
                if (ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".bmp" || ext == ".gif" || ext == ".tiff" || ext == ".webp") {
                    collection.Add(new ImageItemModel() {
                        ImagePath = file,
                        ImageName = Path.GetFileName(file),
                    });
                }
            }
            ImageItems = collection;
        }
        else {
            ImageItems = [];
        }

        RunMatOperatorCommand = new DelegateCommand<IMatOperator>(RunMatOperator);
    }

    private void RunMatOperator(IMatOperator @operator)
    {
        MatSource = @operator?.Run(MatSource);
    }
}
