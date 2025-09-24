using CommunityToolkit.Mvvm.ComponentModel;
using Solution4.Models.RoomWizardCommandModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution4.ViewModels
{
    public partial class RoomWizardViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<RoomBinding> roomBindings = new ObservableCollection<RoomBinding>();
    }
}
