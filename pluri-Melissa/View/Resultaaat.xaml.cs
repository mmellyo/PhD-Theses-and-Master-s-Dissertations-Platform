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
using Project.ViewModels;

namespace Project.View
{
    /// <summary>
    /// Interaction logic for Resultaaat.xaml
    /// </summary>
    public partial class Resultaaat : Window
    {
        public Resultaaat(string searchKey)
        {
            InitializeComponent();

            var viewModel = new ResultPageViewModel(searchKey);
            this.DataContext = viewModel;
        }
        public Resultaaat(ResultPageViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    
}
}
