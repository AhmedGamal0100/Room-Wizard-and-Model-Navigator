using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution4.Models.ModelNavigatorCommandModels
{
    public partial class CategoryBind : ObservableObject
    {
        public string Name{ get; set; }

        [ObservableProperty]
        bool isChecked;
        partial void OnIsCheckedChanged(bool value)
        {
            foreach (var elementType in ElementTypesBind)
            {
                elementType.IsChecked = value;
            }
        }

        public List<ElementTypeBind> ElementTypesBind{ get; set; } = new List<ElementTypeBind> { };
    }
}
