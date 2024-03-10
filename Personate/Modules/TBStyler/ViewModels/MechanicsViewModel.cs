using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TBStyler.Settings;

namespace TBStyler.WPF.ViewModels;

class MechanicsViewModel : ObservableObject
{
    private int refreshRate;
    public int RefreshRate
    {
        get => refreshRate;
        set
        {
            SetProperty(ref refreshRate, value);
            settings.LoopRefreshRate = value;
        }
    }

    private int refreshRateOnBattery;
    public int RefreshRateOnBattery
    {
        get => refreshRateOnBattery;
        set
        {
            SetProperty(ref refreshRateOnBattery, value);
            settings.OnBatteryLoopRefreshRate = value;
        }
    }

    public string[] WaysToRetriveData = 
        ["MSAA", "UIAutomation"];

    private string selectedWayToRetriveData;
    public string SelectedWayToRetriveData
    {
        get => selectedWayToRetriveData;
        set => SetProperty(ref selectedWayToRetriveData, value);
    }

    public ICommand MoreDetailsCommand;

    private SettingsDTO settings { get; set; }
    public MechanicsViewModel(SettingsDTO settings)
    {
        this.settings = settings;

        RefreshRate = settings.LoopRefreshRate;
        RefreshRateOnBattery = settings.OnBatteryLoopRefreshRate;

        SelectedWayToRetriveData = WaysToRetriveData.First();

        MoreDetailsCommand = new RelayCommand(() =>
        {
            Process.Start("https://docs.microsoft.com/en-us/windows/win32/winauto/microsoft-active-accessibility-and-ui-automation-compared");
        });
    }
}
