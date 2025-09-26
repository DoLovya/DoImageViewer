using OpenCvSharp;

namespace DoImageViewer.App.Models;

public interface IMatOperator
{
    Mat? Run(Mat? mat);
}
