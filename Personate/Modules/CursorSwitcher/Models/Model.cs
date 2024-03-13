using System.Windows.Media.Imaging;
using Personate.General;
using Personate.Modules.CursorSwitcher.ViewModels;

namespace Personate.Modules.CursorSwitcher.Models;
internal class Model
{
    public BitmapImage Image { get; private set; }
    public string Name { get; private set; }
    public string Path { get; private set; }
    public int Id { get; private set; }
    public int Count { get; private set; }
    public string Color { get; private set; }

    public static Model FromFileExplorer()
    {
        string path = Open();
        return new Model(path);
    }

    public Model(string path)
    {
        Path = path;
        Image = InitImage(Path + "\\pointer.cur");

        var Name_Color = Path.Split("\\").Last();
        Name = Name_Color.Split("_").First();
        Color = Name_Color.Split("_").Last();
    }

    private static string Open()
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
        InstallInfFile(Path + "\\Install.inf");
    }

    public void ToDefault()
    {
        InstallInfFile(MenuViewModel.PathToDefaultCursor + "\\Install.inf");
    }

    private void InstallInfFile(string pathToInfFile)
    {
        if (!File.Exists(pathToInfFile)) return;

        Win32.ExecuteWithCmd($"Rundll32.exe setupapi,InstallHinfSection DefaultInstall 128 {pathToInfFile}");
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
