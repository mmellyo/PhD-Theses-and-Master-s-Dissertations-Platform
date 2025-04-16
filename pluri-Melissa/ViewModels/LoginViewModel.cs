
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




        //setters/getters
        public string LoginPassword { get; set; }
        public string LoginEmail { get; set; }



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

        /// not yet
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }





        //constructs
        public LoginViewModel()
        {
            usermodel = new UserModel();
            userRepos = new UserRepos();


            //******************************************** COMMAND LOGIN
            LoginCommand = new ViewModelCommand(
            execute: obj =>
            {
                string enteredEmail = LoginEmail;
                var isValidUser = userRepos.AuthenticateUser(LoginEmail, LoginPassword);


                if (isValidUser)
                {
                    MessageBox.Show("loggin in successfully");

                    // Store the user in session
                    //usermodel. SetCurrentUserEmail(LoginEmail);
                    IsViewVisible = false;
                }
                else
                {
                    MessageBox.Show("Invalid email or password");

                }
            },
           canExecute: obj =>true
        );



        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;

            if (string.IsNullOrWhiteSpace(LoginEmail) || LoginEmail.Length < 3 ||
              LoginPassword == null || LoginPassword.Length < 3)
                validData = false;
            else
                validData = true;

            return validData;
        }

        private void ExecuteLoginCommand(object obj)
        {
            string enteredEmail = LoginEmail;
            var isValidUser = userRepos.AuthenticateUser(LoginEmail, LoginPassword);


            if (isValidUser)
            {
                ErrorMessage = "loggin in successfully";

                // Store the user in session
                //usermodel. SetCurrentUserEmail(LoginEmail);
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "Invalid username or password";
            }
        }




        //RecoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverPasswordCommand("", ""));

        /// not yet:
        private void ExecuteRecoverPasswordCommand(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}