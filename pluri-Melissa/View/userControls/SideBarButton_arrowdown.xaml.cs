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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project.View.userControls
{
    /// <summary>
    /// Interaction logic for SideBarButton.xaml
    /// </summary>
    public partial class SideBarButton_arrowdown : UserControl
    {


        public string content
        {
            get { return (string)GetValue(contentProperty); }
            set { SetValue(contentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty contentProperty =
            DependencyProperty.Register("content", typeof(string), typeof(SideBarButton_arrowdown), new PropertyMetadata("content"));



        public Geometry icon
        {
            get { return (Geometry)GetValue(iconProperty); }
            set { SetValue(iconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty iconProperty =
            DependencyProperty.Register("icon", typeof(Geometry), typeof(SideBarButton_arrowdown), new PropertyMetadata(null));



        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(SideBarButton_arrowdown), new PropertyMetadata(null));



        public SideBarButton_arrowdown()
        {
            InitializeComponent();
        }
    }
}
