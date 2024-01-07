using System.Windows.Media.Imaging;
using Personate.General;

namespace Personate.Modules.CursorSwitcher;
internal class Model
{
    private const string PATH_TO_DEFAULT = "";

    public BitmapImage Image;
    public string Name;
    public string Path;

    public int Count;

    public Model(string? path)
    {
        Path = path ?? Open();
        Image = InitImage(Path + "\\pointer.cur");
        Name = Path.Split('\\').Last();
    }

    public string Open()
    {
        OpenFileDialog ofd = new()
        {
            Filter = "Inf files (*.inf)|*.inf|All Files (*.*)|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        };
        if (ofd.ShowDialog() != true) return "";
        else return ofd.FileName;
    }

    public void Set()
    {
        Win32.InstallHinfSection(IntPtr.Zero, IntPtr.Zero, Path, 1);
    }

    public void ToDefault()
    {
        Win32.InstallHinfSection(IntPtr.Zero, IntPtr.Zero, PATH_TO_DEFAULT, 1);
    }

    private BitmapImage InitImage(string pathToCursor)
    {
        if (!File.Exists(pathToCursor)) return new BitmapImage();

        using (var stream = new FileStream(pathToCursor, FileMode.Open, FileAccess.Read))
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }
    }
}
