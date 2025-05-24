using Project.Models;
using Project.Repos;
using Project.Stores;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.Commands
{
    class SendEmailCommand : CommandBase
    {
        
        private NavigationStore _navigationStore;
        private EmailVerificationViewModel _emailVerificationViewModel;
        private readonly UserRepos _userRepos;
        private readonly UserModel _usermodel;
        private readonly EmailVerificationRepo _emailVerificationRepo;
        private readonly EmailVerificationModel _emailVerificationModel;

        public SendEmailCommand(EmailVerificationViewModel emailVerificationViewModel, NavigationStore navigationStore) {
            this._emailVerificationViewModel = emailVerificationViewModel;
            this._navigationStore = navigationStore;
            _emailVerificationRepo = new EmailVerificationRepo();
            _userRepos = new UserRepos();
            _usermodel = new UserModel();
            _emailVerificationModel = new EmailVerificationModel();
        }

        public override void Execute(object parameter)
        {

            if (!_userRepos.IsUsthbMember(_emailVerificationViewModel.Email))
            {

                MessageBox.Show("Invalid email format. Please follow the correct format shown.", "Invalid email format", MessageBoxButton.OK, MessageBoxImage.Error);

                // 1.5 seconds before clearing the status message
                //await Task.Delay(1500);
                //StatusMessage = string.Empty;
            }
            else
            {
                if (_emailVerificationRepo.IsEmailTaken(_emailVerificationViewModel.Email))
                {
                    MessageBox.Show("An account with this email already exists. Try logging in instead.", "Email Taken", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    try
                    {



                        string verificationCode = _emailVerificationRepo.GenerateVerificationCode();
                        _emailVerificationModel.SetVerificationCode(verificationCode);  // Store the code in the model
                        string storedCode = _emailVerificationModel.GetVerificationCode(); //just to make sure


                        // Send the email with the generated code
                        _emailVerificationRepo.SendVerificationEmail(_emailVerificationViewModel.Email, verificationCode);
                        _emailVerificationViewModel.StatusMessage = "A verification code has been sent to your email. Please enter it below.";

                        // Switch the window
                        //OnWindowChange?.Invoke();




                    }
                    catch (Exception ex)
                    {
                        _emailVerificationViewModel.StatusMessage = $"Failed to send email: {ex.Message}";
                        Console.WriteLine($"Error: {ex}");
                    }
                }
                    
            }


        }
    }
}
