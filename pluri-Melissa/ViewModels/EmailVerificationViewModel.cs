using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Project.Commands;
using Project.Models;
using Project.Repos;
using Project.Services;
using Project.Stores;

namespace Project.ViewModels
{
    public class EmailVerificationViewModel : ViewModelBase
    {

        // fields 
        private string _email;
        private string _inputCode;
        private string _statusMessage;
        private  EmailVerificationRepo _emailVerificationRepo;
        private  EmailVerificationModel _emailVerificationModel;
        private readonly IUserRepos _userRepos;
        private UserModel _usermodel;
        private bool _isViewVisible;

        private string _inputVerificationCode;
        private string _codeStatusMessage;
        private NavigationStore navigationStore;
        private readonly IWindowManager _windowManager;
        private readonly ViewModelLocator _viewModelLocator;

        // public IItemService ItemService { get; set; }

        public Action OnWindowChange { get; set; }


        // getters / setters
        public string Email { get; set; }

        public bool IsViewVisible
        {
            get => _isViewVisible;
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        public string InputCode
        {
            get => _inputCode;
            set
            {
                _inputCode = value;
                OnPropertyChanged(nameof(InputCode));
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public string InputVerificationCode
        {
            get => _inputVerificationCode;
            set
            {
                _inputVerificationCode = value;
                OnPropertyChanged(nameof(InputVerificationCode));
            }
        }
        public string CodeStatusMessage
        {
            get => _codeStatusMessage;
            set
            {
                _codeStatusMessage = value;
                OnPropertyChanged(nameof(CodeStatusMessage));
            }
        }


        // commands
        public ICommand SendEmailCommand { get; }
        public ICommand VerifyCodeCommand { get; }
        public ICommand GoHomeCommand { get; }
        public ICommand GologinCommand { get; }


        // constructor
        /*public EmailVerificationViewModel(IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {

            //fields
            _windowManager = windowManager;
            _viewModelLocator = viewModelLocator;

            _emailVerificationRepo = new EmailVerificationRepo();
            _emailVerificationModel = new EmailVerificationModel();

            _userRepos = new UserRepos();
            _usermodel = new UserModel();






            //******************************************** COMMAND SEND EMAIL
            SendEmailCommand = new ViewModelCommand(
            execute: obj =>
            {
                if (!_userRepos.IsUsthbMember(Email))
                {

                    MessageBox.Show("Invalid email format. Please follow the correct format shown.", "Invalid email format", MessageBoxButton.OK, MessageBoxImage.Error);

                    // 1.5 seconds before clearing the status message
                    //await Task.Delay(1500);
                    //StatusMessage = string.Empty;
                }
                else
                {
                    if (_emailVerificationRepo.IsEmailTaken(Email))
                    {
                        MessageBox.Show("An account with this email already exists. Try logging in instead.", "Email Taken", MessageBoxButton.OK, MessageBoxImage.Error);

                    }

                    try
                    {


                   
                        string verificationCode = _emailVerificationRepo.GenerateVerificationCode();
                        _emailVerificationModel.SetVerificationCode(verificationCode);  // Store the code in the model
                        string storedCode = _emailVerificationModel.GetVerificationCode(); //just to make sure


                        // Send the email with the generated code
                        _emailVerificationRepo.SendVerificationEmail(Email, verificationCode);
                        StatusMessage = "A verification code has been sent to your email. Please enter it below.";

                        // Switch the window
                        //OnWindowChange?.Invoke();




                    }
                    catch (Exception ex)
                    {
                        StatusMessage = $"Failed to send email: {ex.Message}";
                        Console.WriteLine($"Error: {ex}");
                    }
                }
            }
        );



            //******************************************** COMMAND VERIFY CODE
            VerifyCodeCommand = new ViewModelCommand(
            execute: obj =>
            {
                // Get the verification code from the model
                string storedCode = _emailVerificationModel.GetVerificationCode();

                Console.WriteLine($"Verification attempt: Input='{InputVerificationCode}', Stored='{storedCode}'");


                if (string.IsNullOrEmpty(storedCode))
                {
                    MessageBox.Show("Error: Verification code was not generated or has expired!", "Empty Verification code", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }


                if (InputVerificationCode.Trim() != storedCode)
                {
                    MessageBox.Show("Invalid code. Please try again.", "Errormail", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    CodeStatusMessage = "Email verified successfully!)";

                    //save info (mail / role) in usermodel
                    //_usermodel.SetCurrentUserEmail(Email);
                    // _usermodel.SetCurrentUserRole(_userRepos.AssignUserRole(Email));

                    //Assigns the entered email to the SignUpViewModel
                    _viewModelLocator.SignUpViewModel.Email = Email;
  
                   
                    _windowManager.CloseWindow();


                    // Switch the window to SignUpViewModel
                    _windowManager.ShowWindow(_viewModelLocator.SignUpViewModel);

                    


                    IsViewVisible = false;


                }
            }
            // canExecute: obj =>  !string.IsNullOrWhiteSpace(InputVerificationCode) && InputVerificationCode.Length >= 6
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
                    _windowManager.ShowWindow(_viewModelLocator.LoginViewModel);
                },
                canExecute: obj => true
                );

        }*/

        public EmailVerificationViewModel(NavigationStore navigationStore)
        {
            SendEmailCommand = new SendEmailCommand(this, navigationStore);
            VerifyCodeCommand = new VerifyCodeCommand(this, navigationStore);

            GoHomeCommand = new NavigateCommand<WelcomeViewModel>(navigationStore, () => new WelcomeViewModel(navigationStore));
            GologinCommand = new NavigateCommand<LoginViewModel>(navigationStore, () => new LoginViewModel(navigationStore));
        }
    }
}