using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Solution4.Models.RoomWizardCommandModels;
using Solution4.ViewModels;
using Solution4.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RApplication = Autodesk.Revit.ApplicationServices.Application;


namespace Solution4.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class RoomWizardCommand : IExternalCommand
    {
        public static UIApplication UIApplication { get; set; }
        public static RevitTask RevitTask { get; set; }
        public static View3D ActiveRoom { get; set; }
        public string activeRoomViewName { get => "3D - Room Wizard Preivew"; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Application & Document
            UIApplication uIApplication = commandData.Application;
            RApplication application = uIApplication.Application;
            UIDocument uIDocument = uIApplication.ActiveUIDocument;
            Document document = uIDocument.Document;

            // Create a 3DView
            using (Transaction transaction = new Transaction(document, "Create Room View"))
            {
                transaction.Start();
                ActiveRoom = new FilteredElementCollector(document).OfClass(typeof(View3D)).ToElements().Cast<View3D>().Where(v => !v.IsTemplate).FirstOrDefault(r => r.Name == activeRoomViewName);

                if(ActiveRoom == null)
                {

                    View3D view3D = new FilteredElementCollector(document).OfClass(typeof(View3D)).ToElements().Cast<View3D>().Where(v => !v.IsTemplate).FirstOrDefault(r => r.Name == "{3D}");
                    ActiveRoom = document.GetElement(view3D.Duplicate(ViewDuplicateOption.Duplicate)) as View3D;
                    ActiveRoom.Name = activeRoomViewName;

                }
                transaction.Commit();
            }

            List<Room> rooms = new FilteredElementCollector(document).OfClass(typeof(SpatialElement)).ToElements().Cast<Room>().Where(r => r.Area != 0).ToList();

            // Intiate RevitTask
            UIApplication = uIApplication;
            RevitTask = new RevitTask();

            // Intiate the Room Wizard ViewModel
            RoomWizardViewModel roomWizardViewModel = new RoomWizardViewModel();

            // Add Data In the ViewModel
            foreach (var room in rooms)
            {
                RoomBinding roomBinding = new RoomBinding()
                {
                    Room = room,
                    Number = room.Number,
                    Name = room.Name,
                    Area = Math.Round(room.Area, 2),
                    Volume = Math.Round(room.Volume, 2),
                    FloorThickness = 0,
                };

                new FilteredElementCollector(document).OfClass(typeof(Material)).ToElements().Cast<Material>().OrderBy(m => m.Name).ToList().ForEach(m => roomBinding.DocumentMaterials.Add(m)); 

                roomWizardViewModel.RoomBindings.Add(roomBinding);
            }

            // Show View
            RoomWizardView roomWizardView = new RoomWizardView();
            roomWizardView.DataContext = roomWizardViewModel;
            roomWizardView.Show();

            return Result.Succeeded;
        }
    }
}
