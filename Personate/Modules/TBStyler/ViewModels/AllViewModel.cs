using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personate.Modules.TBStyler.ViewModels;

internal class AllViewModel : ObservableObject
{
    public ObservableObject StyleSection { get; set; }
    public ObservableObject ColorPickerSection { get; set; }
    public ObservableObject AnimationSection { get; set; }
    public ObservableObject PositionSection { get; set; }
    public ObservableObject ExtraSection { get; set; }
    public ObservableObject MechanicsSection { get; set; }

    public AllViewModel(
        ObservableObject styleSection, 
        ObservableObject colorPickerSection, 
        ObservableObject animationSection, 
        ObservableObject positionSection, 
        ObservableObject extraSection, 
        ObservableObject mechanicsSection)
    {
        StyleSection = styleSection;
        ColorPickerSection = colorPickerSection;
        AnimationSection = animationSection;
        PositionSection = positionSection;
        ExtraSection = extraSection;
        MechanicsSection = mechanicsSection;
    }
}
