using System.Windows.Media.Imaging;

namespace Personate.MVVM.Model;
static class WallpaperModel
{
    public static string path;

    public static bool OpenImage()
    {
        bool result = false;
        OpenFileDialog ofd = new()
        {
            Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        };
        if (ofd.ShowDialog() == true)
        {
            path = ofd.FileName;
            result = true;
        }
        return result;
    }

    public static void SaveImage()
    {
        SaveFileDialog sfd = new()
        {
            Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        };
        if (sfd.ShowDialog() == true)
            System.Drawing.Image.FromFile(path).Save(
                sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
    }

    public static void SetImageToBg()
    {
        if (!Win32.SystemParametersInfo(0x14, 0, path, 0x01 | 0x02))
            Console.WriteLine("Error");
    }
}
public class Wallpaper
{
    public BitmapImage image;
    public string path;
    public string resolution;
    public string name;

    public Wallpaper(string path)
    {
        this.path = path;
        image = new BitmapImage(new Uri(path));
        name = path.Split('\\').Last();
        resolution = $"{image.PixelWidth}" + "x" + $"{image.PixelHeight}";
    }
    public bool OpenImage()
    {
        bool result = false;
        OpenFileDialog ofd = new()
        {
            Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        };
        if (ofd.ShowDialog() == true)
        {
            path = ofd.FileName;
            result = true;
        }
        return result;
    }

    public void SaveImage()
    {
        SaveFileDialog sfd = new()
        {
            Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        };
        if (sfd.ShowDialog() == true)
            System.Drawing.Image.FromFile(path).Save(
                sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
    }

    public void SetImageToBg()
    {
        if (!Win32.SystemParametersInfo(0x14, 0, path, 0x01 | 0x02))
            Console.WriteLine("Error");
    }

}