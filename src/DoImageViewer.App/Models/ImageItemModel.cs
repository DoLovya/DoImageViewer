using Prism.Mvvm;

namespace DoImageViewer.App.Models;

public class ImageItemModel : BindableBase
{
    private string _imagePath = string.Empty;
    private string _imageName = string.Empty;

    public string ImagePath
    {
        get => _imagePath;
        set => SetProperty(ref _imagePath, value);
    }
    public string ImageName
    {
        get => _imageName;
        set => SetProperty(ref _imageName, value);
    }
}
