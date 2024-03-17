using System.Diagnostics;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using Personate.Settings;

namespace TBStyler;

public class Program
{
    public static CancellationTokenSource Cancellation;
    public static bool IsntCancel = !Cancellation?.IsCancellationRequested ?? true;

    static SettingsService settingsService;
    static Setuper taskbar;
    public static void Main(string[] args)
    {
        while (IsntCancel)
        {
            try
            {
                ExecuteCommand();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    private static void ExecuteCommand()
    {
        string? input = Console.ReadLine();

        switch (input)
        {
            case "-start":
                TBStylerInit();
                taskbar.Start();
                break;

            case "-update":
                Cancellation.Cancel();
                taskbar.Stop();
                
                TBStylerInit();
                taskbar.Start();
                break;

            case "-stop":
                Cancellation.Cancel();
                taskbar.Stop();
                Environment.Exit(0);
                break;

            default: 
                Console.WriteLine("command isn`t exist");
                break;
        }
    }

    private static void TBStylerInit()
    {
        Cancellation = new CancellationTokenSource();
        settingsService = new SettingsService();
        taskbar = new Setuper(settingsService);
    }
}
