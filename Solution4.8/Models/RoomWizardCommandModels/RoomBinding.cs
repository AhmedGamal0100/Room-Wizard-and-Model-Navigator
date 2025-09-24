using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Solution4.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution4.Models.RoomWizardCommandModels
{
    public partial class RoomBinding : ObservableObject
    {
        public Room Room{ get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public double Area { get; set; }
        public double Volume { get; set; }

        [ObservableProperty]
        double floorThickness;

        [ObservableProperty]
        Material floorMaterial;
        //partial void OnFloorMaterialChanged(Material value)
        //{
        //    // Create the Floor Directly when we change the Material
        //    if (UIFrameworkServices.QuickAccessToolBarService.collectUndoRedoItems(true).ToList().Contains("Floor Generating"))
        //    {
        //        UIFrameworkServices.QuickAccessToolBarService.performMultipleUndoRedoOperations(true, 1);
        //    }

        //    UIDocument uiDocument = RoomWizardCommand.UIApplication.ActiveUIDocument;
        //    Document document = uiDocument.Document;
        //    RevitTask revitTask = RoomWizardCommand.RevitTask;
        //    View3D activeRoomView = RoomWizardCommand.ActiveRoom;

        //    revitTask.Run(uiapp =>
        //    {
        //        using (Transaction transaction = new Transaction(document, "Floor Generating"))
        //        {
        //            transaction.Start();
        //            if (FloorThickness != 0 & FloorMaterial != null)
        //            {
        //                CurveArray curveArray = new CurveArray();

        //                SpatialElementBoundaryOptions spatialElementBoundaryOptions = new SpatialElementBoundaryOptions();
        //                spatialElementBoundaryOptions.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;
        //                spatialElementBoundaryOptions.StoreFreeBoundaryFaces = true; 

        //                List<List<Curve>> roomCurves = Room.GetBoundarySegments(spatialElementBoundaryOptions).Select(l => l.Select(r => r.GetCurve()).ToList()).ToList();
        //                foreach (List<Curve> curveList in roomCurves)
        //                {
        //                    foreach (var curve in curveList)
        //                    {
        //                        curveArray.Append(curve);
        //                    }
        //                }

        //                FilteredElementCollector collector = new FilteredElementCollector(document);
        //                string floorTypeName = $"{FloorMaterial.Name} - {FloorThickness} Inch";

        //                FloorType floor = collector.OfClass(typeof(FloorType)).Cast<FloorType>().FirstOrDefault(f => f.Name == floorTypeName);
        //                if (floor == null)
        //                {
        //                    floor = collector.OfClass(typeof(FloorType)).Cast<FloorType>().FirstOrDefault().Duplicate(floorTypeName) as FloorType;
        //                }
        //                CompoundStructure compoundStructure = floor.GetCompoundStructure();
        //                CompoundStructureLayer compoundStructureLayer = compoundStructure.GetLayers().FirstOrDefault();
        //                compoundStructureLayer.MaterialId = FloorMaterial.Id;
        //                compoundStructureLayer.Width = FloorThickness / 12; 
        //                compoundStructure.SetLayer(0, compoundStructureLayer);
        //                floor.SetCompoundStructure(compoundStructure);

        //                Floor createdFloor = document.Create.NewFloor(curveArray, floor, Room.Level, true);
        //                createdFloor.Parameters.Cast<Parameter>().FirstOrDefault(p => (p.Definition as InternalDefinition).BuiltInParameter == BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).Set(Room.BaseOffset + (FloorThickness) / 12);
        //            }
        //            transaction.Commit();
        //        }
        //    });
        //}

        [ObservableProperty]
        ObservableCollection<Material> documentMaterials = new ObservableCollection<Material>();

        [RelayCommand]
        async Task Show()
        {

            // 1. Prepare the Document Data
            UIDocument uiDocument = RoomWizardCommand.UIApplication.ActiveUIDocument;
            Document document =  uiDocument.Document;
            RevitTask revitTask = RoomWizardCommand.RevitTask;
            View3D activeRoomView = RoomWizardCommand.ActiveRoom;

            await revitTask.Run(uiapp =>
            {
                using (Transaction transaction = new Transaction(document, "Room Section Box"))
                {
                    transaction.Start();

                    #region Create a Section Box
                    BoundingBoxXYZ boundingBoxXYZ = Room.get_BoundingBox(activeRoomView);

                    // 1. Create an Offset: NOTE: Revit API UNITS always Feet, foot square, cublic feet
                    double offset = 1;
                    double reducedHeight = (boundingBoxXYZ.Max.Z- boundingBoxXYZ.Min.Z)/9;

                    // 2. Add this offset on the Max and Subtract it from Min
                    boundingBoxXYZ.Max = new XYZ(boundingBoxXYZ.Max.X + offset, boundingBoxXYZ.Max.Y + offset, boundingBoxXYZ.Max.Z - reducedHeight);
                    boundingBoxXYZ.Min = new XYZ(boundingBoxXYZ.Min.X - offset, boundingBoxXYZ.Min.Y - offset, boundingBoxXYZ.Min.Z - offset);
                    activeRoomView.SetSectionBox(boundingBoxXYZ);
                    #endregion

                    transaction.Commit();
                }
            });
            var undoItems = UIFrameworkServices.QuickAccessToolBarService.collectUndoRedoItems(true).ToList();

            if (undoItems.Any() && undoItems.First() == "Floor Generating")
            {
                UIFrameworkServices.QuickAccessToolBarService.performMultipleUndoRedoOperations(true, 1);
            }
            await revitTask.Run(uiapp =>
            {
                using (Transaction transaction = new Transaction(document, "Floor Generating"))
                {
                    transaction.Start();

                    #region Create The Floor on the Room Boundires
                    if (FloorThickness != 0 & FloorMaterial != null)
                    {
                        CurveArray curveArray = new CurveArray();

                        // Define the Boundry Options
                        SpatialElementBoundaryOptions spatialElementBoundaryOptions = new SpatialElementBoundaryOptions();
                        spatialElementBoundaryOptions.SpatialElementBoundaryLocation = SpatialElementBoundaryLocation.Finish;
                        spatialElementBoundaryOptions.StoreFreeBoundaryFaces = true;

                        List<List<Curve>> roomCurves = Room.GetBoundarySegments(spatialElementBoundaryOptions).Select(l => l.Select(r => r.GetCurve()).ToList()).ToList();
                        foreach (List<Curve> curveList in roomCurves)
                        {
                            foreach (var curve in curveList)
                            {
                                curveArray.Append(curve);
                            }
                        }

                        // Define the Floor Name,
                        FilteredElementCollector collector = new FilteredElementCollector(document);
                        string floorTypeName = $"{FloorMaterial.Name} - {FloorThickness} Inch";

                        // Define create a dublicate of this material family,
                        FloorType floor = collector.OfClass(typeof(FloorType)).Cast<FloorType>().FirstOrDefault(f => f.Name == floorTypeName);
                        if (floor == null)
                        {
                            floor = collector.OfClass(typeof(FloorType)).Cast<FloorType>().FirstOrDefault().Duplicate(floorTypeName) as FloorType;
                        }

                        // Assign its material Parameter
                        CompoundStructure compoundStructure = floor.GetCompoundStructure();
                        CompoundStructureLayer compoundStructureLayer = compoundStructure.GetLayers().FirstOrDefault();
                        compoundStructureLayer.MaterialId = FloorMaterial.Id;
                        compoundStructureLayer.Width = FloorThickness / 12;
                        compoundStructure.SetLayer(0, compoundStructureLayer);
                        floor.SetCompoundStructure(compoundStructure);

                        Floor createdFloor = document.Create.NewFloor(curveArray, floor, Room.Level, true);

                        createdFloor.Parameters.Cast<Parameter>().FirstOrDefault(p => (p.Definition as InternalDefinition).BuiltInParameter == BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).Set(Room.BaseOffset + (FloorThickness) / 12);
                    }
                    #endregion

                    transaction.Commit();
                }
            });
            // 3. Active This View
            uiDocument.ActiveView = activeRoomView;
        }
    }
}
