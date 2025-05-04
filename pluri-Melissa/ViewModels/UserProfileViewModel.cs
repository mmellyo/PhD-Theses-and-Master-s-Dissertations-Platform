using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Project.Models;
using Project.Repos;
using Project.Services;

namespace Project.ViewModels
{
    public class UserProfileViewModel : ViewModelBase
    {
        private readonly IUserRepos _userRepos;
        private readonly UserModel _usermodel;
        private readonly IUserSessionService _userSession;

        private readonly IWindowManager _windowManager;
        private readonly ViewModelLocator _viewModelLocator;

        public string user_name { get; set; }
        public string user_email { get; set; }
        public int user_id { get; set; }

        private byte[] _userProfilePic;
        public byte[] user_profilepic
        {
            get => _userProfilePic;
            set
            {
                _userProfilePic = value;
                OnPropertyChanged(nameof(user_profilepic)); // Notifies the UI to refresh
            }
        }

        //commands
        public ICommand GoToTheseCommand { get; }


        //constructor
        public UserProfileViewModel( IUserSessionService userSession, IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {
            Console.WriteLine("***************************** UserProfileViewModel *******************");
            Console.WriteLine("UserId in userprofile construt :"+ user_id);

            _windowManager = windowManager;
            _viewModelLocator = viewModelLocator;
            _userSession = userSession;
            _userRepos = new UserRepos();
            _usermodel = new UserModel();

            // COMMAND CHANGE window
            GoToTheseCommand = new ViewModelCommand(
            execute: obj =>
            {
                _windowManager.CloseWindow();
                _windowManager.ShowWindow(_viewModelLocator.CommentViewModel);

            }

            );

        }




        public void InitializeWithUserId(int userId)
        {
            user_id = userId;
            _userSession.UserId = userId;
            user_profilepic = _userRepos.GetProfilepicFromId(user_id);
            Console.WriteLine("***************************** InitializeWithUserId *******************");
            Console.WriteLine("UserId that is recieving from usert SERVICE to USERPROVEIL is: " + user_id);

        }

    }
}
