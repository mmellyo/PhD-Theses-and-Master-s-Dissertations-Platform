using Project.Models;
using Project.View.userControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Project.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private readonly UserModel _user;

        public string name => _user.user_name;
        public ImageSource pfp => _user.pfp;

        public UserViewModel (UserModel user)
        {
            _user = user;
        }
    }
}
