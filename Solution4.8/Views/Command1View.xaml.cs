using Solution4.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Solution4.Views
{
    /// <summary>
    /// Interaction logic for Command1View.xaml
    /// </summary>
    public partial class Command1View : Window
    {
        // GENERAL NOTE: "This" represents Command1View itself & becase the Command1View iherits from
        // Window so we now have the Window methods and params such as Close() 

        // This Constructor will work once the WPF starts
        public Command1View()
        {
            InitializeComponent();
            DataContext = new Command1ViewModel();
        }

        // When click the Exit button close the whole plugin view
        //private void Button_Click_Exit(object sender, RoutedEventArgs e)
        //{
        //    this.Close(); or Close(); --> The same
        //}

        // This is The EventHandler for the SizeChange:
        //private void mainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    MessageBox.Show("XXXXX");
        //}
    }
}
