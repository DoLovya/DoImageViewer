using OpenCvSharp;

namespace DoImageViewer.App.Service;

/// <summary>
/// 图像旋转服务类，提供Mat对象的旋转功能
/// </summary>
public class ImageRotationService
{
    /// <summary>
    /// 将Mat对象左旋转90度
    /// </summary>
    /// <param name="mat">输入的Mat对象</param>
    /// <returns>旋转后的Mat对象，如果输入为空则返回null</returns>
    public static Mat? RotateLeft90(Mat? mat)
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

    public static Mat? RotateRight90(Mat? mat)
    {
        if (mat == null || mat.Empty())
            return null;

        Mat? rotatedMat = null;
        try {
            rotatedMat = new Mat();
            Cv2.Rotate(mat, rotatedMat, RotateFlags.Rotate90Clockwise);
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