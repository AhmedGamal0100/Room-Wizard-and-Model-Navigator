using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Document = Autodesk.Revit.DB.Document;
using Point = Autodesk.Revit.DB.Point;

namespace Solution4.Utiles
{
    public static class UtilesGeometry
    {
        // Method To Covert XYZ to Point
        public static Point ToPoint(this XYZ xyz)
        {
            return Point.Create(xyz);
        } 

        public static void ShowGeometry(this List<GeometryObject> geometryObjects, Document document , bool withTransaction = true)
        {
            if (withTransaction)
            {
                using (Transaction transaction = new Transaction(document, "Create a Point Geometry"))
                {
                    transaction.Start();
                    DirectShape directShape = DirectShape.CreateElement(document, new ElementId(BuiltInCategory.OST_GenericModel));
                    directShape.SetShape(geometryObjects.Where(s => s != null).ToList());
                    transaction.Commit();
                }
            } else
            {
                DirectShape directShape = DirectShape.CreateElement(document, new ElementId(BuiltInCategory.OST_GenericModel));
                directShape.SetShape(geometryObjects);
            }
        }

        // Create 1 Solid For each Element
        public static Solid GetGeometry(this Element element)
        {
            Options options = new Options();
            options.ComputeReferences = true;
            options.View = element.Document.ActiveView;
            GeometryElement geometryElement = element.get_Geometry(options);
            List<Solid> solids1 = geometryElement.OfType<Solid>().ToList(); // 1 Solid
            List<Solid> solids2 = geometryElement.OfType<GeometryInstance>().Select(g => g.GetInstanceGeometry()).SelectMany(g => g.OfType<Solid>()).ToList(); // Multible of Solids
            List<Solid> solids = solids1.Concat(solids2).Where(s => s.Volume != 0).ToList();
            Solid combinedSolid = null;
            foreach (Solid solid in solids)
            {
                if (combinedSolid == null)
                {
                    combinedSolid = solid;
                }
                else
                {
                    try
                    {
                        combinedSolid = BooleanOperationsUtils.ExecuteBooleanOperation(combinedSolid, solid, BooleanOperationsType.Union);
                    } catch { }
                }
            }
            return combinedSolid;
        }
    }
}
