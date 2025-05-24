using Project.Stores;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Interaction logic for profileSide.xaml
    /// </summary>
    public partial class SideBar_user : UserControl
    {
        public SideBar_user()
        {
            InitializeComponent();

        }

        public void SetViewModel (int user_id, NavigationStore navigationStore)
        {
            this.DataContext = new UserSideBarViewModel(user_id, navigationStore);
        }
    }
}
