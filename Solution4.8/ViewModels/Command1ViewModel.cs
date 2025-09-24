using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Solution4.ViewModels
{
    public partial class Command1ViewModel : ObservableObject
    {
        #region Professional Way
        [ObservableProperty]
        string name = "Hello";
        partial void OnNameChanged(string oldValue, string newValue)
        {
            MessageBox.Show(oldValue + " Changed to be " +  newValue);
        }

        [ObservableProperty]
        int fontSize = 20;

        partial void OnFontSizeChanged(int oldValue, int newValue)
        {
            MessageBox.Show(oldValue + " Changed to be " + newValue);
        }

        [RelayCommand]
        void Change()
        {
            Name = "Professional Way";
            FontSize = 8;
        }

        [ObservableProperty]
        string textBlock = "Empty!";

        [ObservableProperty]
        string textBox;

        [ObservableProperty]
        string checkBoxContent = "Unchecked";

        [ObservableProperty]
        bool checkBoxChecked;
        partial void OnCheckBoxCheckedChanged(bool value)
        {
            CheckBoxContent = value ? "Checked" : "Unchecked";
            Debug.WriteLine(value);
        }

        [ObservableProperty]
        // NOTE: In ObservableObject, When we use .Add() inside this Property in memory the addition will happen but in the View nothing will be rendered
        //List<string> listBoxData = new List<string>();

        // SOLUTION to use class ObservableCollection, this works better and will be rendered directly with any list & CRUD Operation
        ObservableCollection<string> listBoxData = new ObservableCollection<string>();


        [RelayCommand]
        void Run()
        {
            //if (CheckBoxChecked)
            //{
            //    CheckBoxContent = "Checked";
            //} else
            //{
            //    CheckBoxContent = "Unchecked";
            //}

            //CheckBoxContent = CheckBoxChecked ? "Checked" : "Unchecked";

            // Intiate an empty ListBox of Items

            if (TextBox != null & TextBox != "")
            {
                ListBoxData.Add(TextBox);
                TextBlock = TextBox;
                TextBox = "";
            }
        }

        #endregion

        #region Ordinary Way
        //protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == "Name")
        //    {
        //        // Do Something in my data or Revit
        //    }
        //}

        //#region Properties
        //private string _name = "Initial MVVM Value";
        //public string Name
        //{
        //    get { return _name; }
        //    set { SetProperty(ref _name, value); }
        //}

        //private int _fontSize = 20;
        //public int FontSize
        //{
        //    get { return _fontSize; }
        //    set { SetProperty(ref _fontSize, value); }
        //}
        //#endregion

        //#region Commands
        //public ICommand ChangeCommand { get; set; }
        //private void Change()
        //{
        //    Name = "Intial Value for Button Commands";
        //    FontSize = 10;
        //}
        //#endregion

        //public Command1ViewModel()
        //{
        //    this.ChangeCommand = new RelayCommand(Change);
        //}
        #endregion
    }
}
