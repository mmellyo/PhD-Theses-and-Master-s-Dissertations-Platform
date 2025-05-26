using System.Configuration;
using System.Data;
using System.Windows;
using Project.Services;
using Project.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using Project.Stores;
using Project.View;
using Microsoft.EntityFrameworkCore;


namespace Project
{
   
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            
            
            NavigationStore navigationStore = new NavigationStore();


            navigationStore.CurrentViewModel = new ThesePageViewModel(navigationStore, 1, 4);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(navigationStore)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }
    }

}

