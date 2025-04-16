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

namespace Project.ViewModels
{
    public class WelcomeViewModel : ViewModelBase
    {   
        
        // fields 
        private IWindowManager _windowManager;
        private ViewModelLocator _viewModelLocator;
        private bool _isViewVisible;

     


        // getters / setters
        public bool IsViewVisible
        {
            get => _isViewVisible;
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible)); // Notify the UI of the property change
            }
        }




        // commands
        public ICommand GoToLogin { get; }
        public ICommand GoToSignup { get; }






        // constructor
        public WelcomeViewModel(IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {

            //fields
            _windowManager = windowManager;
            _viewModelLocator = viewModelLocator;



            //******************************************** COMMAND GO TO LOGIN
            GoToLogin = new ViewModelCommand(
            execute: obj =>
            {

                  _windowManager.CloseWindow(); 


                // Switch the window to LoginVM
                _windowManager.ShowWindow(_viewModelLocator.LoginViewModel);

                IsViewVisible = false;
            }
        );



            //******************************************** COMMAND VGO TO SIGN UP
            GoToSignup = new ViewModelCommand(
            execute: obj =>
            {
                  _windowManager.CloseWindow(); 

                    IsViewVisible = false;
                // Switch the window to sign up (1st Email verification)
                _windowManager.ShowWindow(_viewModelLocator.EmailVerificationViewModel);

               
            }
        );
              
        }
    }
}


