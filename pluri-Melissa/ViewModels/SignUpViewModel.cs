
using System;
using System.Windows;
using System.Windows.Input;
using Project.Commands;
using Project.Models;
using Project.Repos;
using Project.Services;
using Project.Stores;


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
        



        //setters/getters
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }




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
        //public ICommand AddItemCommand { get; set; }
        //public ICommand SignUpCommand { get; set; }

    
        public ICommand GoLoginCommand { get; }
        public ICommand GoHomeCommand { get; }

        public SignUpViewModel(NavigationStore navigationStore)
        {
            GoHomeCommand = new NavigateCommand<WelcomeViewModel>(navigationStore, ()=> new WelcomeViewModel(navigationStore));
            GoLoginCommand = new NavigateCommand<LoginViewModel>(navigationStore, ()=> new LoginViewModel(navigationStore));

            SignUpCommand = new SignUpCommand(this, navigationStore);

        }



        // Constructor
        /* public SignUpViewModel(IUserSessionService userSession, IWindowManager windowManager, ViewModelLocator viewModelLocator)
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
                     user_password = this.Password,
                     user_role = "user" //later :P
                 };
                 _userSession.Email = this.Email;
                 //ErrorMessage = $"saved: mail='{user.user_email}', pw='{user.user_password}', role='{repo.AssignUserRole(user.user_email)}'";

                 bool success = repo.SignUp(user);
                 MessageBox.Show(success ? "User registered!" : "Registration failed.");

                 //if (success) { _userSession.Email = this.Email; }


                 _windowManager.CloseWindow();

                 // Switch the window to SignUpViewModel
                 _windowManager.ShowWindow(_viewModelLocator.CommentViewModel);
             },
             canExecute: obj => true
         );


             // Load the email from UserModel - make sure to initialize this first
             //  LoadUserData();

             // SignUpCommand = new ViewModelCommand(ExecuteSignUpCommand, CanExecuteSignUpCommand);

             // Debug - check if email was loaded
             // Console.WriteLine($"SignUpViewModel initialized with email: {SignUpEmail}");
         }*/


    }
}