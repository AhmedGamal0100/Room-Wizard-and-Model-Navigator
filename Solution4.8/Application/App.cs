using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Solution4.Application
{
    internal class App : IExternalApplication
    {
        public string RibbonTabName { get => "NewRevitPlugin"; }
        public string RibbonPanelName { get => "RevitPanel"; }
        public string ModelNavigatorRibbonPanelName { get => "ModelNavigator"; }
        public string RoomWizardRibbonPanelName { get => "RoomWizard"; }

        public Result OnStartup(UIControlledApplication application)
        {
            #region Ribbon Tab
            // Create a Ribbon Tab:
            application.CreateRibbonTab(RibbonTabName);
            #endregion

            // Create a Ribbon Panel: 
            #region Ribbon Button

            BitmapImage bitmapImage = new BitmapImage(
                new Uri("pack://application:,,,/Solution4;component/Images/About.ico")
            );
            #endregion

            #region Model Navigator Command
            RibbonPanel modelNavigatorRibbonPanel = application.CreateRibbonPanel(RibbonTabName, ModelNavigatorRibbonPanelName);
            PushButtonData modelNavigatorPushButton = new PushButtonData("ModelNavigator", "Model Navigator", Assembly.GetExecutingAssembly().Location, "Solution4.Commands.ModelNavigatorCommand");
            modelNavigatorPushButton.LargeImage = bitmapImage;
            modelNavigatorPushButton.ToolTip = "Opens the Model Navigator tool.";
            modelNavigatorPushButton.LongDescription = "The Model Navigator helps you explore, filter, and manage model elements quickly.";
            modelNavigatorRibbonPanel.AddItem(modelNavigatorPushButton);
            #endregion

            #region Room Wizard
            RibbonPanel roomWizardRibbonPanel = application.CreateRibbonPanel(RibbonTabName, RoomWizardRibbonPanelName);
            PushButtonData roomWizardPushButton = new PushButtonData("RoomWizard", "RoomWizard", Assembly.GetExecutingAssembly().Location, "Solution4.Commands.RoomWizardCommand");
            roomWizardPushButton.LargeImage = bitmapImage;
            roomWizardPushButton.ToolTip = "Opens the Room Wizard tool.";
            roomWizardPushButton.LongDescription = "The Room Wizard helps you explore, display, and manage model elements quickly.";
            roomWizardRibbonPanel.AddItem(roomWizardPushButton);
            #endregion

            // To Load Material Design Package
            Assembly.LoadFrom(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\MaterialDesignThemes.Wpf.dll");
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
