using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Personate.Settings;
using Personate.Modules.TBStyler.Models;

namespace Personate.Modules.TBStyler.ViewModels;

class AnimationViewModel : ObservableObject
{
    private readonly Dictionary<string, AnimationStyle> animationStylesWithNames = new()
    {
        { "linear", AnimationStyles.Linear },

        { "expoeaseout", AnimationStyles.ExpoEaseOut },
        { "expoeasein", AnimationStyles.ExpoEaseIn },
        { "expoeaseinout", AnimationStyles.ExpoEaseInOut },
        { "expoeaseoutin", AnimationStyles.ExpoEaseOutIn },

        { "circeaseout", AnimationStyles.CircEaseOut },
        { "circeasein", AnimationStyles.CircEaseIn },
        { "circeaseinout", AnimationStyles.CircEaseInOut },
        { "circeaseoutin", AnimationStyles.CircEaseOutIn },

        { "quadeaseout", AnimationStyles.QuadEaseOut },
        { "quadeasein", AnimationStyles.QuadEaseIn },
        { "quadeaseinout", AnimationStyles.QuadEaseInOut },
        { "quadeaseoutin", AnimationStyles.QuadEaseOutIn },

        { "sineeaseout", AnimationStyles.SineEaseOut },
        { "sineeasein", AnimationStyles.SineEaseIn },
        { "sineeaseinout", AnimationStyles.SineEaseInOut },
        { "sineeaseoutin", AnimationStyles.SineEaseOutIn },

        { "cubiceaseout", AnimationStyles.CubicEaseOut },
        { "cubiceasein", AnimationStyles.CubicEaseIn },
        { "cubiceaseinout", AnimationStyles.CubicEaseInOut },
        { "cubiceaseoutin", AnimationStyles.CubicEaseOutIn },

        { "quarteaseout", AnimationStyles.QuartEaseOut },
        { "quarteasein", AnimationStyles.QuartEaseIn },
        { "quarteaseinout", AnimationStyles.QuartEaseInOut },
        { "quarteaseoutin", AnimationStyles.QuartEaseOutIn },

        { "quinteaseout", AnimationStyles.QuintEaseOut },
        { "quinteasein", AnimationStyles.QuintEaseIn },
        { "quinteaseinout", AnimationStyles.QuintEaseInOut },
        { "quinteaseoutin", AnimationStyles.QuintEaseOutIn },

        { "elasticeaseout", AnimationStyles.ElasticEaseOut },
        { "elasticeasein", AnimationStyles.ElasticEaseIn },
        { "elasticeaseinout", AnimationStyles.ElasticEaseInOut },
        { "elasticeaseoutin", AnimationStyles.ElasticEaseOutIn },

        { "bounceeaseout", AnimationStyles.BounceEaseOut },
        { "bounceeasein", AnimationStyles.BounceEaseIn },
        { "bounceeaseinout", AnimationStyles.BounceEaseInOut },
        { "bounceeaseoutin", AnimationStyles.BounceEaseOutIn },

        { "backeaseout", AnimationStyles.BackEaseOut },
        { "backeasein", AnimationStyles.BackEaseIn },
        { "backeaseinout", AnimationStyles.BackEaseInOut },
        { "backeaseoutin", AnimationStyles.BackEaseOutIn },
    };

    public List<string> AnimationStyleNames =>
        animationStylesWithNames.Keys.ToList();

    private string animationStyleName;
    public string AnimationStyleName
    {
        get => animationStyleName;
        set
        {
            SetProperty(ref animationStyleName, value);
            settings.AnimationStyle = value;
        }
    }

    private string onBatteryAnimationStyleName;
    public string OnBatteryAnimationStyleName
    {
        get => onBatteryAnimationStyleName;
        set
        {
            SetProperty(ref onBatteryAnimationStyleName, value);
            settings.OnBatteryAnimationStyle = value;
        }
    }

    private int animationSpeed;
    public int AnimationSpeed
    {
        get => animationSpeed;
        set
        {
            SetProperty(ref animationSpeed, value);
            settings.AnimationSpeed = value;
        }
    }

    private TaskbarSettingsDTO settings { get; set; }
    public AnimationViewModel(TaskbarSettingsDTO settings)
    {
        this.settings = settings;

        animationStyleName = 
            animationStylesWithNames.ContainsKey(settings.AnimationStyle) ? 
                settings.AnimationStyle : 
                AnimationStyleNames.First();

        onBatteryAnimationStyleName = 
            animationStylesWithNames.ContainsKey(settings.OnBatteryAnimationStyle) ? 
                settings.OnBatteryAnimationStyle : 
                AnimationStyleNames.First();

        AnimationSpeed = settings.AnimationSpeed;
    }
}
