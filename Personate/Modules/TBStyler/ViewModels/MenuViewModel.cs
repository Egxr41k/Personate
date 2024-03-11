using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Personate.Settings;
using Personate.Modules.TBStyler.Models;

namespace Personate.Modules.TBStyler.ViewModels;

class MenuViewModel : ObservableObject
{
    public ObservableObject AllSection { get; set; }
    public ObservableObject StyleSection { get; set; }
    public ObservableObject ColorPickerSection { get; set; }
    public ObservableObject AnimationSection { get; set; }
    public ObservableObject PositionSection { get; set; }
    public ObservableObject ExtraSection { get; set; }
    public ObservableObject MechanicsSection { get; set; }

    private ObservableObject currentSection;
    public ObservableObject CurrentSection
    {
        get => currentSection;
        set => SetProperty(ref currentSection, value);
    }

    public ICommand ToAllCommand { get; set; }
    public ICommand ToStyleCommand { get; set; }
    public ICommand ToColorPickerCommand { get; set; }
    public ICommand ToAnimationCommand { get; set; }

    public ICommand ToPositionCommand { get; set; }
    public ICommand ToExtraCommand { get; set; }
    public ICommand ToMechanicsCommand { get; set; }

    public ICommand ResetCommand { get; set; }
    public ICommand ReloadCommand { get; set; }
    public ICommand StopCommand { get; set; }
    public ICommand RestartCommand { get; set; }
    public ICommand ApplyCommand { get; set; }


    SettingsService settingsService;
    MainModel model;
    public MenuViewModel()
    {
        settingsService = new SettingsService();
        TaskbarSettingsDTO defaultSettings = settingsService.Settings.Taskbar;

        model = new MainModel(settingsService);

        SectionsInit(defaultSettings);
        MenuCommandsInit();

        BehaviorCommandsInit();
    }

    private void SectionsInit(TaskbarSettingsDTO settings)
    {
        StyleSection = new StyleViewModel(settings);
        ColorPickerSection = new ColorPickerViewModel(settings);
        AnimationSection = new AnimationViewModel(settings);
        PositionSection = new PositionViewModel(settings);
        ExtraSection = new ExtraViewModel(settings);
        MechanicsSection = new MechanicsViewModel(settings);

        AllSection = new AllViewModel(
            StyleSection,
            ColorPickerSection,
            AnimationSection,
            PositionSection,
            ExtraSection,
            MechanicsSection
        );
        CurrentSection = StyleSection;
    }

    private void MenuCommandsInit()
    {
        ToAllCommand = new RelayCommand(() => NavigateTo(AllSection));
        ToStyleCommand = new RelayCommand(() => NavigateTo(StyleSection));
        ToColorPickerCommand = new RelayCommand(() => NavigateTo(ColorPickerSection));
        ToAnimationCommand = new RelayCommand(() => NavigateTo(AnimationSection));
        ToPositionCommand = new RelayCommand(() => NavigateTo(PositionSection));
        ToExtraCommand = new RelayCommand(() => NavigateTo(ExtraSection));
        ToMechanicsCommand = new RelayCommand(() => NavigateTo(MechanicsSection));
    }

    private void NavigateTo(ObservableObject target)
    {
        CurrentSection = target;
    }

    private void BehaviorCommandsInit()
    {
        ResetCommand = new RelayCommand(() => model.Reset());
        ReloadCommand = new RelayCommand(() => model.Reload());
        StopCommand = new RelayCommand(() => model.Stop());
        RestartCommand = new RelayCommand(() => model.Restart());
        ApplyCommand = new RelayCommand(() =>
        {
            settingsService.Save();
            model.Apply();
        });
    }
}
