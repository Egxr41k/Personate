using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personate.Settings;

public class TaskbarSettingsDTO
{
    public bool Pause { get; set; }
    public bool HorizontalOrientation { get; set; }

    public int TaskbarStyle { get; set; }
    public int SecondaryTaskbarStyle { get; set; }
    public int PrimaryTaskbarOffset { get; set; }
    public int SecondaryTaskbarOffset { get; set; }
    public int CenterPrimaryOnly { get;  set; }
    public int CenterSecondaryOnly { get; set; }
    public string AnimationStyle { get; set; } = string.Empty;
    public int AnimationSpeed { get; set; }
    public int LoopRefreshRate { get; set; }
    public int CenterInBetween { get; set; }
    public int FixToolbarsOnTrayChange { get; set; }
    public int SkipResolution { get; set; }
    public int SkipResolution2 { get; set; }
    public int SkipResolution3 { get; set; }
    public int CheckFullscreenApp { get; set; }
    public int DefaultTaskbarStyleOnWinMax { get; set; }
    public int DontCenterTaskbar { get; set; }
    public int HidePrimaryStartButton { get; set; }
    public int HideSecondaryStartButton { get; set; }
    public int HidePrimaryNotifyWnd { get; set; }
    public int HideSecondaryNotifyWnd { get; set; }
    public int ShowTrayIcon { get; set; }
    public int TaskbarStyleOnMax { get; set; }
    public int TaskbarStyleRed { get; set; }
    public int TaskbarStyleGreen { get; set; }
    public int TaskbarStyleBlue { get; set; }
    public int TaskbarStyleAlpha { get; set; }
    public int ConsoleEnabled { get; set; }
    public int StickyStartButton { get; set; }
    public int TotalPrimaryOpacity { get; set; }
    public int TotalSecondaryOpacity { get; set; }
    public int RevertZeroBeyondTray { get; set; }
    public int TaskbarRounding { get; set; }
    public int TaskbarSegments { get; set; }

    public int UseUIA { get; set; }

    public string OnBatteryAnimationStyle { get; set; } = string.Empty;
    public int OnBatteryLoopRefreshRate { get; set; }

}
