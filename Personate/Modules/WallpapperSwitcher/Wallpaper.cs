using System.Windows.Media.Imaging;
using Personate.General;

namespace Personate.Modules.WallpapperSwitcher;
internal class Wallpaper
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

    public Wallpaper() => path = Open();

    public string Open()
    {
        OpenFileDialog ofd = new()
        {
            Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        };
        if (ofd.ShowDialog() != true) return "";
        else return ofd.FileName;
    }

    public void Save()
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

    public void SetToBg()
    {
        if (!Win32.SystemParametersInfo(0x14, 0, path, 0x01 | 0x02))
            Console.WriteLine("Error");
    }

}