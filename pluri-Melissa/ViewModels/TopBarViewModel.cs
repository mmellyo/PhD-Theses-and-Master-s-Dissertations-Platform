using Project.Commands;
using Project.Repos;
using Project.Stores;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Project.ViewModels
{
    public class TopBarViewModel : ViewModelBase
    {
        private int user_id;
        private readonly UserRepos userRepo = new UserRepos();
        private readonly NavigationStore navigationStore;

        private ICommand UploadCommand { get; }
        private ImageSource user_profilepic { get; set; }
        private string Username { get; set; }
        private string Email { get; set; }
        private string User_role { get; set; }
        public TopBarViewModel(int user_id, NavigationStore navigationStore)
        {
            this.user_id = user_id;
            this.navigationStore = navigationStore;
           
            UploadCommand = new NavigateCommand<UploadViewModel>(navigationStore, () => new UploadViewModel(navigationStore, user_id));

            Username = userRepo.GetuserName(user_id);
            user_profilepic = ByteArrayToImageConverter.LoadImageSourceFromBytes( userRepo.GetProfilepicFromId(user_id));
            Email = userRepo.GetuserEmail(user_id);
            User_role = userRepo.GetUserRole(user_id);
        }
    }
}
