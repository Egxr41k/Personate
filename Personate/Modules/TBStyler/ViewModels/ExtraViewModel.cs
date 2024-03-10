using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TBStyler.Settings;

namespace TBStyler.WPF.ViewModels;

class ExtraViewModel : ObservableObject
{
	private bool onlyCenterPrimaryTaskbar;
	public bool OnlyCenterPrimaryTaskbar
	{
		get => onlyCenterPrimaryTaskbar;
		set
		{
			SetProperty(ref onlyCenterPrimaryTaskbar, value);
			settings.CenterPrimaryOnly = value == true ? 1 : 0;
        }
	}

	private bool onlyCenterSecondaryTaskbar;
	public bool OnlyCenterSecondaryTaskbar
    {
        get => onlyCenterSecondaryTaskbar;
        set
        {
            SetProperty(ref onlyCenterSecondaryTaskbar, value);
			settings.CenterSecondaryOnly = value == true ? 1 : 0;
        }
    }

    private bool updateTaskbarToolbars;
	public bool UpdateTaskbarToolbars
	{
		get => updateTaskbarToolbars;
        set 
		{
			SetProperty(ref updateTaskbarToolbars, value);
            settings.FixToolbarsOnTrayChange  = value == true ? 1 : 0;
        }
	}

    private bool pauseLooperWhenFullScreen;
    public bool PauseLooperWhenFullScreen
    {
        get => pauseLooperWhenFullScreen;
        set
		{
            SetProperty(ref pauseLooperWhenFullScreen, value);
			settings.CheckFullscreenApp = value == true ? 1 : 0;
        }
    }

    private bool hidePrimaryStartButton;
    public bool HidePrimaryStartButton
    {
        get => hidePrimaryStartButton;
        set 
        {
            SetProperty(ref hidePrimaryStartButton, value);
            settings.HidePrimaryStartButton = value == true ? 1 : 0;
        }
    }

	private bool hideSecondaryStartButton;
	public bool HideSecondaryStartButton
	{
		get => hideSecondaryStartButton;
		set 
        {
            SetProperty(ref hidePrimaryStartButton, value);
            settings.HideSecondaryStartButton = value == true ? 1 : 0;
        }
    }

	private bool hidePrimaryTrayArea;

	public bool HidePrimaryTrayArea
	{
		get => hidePrimaryTrayArea;
		set 
        {
            SetProperty(ref hidePrimaryTrayArea, value);
            settings.HidePrimaryNotifyWnd = value == true ? 1 : 0;
        }
    }

	private bool hideSecondaryTrayArea;
	public bool HideSecondaryTrayArea
	{
        get => hideSecondaryTrayArea;
        set 
        {
            SetProperty(ref hideSecondaryTrayArea, value);
            settings.HideSecondaryNotifyWnd = value == true ? 1 : 0;
        }
    }

	private bool showConsole;
	public bool ShowConsole
	{
		get => showConsole;
		set
        {
            SetProperty(ref showConsole, value);
            settings.ConsoleEnabled = value == true ? 1 : 0;
        }
	}

    private bool showTrayIcon;
	public bool ShowTrayIcon
	{
		get => showTrayIcon;
		set
        {
            SetProperty(ref showTrayIcon, value);
            settings.ShowTrayIcon = value == true ? 1 : 0;
        }
	}

    private SettingsDTO settings { get; set; }
    public ExtraViewModel(SettingsDTO settings)
	{
		this.settings = settings;

		OnlyCenterPrimaryTaskbar = settings.CenterPrimaryOnly == 1;
		OnlyCenterSecondaryTaskbar = settings.CenterSecondaryOnly == 1;
		
		UpdateTaskbarToolbars = settings.FixToolbarsOnTrayChange == 1;
		
		PauseLooperWhenFullScreen = settings.CheckFullscreenApp == 1;

        HidePrimaryStartButton = settings.HidePrimaryStartButton == 1;
		HideSecondaryStartButton = settings.HideSecondaryStartButton == 1;

		HidePrimaryTrayArea = settings.HidePrimaryNotifyWnd == 1;
		HideSecondaryTrayArea = settings.HideSecondaryNotifyWnd == 1;

        ShowConsole = settings.ConsoleEnabled == 1;
        ShowTrayIcon = settings.ShowTrayIcon == 1;
    }
}
