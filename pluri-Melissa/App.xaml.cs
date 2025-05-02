using System.Configuration;
using System.Data;
using System.Windows;
using Project.Services;
using Project.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace Project
{
   
    public partial class App : Application
    {
        private readonly IServiceCollection services = new ServiceCollection();
        private readonly IServiceProvider _serviceProvider;
        public static IServiceProvider ServiceProviderInstance { get; private set; }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        public App()
        {
            services.AddSingleton<ArticleCardViewModel>();
            services.AddSingleton<HomePageViewModel>();
            services.AddSingleton<MODCommentViewModel>();
            services.AddSingleton<ViewModelLocator>();
            services.AddSingleton<WindowMapper>();
            services.AddSingleton<SideBarViewModel>();
            services.AddSingleton<CommentViewModel>();
            services.AddSingleton<WelcomeViewModel>();
            services.AddSingleton<EmailVerificationViewModel>();
            services.AddSingleton<SignUpViewModel>();
            services.AddSingleton<LoginViewModel>();


            services.AddSingleton<IWindowManager, WindowManager>();
            services.AddSingleton<IUserSessionService, UserSessionService>();
            services.AddSingleton<ICommentService, CommentService>();


            





            //services.AddSingleton<LoginViewModel>();
            //services.AddSingleton<WelcomeViewModel>();



            // services.AddSingleton<IItemService, ItemService>();


            _serviceProvider = services.BuildServiceProvider();
            ServiceProviderInstance = _serviceProvider;

            ViewModelLocator.Init(ServiceProviderInstance);
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


            AllocConsole();
            Console.WriteLine("Console attached!");
        }
    }

}
