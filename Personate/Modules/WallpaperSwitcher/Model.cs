using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Personate.General;

namespace Personate.Modules.WallpaperSwitcher;
internal class Model
{
    public BitmapImage Image;
    public string Name;
    public string Path;
    public int Id;

    public string Resolution;
    public string Color;

    public Model(string? path)
    {
        Path = path ?? Open();
        Image = new BitmapImage(new Uri(Path));
        var Name_Id = Path.Split("\\").Last();

        Name = Name_Id.Split("_").First();
        Id = Convert.ToInt32(Name_Id.Split("_").Last().Split(".").First());
        Resolution = $"{Image.PixelWidth}" + "x" + $"{Image.PixelHeight}";
    }

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
            System.Drawing.Image.FromFile(Path).Save(
                sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
    }

    public void Set()
    {
        if (!Win32.SystemParametersInfo(0x14, 0, Path, 0x01 | 0x02))
            Console.WriteLine("Error");
    }

}