using System.Windows.Media.Imaging;
using Personate.General;

namespace Personate.Modules.CursorSwitcher;
internal class Model
{
    private static string PATH_TO_DEFAULT = MenuViewModel.CursorDirectory + "\\default\\Install.inf";

    public BitmapImage Image;
    public string Name;
    public string Path;
    public int Id;

    public int Count;
    public string Color;

    public Model(string? path)
    {
        Path = path ?? Open();
        Image = InitImage(Path + "\\pointer.cur");

        var Name_Color = Path.Split("\\").Last();
        Name = Name_Color.Split("_").First();
        Color = Name_Color.Split("_").Last();
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
