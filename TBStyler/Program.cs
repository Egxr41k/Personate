using Microsoft.VisualBasic.ApplicationServices;
using Personate.Settings;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace TBStyler;

internal class Program
{
    public static CancellationTokenSource Cancellation = new CancellationTokenSource();
    public static void Main(string[] args)
    {
        TaskbarSettingsDTO taskbarSettings = new SettingsService().Settings.Taskbar;
        Setuper setuper = new Setuper(taskbarSettings);
        while (true)
        {
            try
            {
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "-start":
                        setuper = new Setuper(taskbarSettings);
                        //taskbar.Start();
                        break;
                    case "-update":
                        //taskbar.Stop();
                        Cancellation.Cancel();
                        setuper = new Setuper(taskbarSettings);
                        //taskbar.Start();
                        break;
                    case "-stop":
                        //taskbar.Stop();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("command isn`t exist");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
