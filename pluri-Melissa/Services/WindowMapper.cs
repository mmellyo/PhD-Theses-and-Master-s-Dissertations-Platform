﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Project.View;
using Project.ViewModels;

namespace Project.Services
{

    //MAP VM TO WINDOW
    public class WindowMapper
    {
        private readonly Dictionary<Type, Type> _mapping = new();

        public WindowMapper()
        {
            // Register everything
            //Registermapping<EmailVerificationViewModel, EmailVerificationView>();
            //Registermapping<SignUpViewModel, SignUp>();
            //Registermapping<LoginViewModel, Login>();
            //Registermapping<WelcomeViewModel, WelcomeView>();
            //Registermapping<HomePageViewModel, HomePage>();
            //Registermapping<MODCommentViewModel, MODcommentView>();
            //Registermapping<CommentViewModel, ThesesView>();
            //Registermapping<SideBarViewModel, AdminSpace>();
            Registermapping<MyProfileViewModel, MyProfileView>();
            //Registermapping<rechercheWinViewModel, rechercheWin>();
            //Registermapping<ResultPageViewModel, Resultaaat>();
            //Registermapping<RechercheAvanceViewModel, rechercheAvance>();
            //Registermapping<UserProfileViewModel, UserProfileView>();





        }



        public void Registermapping<TViewModel, TWindow>() where TViewModel : ViewModelBase where TWindow : Window
        {
            //setting everything 
            _mapping[typeof(TViewModel)] = typeof(TWindow);
        }



        public Type GetWindowTypeForViewModel(Type viewModelRype)
        {
            //i dded the exception
            if (_mapping.TryGetValue(viewModelRype, out var windowType))
            {
                return windowType;
            }
            throw new InvalidOperationException($"No window registered for {viewModelRype.Name}");
        }
    }
}
