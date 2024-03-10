using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBStyler.Settings;

namespace TBStyler.WPF.ViewModels;

class PositionViewModel : ObservableObject
{
    public string Name { get; set; } = "Position";
    public string Description { get; set; } = "Position";

    private int primaryOffset;
    public int PrimaryOffset
    {
        get => primaryOffset;
        set
        {
            SetProperty(ref primaryOffset, value);
            settings.PrimaryTaskbarOffset = value;
        }
    }

    private int secondaryOffset;
    public int SecondaryOffset
    {
        get => secondaryOffset;
        set
        {
            SetProperty(ref secondaryOffset, value);
            settings.SecondaryTaskbarOffset = value;
        }
    }

    private int firstSkipResiolution;
    public int FirstSkipResiolution
    {
        get => firstSkipResiolution;
        set
        {
            SetProperty(ref firstSkipResiolution, value);
            settings.SkipResolution = value;
        }
    }

    private int secondSkipResiolution;
    public int SecondSkipResiolution
    {
        get => secondSkipResiolution;
        set
        {
            SetProperty(ref secondSkipResiolution, value);
            settings.SkipResolution2 = value;
        }
    }

    private int thirdSkipResiolution;
    public int ThirdSkipResiolution
    {
        get => thirdSkipResiolution;
        set
        {
            SetProperty(ref thirdSkipResiolution, value);
            settings.SkipResolution3 = value;
        }
    }

    private bool compensateTrayClock;
    public bool CompensateTrayClock
    {
        get => compensateTrayClock;
        set
        {
            SetProperty(ref compensateTrayClock, value);
            settings.CenterInBetween = value == true ? 1 : 0;
        }
    }

    private bool dontCenter;
    public bool DontCenter
    {
        get => dontCenter;
        set
        {
            SetProperty(ref dontCenter, value);
            settings.DontCenterTaskbar = value == true ? 1 : 0;
        }
    }

    private bool revertToZeroBeyondTray;
    public bool RevertToZeroBeyondTray
    {
        get => revertToZeroBeyondTray;
        set
        {
            SetProperty(ref revertToZeroBeyondTray, value);
            settings.RevertZeroBeyondTray = value == true ? 1 : 0;
        }
    }

    private SettingsDTO settings { get; set; }
    public PositionViewModel(SettingsDTO settings)
    {
        this.settings = settings;

        PrimaryOffset = settings.PrimaryTaskbarOffset;
        SecondaryOffset = settings.SecondaryTaskbarOffset;

        FirstSkipResiolution = settings.SkipResolution;
        SecondSkipResiolution = settings.SkipResolution2;
        ThirdSkipResiolution = settings.SkipResolution3;

        CompensateTrayClock = settings.CenterInBetween == 1;
        DontCenter = settings.DontCenterTaskbar == 1;
        RevertToZeroBeyondTray = settings.RevertZeroBeyondTray == 1;
    }
}
