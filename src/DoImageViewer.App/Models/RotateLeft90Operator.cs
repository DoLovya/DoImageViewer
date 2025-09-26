using OpenCvSharp;

namespace DoImageViewer.App.Models;

public class RotateLeft90Operator : IMatOperator
{
    public Mat? Run(Mat? mat)
    {
        if (mat == null || mat.Empty())
            return null;

        Mat? rotatedMat = null;
        try {
            rotatedMat = new Mat();
            Cv2.Rotate(mat, rotatedMat, RotateFlags.Rotate90Counterclockwise);
            return rotatedMat;
        }
        catch (Exception ex) {
            // 如果发生异常，确保释放已创建的Mat对象
            rotatedMat?.Dispose();
            System.Diagnostics.Debug.WriteLine($"Mat左旋转90度失败: {ex.Message}");
            return null;
        }
    }
}
