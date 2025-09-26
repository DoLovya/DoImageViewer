using OpenCvSharp;

namespace DoImageViewer.App.Service;

public interface IMatOperator
{
    Mat? Run(Mat? mat);
}
