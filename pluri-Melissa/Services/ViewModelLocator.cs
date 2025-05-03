using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Project.ViewModels;
using Project.View;

namespace Project.Services
{



    //fetch VM


    public class ViewModelLocator
    {
        private static IServiceProvider _provider;

        public static void Init(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ViewModelLocator()
        {
            // parameterless constructor for XAML
            if (_provider == null)
                throw new InvalidOperationException("ViewModelLocator has not been initialized.");
        }

        public EmailVerificationViewModel EmailVerificationViewModel => _provider.GetRequiredService<EmailVerificationViewModel>() ;
        public SignUpViewModel SignUpViewModel => _provider.GetRequiredService<SignUpViewModel>();
        public LoginViewModel LoginViewModel => _provider.GetRequiredService<LoginViewModel>();
        public WelcomeViewModel WelcomeViewModel => _provider.GetRequiredService<WelcomeViewModel>();
        public CommentViewModel CommentViewModel => _provider.GetRequiredService<CommentViewModel>();
        public MODCommentViewModel MODCommentViewModel => _provider.GetRequiredService<MODCommentViewModel>();

        public SideBarViewModel SideBarViewModel => _provider.GetRequiredService<SideBarViewModel>();
        public HomePageViewModel HomePageViewModel => _provider.GetRequiredService<HomePageViewModel>();
        public MyProfileViewModel MyProfileViewModel => _provider.GetRequiredService<MyProfileViewModel>();





    }
}
