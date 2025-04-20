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
        private readonly IServiceProvider _provider;

        public ViewModelLocator(IServiceProvider provider)
        { 
            _provider = provider;
        }

        public EmailVerificationViewModel EmailVerificationViewModel => _provider.GetRequiredService<EmailVerificationViewModel>() ;
        public SignUpViewModel SignUpViewModel => _provider.GetRequiredService<SignUpViewModel>();
        public LoginViewModel LoginViewModel => _provider.GetRequiredService<LoginViewModel>();
        public WelcomeViewModel WelcomeViewModel => _provider.GetRequiredService<WelcomeViewModel>();
        public CommentViewModel CommentViewModel => _provider.GetRequiredService<CommentViewModel>();



    }
}
