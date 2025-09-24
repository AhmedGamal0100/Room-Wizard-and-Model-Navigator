using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution4.Models.ModelNavigatorCommandModels
{
    public partial class ElementTypeBind :ObservableObject
    {
        public string Name { get; set; }

        [ObservableProperty]
        bool isChecked;
        public List<Element> Elements{ get; set; } = new List<Element> { };
    }
}
