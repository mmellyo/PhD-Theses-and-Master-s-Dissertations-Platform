
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Project.Models;
using Project.Repos;
using Org.BouncyCastle.Bcpg;
using System.Windows;
using Project.Services;
using System.Security.Cryptography;
using Project.Commands;
using Project.Stores;

namespace Project.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private string _loginemail;
        private bool _isViewVisible = true;
        private UserModel usermodel;
        private IUserRepos userRepos;
        private readonly IUserSessionService _userSession;
        

        


        //setters/getters
        public string LoginPassword { get; set; }
        public string LoginEmail { get; set; }


        public int UserId { get; set; }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));  // Notify the UI of the property change
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));  // Notify the UI of the property change
            }
        }

        public bool IsViewVisible
        {
            get => _isViewVisible;
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible)); // Notify the UI of the property change
            }
        }




        //COMMANDS
        public ICommand LoginCommand { get; }

        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }
        public ICommand GoSignUpCommand { get; }
        public ICommand GoHomeCommand { get; }


        public LoginViewModel(NavigationStore navigationStore)
        {
            GoHomeCommand = new NavigateCommand<WelcomeViewModel>(navigationStore, () => new WelcomeViewModel(navigationStore));
            GoSignUpCommand = new NavigateCommand<SignUpViewModel>(navigationStore, () => new SignUpViewModel(navigationStore));

            usermodel = new UserModel();
            

           
            LoginCommand = new LoginCommand(this, navigationStore);

                     
           
        }



        //constructs
        /*public LoginViewModel(IUserSessionService userSession, IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {
 




            //******************************************** COMMAND LOGIN
            LoginCommand = new ViewModelCommand(
            execute: obj =>
            {
                string enteredEmail = LoginEmail;

                if (LoginEmail.Equals("theses.usthb@gmail.com"))
                {
                    _windowManager.CloseWindow();

                    
                    _windowManager.ShowWindow(_viewModelLocator.SideBarViewModel);

                } else
                {

                    _userSession.Email = this.LoginEmail;
                     UserId = userRepos.AuthenticateUser(LoginEmail, LoginPassword);

                    if (UserId != 0)
                    {
                        // Assigns usser info
                        _viewModelLocator.MyProfileViewModel.InitializeWithUserId(UserId);
                        _viewModelLocator.CommentViewModel.InitializeWithUserId(UserId);

                        MessageBox.Show("loggin in successfully");
                        Console.WriteLine("************************LOGED IN***********************************");
                        IsViewVisible = false;

                        _windowManager.CloseWindow();
                        _windowManager.ShowWindow(_viewModelLocator.rechercheWinViewModel);

                    }
                    else
                    {
                        MessageBox.Show("Invalid email or password");

                    }

                }
            
           canExecute: obj =>true
        );

        











        }*/


    }
}