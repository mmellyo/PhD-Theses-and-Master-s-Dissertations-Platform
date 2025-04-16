
using System;
using System.Windows;
using System.Windows.Input;
using Project.Models;
using Project.Repos;
using Project.Services;

namespace Project.ViewModels
{
    public class SignUpViewModel : ViewModelBase
    {
        private string _username;
        private string _signUpPassword;
        private string _errorMessage;
        private string _email;
        private UserModel _userModel;
        private bool _isViewVisible = true;
        private IUserRepos _userRepos;



        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }


        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }





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

        //  public IItemService ItemService { get; set; }
        //public ICommand AddItemCommand { get; set; }
        //public ICommand SignUpCommand { get; set; }




        // Constructor
        public SignUpViewModel()
        {
            // ItemService = itemService;


            //test commands
            // AddItemCommand = new ViewModelCommand(
            //execute: _ => { ItemService.AddItem(); },
            // canExecute: _ => true

            // );

            _userModel = new UserModel();
            _userRepos = new UserRepos();



            //commands
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

                //ErrorMessage = $"saved: mail='{user.user_email}', pw='{user.user_password}', role='{repo.AssignUserRole(user.user_email)}'";

                bool success = repo.SignUp(user);
                MessageBox.Show(success ? "User registered!" : "Registration failed.");
            },
            canExecute: obj => true
        );


            // Load the email from UserModel - make sure to initialize this first
            //  LoadUserData();

            // SignUpCommand = new ViewModelCommand(ExecuteSignUpCommand, CanExecuteSignUpCommand);

            // Debug - check if email was loaded
            // Console.WriteLine($"SignUpViewModel initialized with email: {SignUpEmail}");
        }









    }
}