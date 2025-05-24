using Org.BouncyCastle.Utilities;
using Project.Models;
using Project.Repos;
using Project.Services;
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
    class VerifyCodeCommand : CommandBase
    {
        private NavigationStore _navigationStore;
        private EmailVerificationViewModel _emailVerificationViewModel;
        private readonly UserRepos _userRepos;
        private readonly UserModel _usermodel;
        private readonly EmailVerificationRepo _emailVerificationRepo;
        private readonly EmailVerificationModel _emailVerificationModel;
        private string StatusMessage;
        private string _codeStatusMessage;



        public VerifyCodeCommand(EmailVerificationViewModel emailVerificationViewModel, NavigationStore navigationStore)
        {
            this._emailVerificationViewModel = emailVerificationViewModel;
            this._navigationStore = navigationStore;
            _emailVerificationRepo = new EmailVerificationRepo();
            _userRepos = new UserRepos();
            _usermodel = new UserModel();
            _emailVerificationModel = new EmailVerificationModel();
        }

        public override void Execute(object parameter)
        {
            string storedCode = _emailVerificationModel.GetVerificationCode();

            Console.WriteLine($"Verification attempt: Input='{_emailVerificationViewModel.InputVerificationCode}', Stored='{storedCode}'");


            if (string.IsNullOrEmpty(storedCode))
            {
                MessageBox.Show("Error: Verification code was not generated or has expired!", "Empty Verification code", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }


            if (_emailVerificationViewModel.InputVerificationCode.Trim() != storedCode)
            {
                MessageBox.Show("Invalid code. Please try again.", "Errormail", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                _emailVerificationViewModel.CodeStatusMessage = "Email verified successfully!)";

                //save info (mail / role) in usermodel
                //_usermodel.SetCurrentUserEmail(Email);
                // _usermodel.SetCurrentUserRole(_userRepos.AssignUserRole(Email));

                //Assigns the entered email to the SignUpViewModel
                _navigationStore.CurrentViewModel = new SignUpViewModel(_navigationStore);





            }
        }
    }

}
