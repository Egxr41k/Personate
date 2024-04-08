using Microsoft.VisualBasic.ApplicationServices;
using Personate.Settings;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace TBStyler;

internal class Program
{
    public static void Main(string[] args)
    {
        SettingsService settingsService = new SettingsService();
        Setuper taskbar = new Setuper(settingsService.Settings.Taskbar);
    }
}
