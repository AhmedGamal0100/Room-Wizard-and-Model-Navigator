using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution4.Utiles
{
    public class UtilesSelectionFilters : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            // To Not return any Error if the Category is not exist
            if (elem.Category == null) return false;

            // Allows Walls to be Selected
            if (elem is Wall)
                return true;

            // Allows Doors to be Selected
            if (elem is FamilyInstance)
            {
                if ( elem.Category.Name == "Doors")
                {
                    return true;
                }
            }

            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            throw new NotImplementedException();
        }
    }
}
