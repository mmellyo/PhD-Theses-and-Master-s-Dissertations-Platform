
using System;
using System.Windows;
using System.Windows.Input;
using Mysqlx.Prepare;
using Project.Models;
using Project.Repos;
using Project.Services;


namespace Project.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {

        // fields 
        private string _username;
        private string _signUpPassword;
        private string _errorMessage;
        private string _email;
        private UserModel _userModel;
        private bool _isViewVisible = true;
        private IUserRepos _userRepos;
        private EmailVerificationRepo _emailVerificationRepo;
        private readonly IUserSessionService _userSession;
        private readonly IWindowManager _windowManager;
        private readonly ViewModelLocator _viewModelLocator;




        //setters/getters
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public byte[] user_profilepic { get; set; }




        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsViewVisible
        {
            get => _isViewVisible;
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }




        // COMMANDS
        public ICommand SignUpCommand { get; }
        public ICommand GoHomeCommand { get; }
        public ICommand GologinCommand { get; }





        // Constructor
        public SignUpViewModel(IUserSessionService userSession, IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {

            //fields
            _userSession = userSession;
            _windowManager = windowManager;
            _viewModelLocator = viewModelLocator;
            _userModel = new UserModel();
            _userRepos = new UserRepos();
            _emailVerificationRepo = new EmailVerificationRepo();


            //******************************************** COMMAND SIGN UP
            SignUpCommand = new ViewModelCommand(
            execute: obj =>
            {
                
                if (Password != ConfirmPassword)
                {
                    MessageBox.Show("Passwords do not match! Please re-enter to confirm.", "Passwords do not match", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var repo = new UserRepos();
                var user = new UserModel
                {
                    user_email = this.Email,
                    user_name = _userRepos.GetUsernameFromEmail(this.Email),
                    user_password = this.Password,
                    user_role = _userRepos.AssignUserRole(this.Email),
                };

                bool success = repo.SignUp(user);
                MessageBox.Show(success ? "User registered!" : "Registration failed.");


                //Assigns EMAIL
                _viewModelLocator.MyProfileViewModel.Email = Email;
                _viewModelLocator.CommentViewModel.Email = Email;

                //Assigns pdp
                //_viewModelLocator.MyProfileViewModel.user_profilepic = _userRepos.GetProfilepicFromEmail(Email);
               // _viewModelLocator.CommentViewModel.user_profilepic = _userRepos.GetProfilepicFromEmail(Email);


                _windowManager.CloseWindow();
                _windowManager.ShowWindow(_viewModelLocator.MyProfileViewModel);
            },
            canExecute: obj => true
        );



            //******************************************** COMMAND GO HOME
            GoHomeCommand = new ViewModelCommand(
            execute: obj =>
            {
                _windowManager.CloseWindow();
                _windowManager.ShowWindow(_viewModelLocator.WelcomeViewModel);
            },
            canExecute: obj => true
        );



            //******************************************** COMMAND GO LOGIN
            GologinCommand = new ViewModelCommand(
                execute: obj =>
                {
                    _windowManager.CloseWindow();
                    // Switch the window to welcoùme
                    _windowManager.ShowWindow(_viewModelLocator.WelcomeViewModel);
                },
                canExecute: obj => true
                );
        }

    }
}