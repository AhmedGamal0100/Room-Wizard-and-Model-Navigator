using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Document = Autodesk.Revit.DB.Document;

namespace Solution4.Utiles
{
    public static class UtilesDocument
    {
        public static List<Element> GetElementsOfType(this Document document, Type type)
        {
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(document);
            return filteredElementCollector.OfClass(type).ToElements().ToList();
        }
    }
}
 