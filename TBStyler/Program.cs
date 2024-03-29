using System.Diagnostics;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using Personate.Settings;

namespace TBStyler;

public class Program
{
    public static CancellationTokenSource Cancellation = new CancellationTokenSource();
    public static void Main(string[] args)
    {
        SettingsService settingsService = new SettingsService();
        Setuper taskbar = new Setuper(settingsService);
        while (true)
        {
            try
            {
                string? input = Console.ReadLine();

                switch (input)
                {
                    case "-start":
                        taskbar = new Setuper(settingsService);
                        taskbar.Start();
                        break;
                    case "-update":
                        taskbar.Stop();
                        Cancellation.Cancel();
                        //settingsService.Actualize();
                        taskbar = new Setuper(settingsService);
                        taskbar.Start();
                        break;
                    case "-stop":
                        taskbar.Stop();
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
