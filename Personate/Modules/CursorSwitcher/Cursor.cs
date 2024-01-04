using System.Diagnostics;

namespace Personate.Modules.CursorSwitcher;
internal class Cursor
{
    public string CursorImg = "";
    public int CursorCount;
    public string CursorTitle = "";
    public string CursorColor = "";
    public string path = "";

    public void Open()
    {
        OpenFileDialog ofd = new()
        {
            Filter = "Inf files (*.inf)|*.inf|All Files (*.*)|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        };
        if (ofd.ShowDialog() == true) path = ofd.FileName;
    }

    public void Save()
    {
        SaveFileDialog sfd = new()
        {
            Filter = "Cursor files (*.cur)|*.cur|All Files (*.*)|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
        };
        if (sfd.ShowDialog() == true)
        {

            ////Cursor cursor = new(sfd.FileName);
            //Cursor cr = new Cursor(Cursors.Arrow.Handle);


            ////GET ICON FROM YOUR CURSOR HANDLE
            //Icon ico = Icon.FromHandle(cr.Handle);

            ////WRITE TO FILE STREAM
            //using (FileStream fs = new FileStream(@"c:\users\<<XXXX>>\test.cur", FileMode.Create, FileAccess.Write))
            //ico.Save(fs);
            ////Cursor.FromFile(path).Save(sfd.FileName, ImageFormat.Jpeg);
        }
    }
    public void Set()
    {
        //Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Arrow", path);
        using var process = new Process();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.FileName = "cmd.exe";

        process.StartInfo.Arguments = "/c " + Environment.SpecialFolder.Windows + "\\System32\\InfDefaultInstall.exe " + "\"" + path + "\""; // where driverPath is path of .inf file
        process.Start();
        process.WaitForExit();
        process.Dispose();

        //Win32.InstallHinfSection(IntPtr.Zero, IntPtr.Zero, $"DefaultInstall 129 {path}", 1);
    }
}
