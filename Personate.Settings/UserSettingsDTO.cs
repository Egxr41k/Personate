using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personate.Settings;

public class UserSettingsDTO
{
    public string Cursor { get; set; }
    public string Wallpaper { get; set; }
    public TaskbarSettingsDTO Taskbar {  get; set; }
}
