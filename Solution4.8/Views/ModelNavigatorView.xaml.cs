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
    /// Interaction logic for ModelNavigatorView.xaml
    /// </summary>
    public partial class ModelNavigatorView : Window
    {
        // Static Instance to be used in the ViewModel to Close the Window
        public static ModelNavigatorView Current { get; private set; }
        public ModelNavigatorView()
        {
            InitializeComponent();
            Current = this;

        }
    }
}
