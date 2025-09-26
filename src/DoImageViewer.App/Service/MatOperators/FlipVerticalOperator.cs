using OpenCvSharp;

namespace DoImageViewer.App.Service.MatOperators;

public class FlipVerticalOperator : IMatOperator
{
    public Mat? Run(Mat? mat)
    {
        if (mat == null || mat.Empty())
            return null;

        Mat? dst = null;
        try {
            dst = new Mat();
            Cv2.Flip(mat, dst, FlipMode.X);
            return dst;
        }
        catch (Exception ex) {
            dst?.Dispose();
            System.Diagnostics.Debug.WriteLine($"Mat左旋转90度失败: {ex.Message}");
            return null;
        }
    }
}
