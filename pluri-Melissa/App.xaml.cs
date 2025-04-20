using System.Configuration;
using System.Data;
using System.Windows;
using Project.Services;
using Project.ViewModels;
using Microsoft.Extensions.DependencyInjection;


namespace Project
{
   
    public partial class App : Application
    {
        private readonly IServiceCollection services = new ServiceCollection();
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            services.AddSingleton<EmailVerificationViewModel>();
            services.AddSingleton<SignUpViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<WelcomeViewModel>();
            services.AddSingleton<CommentViewModel>();


            services.AddSingleton<ViewModelLocator>();
            services.AddSingleton<WindowMapper>();
            services.AddSingleton<IWindowManager, WindowManager>();

            services.AddSingleton<ICommentService, CommentService>();


            _serviceProvider = services.BuildServiceProvider();
        }
        //test github
        protected override void OnStartup(StartupEventArgs e)
        {
            var windowManager = _serviceProvider.GetRequiredService<IWindowManager>();


            //start window (keep it at the welcomeVM)
            windowManager.ShowWindow(_serviceProvider.GetRequiredService<CommentViewModel>());

            base.OnStartup(e);
            //var mainWindow = new MainWindow();
            // var viewModelLocator = _serviceProvider.GetRequiredService<ViewModelLocator>();
            // mainWindow.DataContext = viewModelLocator.MainViewModel;
            // mainWindow.Show();
        }
    }

}
