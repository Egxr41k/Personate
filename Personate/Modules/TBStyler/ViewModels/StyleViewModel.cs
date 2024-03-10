using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personate.Settings;

namespace Personate.Modules.TBStyler.ViewModels;

class StyleViewModel : ObservableObject
{
    private Dictionary<string, string> stylesWithDescriptions = new()
    {
        { "Default", "Keep the taskbar background as it has always been." },
        { "Transparent", "Keep the taskbar background as it has always been." },
        { "Blur", "Make the taskbar background Blurred with a gradient color." },
        { "Acrylic (can flicker)", "Make the taskbar background Acrylic/Fluent with a gradient color." },
        { "Transparent Gradient", "Make the taskbar background Transparent with a gradient color." },
        { "Opaque", "Make the taskbar background Opaque with a gradient color."}
    };
    public List<string> Styles => stylesWithDescriptions.Keys.ToList();

    private string selectedStyle;
    public string SelectedStyle
    {
        get => selectedStyle; 
        set 
        {
            SetProperty(ref selectedStyle, value);

            settings.TaskbarStyle = 
                stylesWithDescriptions.ContainsKey(value) ? 
                    stylesWithDescriptions.Keys.ToList().IndexOf(value) : 
                    0;

            StyleDescription = stylesWithDescriptions[value];
        }
    }

    private string styleDescription;
    public string StyleDescription
    {
        get => styleDescription;
        set
        {
            SetProperty(ref styleDescription, value);
        }
    }

    private bool backToDefault;
    public bool BackToDefault
    {
        get => backToDefault;
        set
        {
            SetProperty(ref backToDefault, value);
            settings.DefaultTaskbarStyleOnWinMax = value == true ? 1 : 0;
        }
    }

    private int primaryOpacity;
    public int PrimaryOpacity
    {
        get => primaryOpacity;
        set
        {
            SetProperty(ref primaryOpacity, value);
            settings.PrimaryTaskbarOffset = value;
        }
    }

    private int secondaryOpacity;
    public int SecondaryOpacity
    {
        get => secondaryOpacity;
        set
        {
            SetProperty(ref secondaryOpacity, value);
            settings.TotalSecondaryOpacity = value;
        }
    }

    private bool separateToSegments;
    public bool SeparateToSegments
    {
        get => separateToSegments;
        set
        {
            SetProperty(ref separateToSegments, value);
            settings.TaskbarSegments = value == true ? 1 : 0;
        }
    }

    private int cornerRadius;
    public int CornerRadius
    {
        get => cornerRadius;
        set
        {
            SetProperty(ref cornerRadius, value);
            settings.TaskbarRounding = value;
        }
    }

    private SettingsDTO settings { get; set; }
    public StyleViewModel(SettingsDTO settings)
    {
        this.settings = settings;

        SelectedStyle = 
            stylesWithDescriptions.Count >= settings.TaskbarStyle ? 
                stylesWithDescriptions.Keys.ToArray()[settings.TaskbarStyle] : 
                Styles.First();

        CornerRadius = settings.TaskbarRounding;

        SeparateToSegments = settings.TaskbarSegments == 1;

        PrimaryOpacity = settings.TotalPrimaryOpacity;
        SecondaryOpacity = settings.TotalSecondaryOpacity;

        BackToDefault = settings.DefaultTaskbarStyleOnWinMax == 1;
    }
}
