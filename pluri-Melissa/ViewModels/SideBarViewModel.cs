using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Project.Commands;
using Project.Repos;
using Project.Services;
using Project.Stores;
using Project.View;

namespace Project.ViewModels
{
    public class SideBarViewModel : ViewModelBase
    {
        private NavigationStore _navigationStore;
        private string role;
        private UserRepos UserRepos;

        
        
        
        public ICommand NavigateReportedCommentsCommand { get; }
        public ICommand NavigateAutoFlaggedCommentsCommand { get; }

        
        public ICommand NavigateReportedThesesCommand { get; }
        public ICommand NavigateReportedAccountsCommand { get; }



        
        //public ICommand NavigateSettingsCommand { get; }
        //public ICommand NavigateHelpCommand { get; }
        public ICommand NavigateReportsCommand { get; }
        public ICommand NavigateInboxCommand { get; }

        

        public SideBarViewModel(int user_id, NavigationStore navigationStore)
        {
            this._navigationStore = navigationStore;
            
            
        }



       
    }
}
