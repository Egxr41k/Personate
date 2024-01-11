using System.Diagnostics;
using System.Windows.Media.Imaging;
using Personate.General;

namespace Personate.Modules.CursorSwitcher;
internal class Model
{
    private const string PATH_TO_DEFAULT = "";

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
        Win32.InstallHinfSection(IntPtr.Zero, IntPtr.Zero, PATH_TO_DEFAULT, 1);
    }

    private bool InstallInfFile(string infPath)
    {
        try
        {
            // Формирование строки команды
            string command = $"Rundll32.exe setupapi,InstallHinfSection DefaultInstall 128 {infPath}";

            // Создание процесса и выполнение команды
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            process.StartInfo = startInfo;
            process.Start();

            // Передача команды ввода в командную строку
            process.StandardInput.WriteLine(command);
            process.StandardInput.Flush();
            process.StandardInput.Close();

            // Ожидание завершения процесса
            process.WaitForExit();

            // Проверка успешного завершения процесса
            return process.ExitCode == 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
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
