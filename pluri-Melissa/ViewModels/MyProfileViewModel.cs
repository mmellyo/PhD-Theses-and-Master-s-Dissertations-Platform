using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Models;
using Project.Repos;
using Project.Services;
using System.Windows.Input;
using System.Windows;
using System.IO;
using Org.BouncyCastle.Bcpg;

namespace Project.ViewModels
{
    public class MyProfileViewModel : ViewModelBase
    {
        // fields 
        private string _email;
        private bool _isViewVisible;

        private readonly IUserRepos _userRepos;
        private readonly UserModel _usermodel;
        private readonly IUserSessionService _userSession;

        private readonly IWindowManager _windowManager;
        private readonly ViewModelLocator _viewModelLocator;


        // getters / setters
        public string Email { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string User_role { get; set; }
        public bool IsViewVisible
        {
            get => _isViewVisible;
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

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

        // commands
        public ICommand ChangeProfilepicCommand { get; }
        public ICommand GoToTheseCommand { get; }



        // constructor
        public MyProfileViewModel(IUserSessionService userSession, IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {

            //fields
            _windowManager = windowManager;
            _viewModelLocator = viewModelLocator;
            _userSession = userSession;
            _userRepos = new UserRepos();
            _usermodel = new UserModel();


            // COMMAND CHANGE PDP
            ChangeProfilepicCommand = new ViewModelCommand(
            execute: obj =>
            {
                int UserId = _userRepos.GetUserId(Email);//not null

                Console.WriteLine("inside UserId: " + UserId); //null
                Console.WriteLine("inside email: " + Email); // null 


                //ask to upload a pic
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg",
                    Title = "Select Profile Picture"
                };

                if (dialog.ShowDialog() == true)
                {
                    // convert into binary array
                    byte[] imageBytes = File.ReadAllBytes(dialog.FileName);

                    bool success = _userRepos.ChangeProfilePic(UserId, profilepic: imageBytes);

                    if (success)
                    {
                        MessageBox.Show("Profile picture updated!");
                        this.user_profilepic = imageBytes;
                        //Assigns user_profilepic 
                        _viewModelLocator.CommentViewModel.user_profilepic = imageBytes;
                    }

                    else
                    { MessageBox.Show("Failed to update profile picture."); }


                }
            }
            );






            // temp COMMAND CHANGE window
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
            UserId = userId;
            _userSession.UserId = userId;
            user_profilepic = _userRepos.GetProfilepicFromId(UserId);
            Email = _userRepos.GetuserEmail(UserId);
            User_role = _userRepos.AssignUserRole(Email);
            Username = _userRepos.GetUsernameFromEmail(Email);

            Console.WriteLine("***************************** InitializeWithUserId *******************");
            Console.WriteLine("UserId that is recieving from login to MYPROFILE is: " + UserId);
            Console.WriteLine("my email " + Email);
            Console.WriteLine("my role " + User_role);
            Console.WriteLine("my username " + Username);

        }

    }
}
