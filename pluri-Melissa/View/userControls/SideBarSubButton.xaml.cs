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
    /// Interaction logic for SideBarSubButton.xaml
    /// </summary>
    public partial class SideBarSubButton : UserControl
    {
        public string content
        {
            get { return (string)GetValue(contentProperty); }
            set { SetValue(contentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty contentProperty =
            DependencyProperty.Register("content", typeof(string), typeof(SideBarSubButton), new PropertyMetadata("content"));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(SideBarSubButton), new PropertyMetadata(null));

        public ImageSource arrow
        {
            get { return (ImageSource)GetValue(arrowProperty); }
            set { SetValue(arrowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty arrowProperty =
            DependencyProperty.Register("arrow", typeof(ImageSource), typeof(SideBarSubButton), new PropertyMetadata(null));

        public SideBarSubButton()
        {
            InitializeComponent();
        }
    }
}
