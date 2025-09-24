using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Solution4.Models.ModelNavigatorCommandModels;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Solution4.Views;
using System.Diagnostics;

// NOTE: This Color Class the returns from WPF Color Picker is different than the Revit.DB Color Class, SO we need to use an Alias for one of them
using MediaColor = System.Windows.Media.Color;

namespace Solution4.ViewModels
{
    public partial class ModelNavigatorViewModel : ObservableObject
    {
        public Document Document { get; set; }
        public UIDocument UIDocument { get; set; }
        public RevitTask RevitTask{ get; set; }


        #region Properties
        [ObservableProperty]
        ObservableCollection<CategoryBind> categoriesBind = new ObservableCollection<CategoryBind>();

        [ObservableProperty]
        ObservableCollection<ElementTypeBind> elementTypesBind = new ObservableCollection<ElementTypeBind>();

        [ObservableProperty]
        CategoryBind selectedCategoryBind;
        partial void OnSelectedCategoryBindChanged(CategoryBind value)
        {
            ElementTypesBind = new ObservableCollection<ElementTypeBind>();
            foreach (var elementType in value.ElementTypesBind)
            {
                ElementTypesBind.Add(elementType);
            }
        }

        [ObservableProperty]
        bool isAllCateogriesChecked;
        partial void OnIsAllCateogriesCheckedChanged(bool value)
        {
            foreach (var category in CategoriesBind)
            {
                category.IsChecked = value;
            }
        }

        [ObservableProperty]
        MediaColor color;

        #endregion

        #region Relay Commands
        [RelayCommand]
        void Select()
        {
            // Close the Window (Check the Code Behind) In case used ShowDialog() not Show()
            //ModelNavigatorView.Current.Close();

            List<Element> selectedElements = CategoriesBind.ToList().SelectMany(c => c.ElementTypesBind).Where(et => et.IsChecked).SelectMany(et => et.Elements).ToList();
            UIDocument.Selection.SetElementIds(selectedElements.Select(se => se.Id).ToList());
        }

        [RelayCommand]
        // NOTE: RevitTask if not used Async & Await it will not work properly and will run what is inside it before the Transaction is Committed, so we need to use Async & Await
        async Task Isolate()
        {
            //ModelNavigatorView.Current.Close();
            await RevitTask.Run((uiapp) => 
            {
                using (Transaction transaction = new Transaction(Document, "Isolate From Model Navigator"))
                {
                    transaction.Start();
                    List<Element> selectedElements = CategoriesBind.ToList().SelectMany(c => c.ElementTypesBind).Where(et => et.IsChecked).SelectMany(et => et.Elements).ToList();
                    UIDocument.ActiveView.IsolateElementsTemporary(selectedElements.Select(se => se.Id).ToList());
                    transaction.Commit();
                }
            });
        }

        [RelayCommand]
         async Task Hide()
        {
            //ModelNavigatorView.Current.Close();
            await RevitTask.Run((uiapp) =>
            {
                using (Transaction transaction = new Transaction(Document, "Hide From Model Navigator"))
                {
                    transaction.Start();
                    List<Element> selectedElements = CategoriesBind.ToList().SelectMany(c => c.ElementTypesBind).Where(et => et.IsChecked).SelectMany(et => et.Elements).ToList();
                    UIDocument.ActiveView.HideElementsTemporary(selectedElements.Select(se => se.Id).ToList());
                    transaction.Commit();
                }
            });
        }

        [RelayCommand]
         async Task Delete()
        {
            //ModelNavigatorView.Current.Close();
             await RevitTask.Run((uiapp) =>
            {
                using (Transaction transaction = new Transaction(Document, "Delete From Model Navigator"))
            {
                transaction.Start();
                List<Element> selectedElements = CategoriesBind.ToList().SelectMany(c => c.ElementTypesBind).Where(et => et.IsChecked).SelectMany(et => et.Elements).ToList();
                foreach (var item in selectedElements.Select(e => e.Id).ToList())
                {
                    try
                    {
                        Document.Delete(item);
                    }
                    catch { }
                }
                transaction.Commit();
            }
            });
        }

        [RelayCommand]
        async Task Override()
        {
            //ModelNavigatorView.Current.Close();
            await RevitTask.Run((uiapp) =>
            {
                using (Transaction transaction = new Transaction(Document, "Delete From Model Navigator"))
                {
                    transaction.Start();
                    // 1. Have the element
                    List<Element> selectedElements = CategoriesBind.ToList().SelectMany(c => c.ElementTypesBind).Where(et => et.IsChecked).SelectMany(et => et.Elements).ToList();
                    // 2. Get the ID of the needed param we want to override it
                    ElementId solidPatternId = new FilteredElementCollector(Document).OfClass(typeof(FillPatternElement)).FirstOrDefault(e => e.Name == "<Solid fill>").Id;
                    foreach (var item in selectedElements.Select(e => e.Id).ToList())
                    {

                        // This is the Override Graphic settings for the item:
                        OverrideGraphicSettings overrideGraphicSettings = UIDocument.ActiveView.GetElementOverrides(item);
                        overrideGraphicSettings.SetSurfaceBackgroundPatternId(solidPatternId);
                        overrideGraphicSettings.SetSurfaceForegroundPatternId(solidPatternId);
                        overrideGraphicSettings.SetCutBackgroundPatternId(solidPatternId);
                        overrideGraphicSettings.SetCutForegroundPatternId(solidPatternId);

                        // If I want to set the Color Manual:
                        //overrideGraphicSettings.SetSurfaceBackgroundPatternColor(new Color(20, 30, 40));
                        //overrideGraphicSettings.SetSurfaceForegroundPatternColor(new Color(20, 30, 40));
                        //overrideGraphicSettings.SetCutBackgroundPatternColor(new Color(20, 30, 40));
                        //overrideGraphicSettings.SetCutForegroundPatternColor(new Color(20, 30, 40));

                        // OR Override IT:
                        // NOTE: SetSurfaceBackgroundPatternColor() Require a Revit.DB Color Class not the WPF one, SO we need to convert it first
                        overrideGraphicSettings.SetSurfaceBackgroundPatternColor(new Color(Color.R, Color.G, Color.B));
                        overrideGraphicSettings.SetSurfaceForegroundPatternColor(new Color(Color.R, Color.G, Color.B));
                        overrideGraphicSettings.SetCutBackgroundPatternColor(new Color(Color.R, Color.G, Color.B));
                        overrideGraphicSettings.SetCutForegroundPatternColor(new Color(Color.R, Color.G, Color.B));
                        overrideGraphicSettings.SetProjectionLineColor(new Color(Color.R, Color.G, Color.B));
                        overrideGraphicSettings.SetCutLineColor(new Color(Color.R, Color.G, Color.B));

                        // Last Thing is to set these Overrides to The Element using SetElementOverrides()
                        Document.ActiveView.SetElementOverrides(item, overrideGraphicSettings);
                    }
                    transaction.Commit();

                }
            });
        }

        #endregion

        // This Constructor is to get the Document data & UIDocument Data from the Command
        public ModelNavigatorViewModel(Document document, UIDocument uIDocument, RevitTask revitTask)
        {
            Document = document;
            UIDocument = uIDocument;
            RevitTask = revitTask;
        }
    }
}
