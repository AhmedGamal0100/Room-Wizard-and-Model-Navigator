using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Solution4.Models.ModelNavigatorCommandModels;
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
    public class ModelNavigatorCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Application & Document
            UIApplication uIApplication = commandData.Application;
            RApplication application = uIApplication.Application;
            UIDocument uIDocument = uIApplication.ActiveUIDocument;
            Document document = uIDocument.Document;

            // Get All Document Elements
            List<Element> documentElements = new FilteredElementCollector(document).WhereElementIsNotElementType().ToElements().Where(e => e.Category != null).Where(e => e.Category.CategoryType == CategoryType.Model).Where(e => document.GetElement(e.GetTypeId()) != null).ToList();

            // Get Unique Categories for these Elements:
            List<Category> documentCategories = documentElements.Select(e => e.Category).GroupBy(e => e.Name).Select(g => g.FirstOrDefault()).OrderBy(e => e.Name).ToList();

            // Create RevitTask to be able to use Show() with Transaction in the ViewModel it Asynchronously:
            RevitTask revitTask = new RevitTask();

            // ------------------------------------------------------- 1. Create the ViewModel
            ModelNavigatorViewModel modelNavigatorViewModel = new ModelNavigatorViewModel(document, uIDocument, revitTask);

            // ------------------------------------------------------- 2. Fill the ViewModel with Data
            foreach (var category in documentCategories)
            {
                CategoryBind categoryBind = new CategoryBind()
                {
                    Name = category.Name,
                    IsChecked = false
                };
                List<Element> categoryElements = documentElements.Where(e => e.Category.Id == category.Id).ToList();
                List<ElementType> elementTypes = categoryElements.Select(e => document.GetElement(e.GetTypeId()) as ElementType).GroupBy(e => e.Name).Select(g => g.FirstOrDefault()).OrderBy(e => e.Name).ToList();

                foreach (var elementType in elementTypes)
                {
                    ElementTypeBind elementTypeBind = new ElementTypeBind()
                    {
                        Name = elementType.Name,
                        IsChecked = false
                    };
                    List<Element> elementTypeElements = categoryElements.Where(e => e.GetTypeId() == elementType.Id).ToList();
                    elementTypeBind.Elements = elementTypeElements;

                    categoryBind.ElementTypesBind.Add(elementTypeBind);
                }
                modelNavigatorViewModel.CategoriesBind.Add(categoryBind);
            }

            // ------------------------------------------------------- 3. Create the View and Connect it to the ViewModel
            ModelNavigatorView modelNavigatorView = new ModelNavigatorView();
            modelNavigatorView.DataContext = modelNavigatorViewModel;

            modelNavigatorView.Show();

            return Result.Succeeded;
        }
    }
}
