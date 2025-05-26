using Project.Models;
using Project.Repos;
using Project.Services;
using Project.Stores;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.Commands
{
    internal class SignUpCommand : CommandBase
    {
        private readonly SignUpViewModel _viewModel;
        private readonly NavigationStore _navigationStore;
        private readonly UserRepos _userRepos;
        private readonly EmailVerificationRepo _emailVerificationRepo;

        public SignUpCommand(SignUpViewModel viewModel, NavigationStore navigationStore)
        {
            _viewModel = viewModel;
            _navigationStore = navigationStore;
            _userRepos = new UserRepos();
            _emailVerificationRepo = new EmailVerificationRepo();
        }

        public override void Execute(object parameter)
        {
            if (_viewModel.Password  != _viewModel.ConfirmPassword)
            {
                MessageBox.Show("Passwords do not match! Please re-enter to confirm.", "Passwords do not match", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            //profile pic
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "download_4.jpg");

            byte[] profilePic;

            if (File.Exists(imagePath))
            {
                profilePic = File.ReadAllBytes(imagePath);
            }
            else
            {
                // Create a gray 1x1 PNG placeholder
                using (var bmp = new System.Drawing.Bitmap(1, 1))
                {
                    bmp.SetPixel(0, 0, System.Drawing.Color.Gray);
                    using (var ms = new MemoryStream())
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        profilePic = ms.ToArray();
                    }
                }
            }

            var user = new UserModel
            {
                user_name = _userRepos.GetUsernameFromEmail(_viewModel.Email),
                user_email = _viewModel.Email,
                user_password = _viewModel.Password,
                user_role = _userRepos.AssignUserRole(_viewModel.Email),
                user_faculty = _viewModel.Faculty,
                user_profilepic = profilePic
            };


            //_userSession.Email = this.Email;
            //ErrorMessage = $"saved: mail='{user.user_email}', pw='{user.user_password}', role='{repo.AssignUserRole(user.user_email)}'";

            int userId = _userRepos.SignUp(user);

            if (userId > 0)
            {
                MessageBox.Show(
                                   "A recovery file will be created to help you reset your password in case you forget it.\n\n" +
                                   "This file is essential. Keep it somewhere safe. If you lose it, you may not be able to recover your account.\n\n" +
                                   "Do you want to continue?",
                                   "Important: Recovery File",
                                   MessageBoxButton.YesNo,
                                   MessageBoxImage.Warning
                                );

                _emailVerificationRepo.CreateRecoveryFile(user.user_email, user.user_name);
                _navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
            }


            //if (success) { _userSession.Email = this.Email; }



        }
    }
}
