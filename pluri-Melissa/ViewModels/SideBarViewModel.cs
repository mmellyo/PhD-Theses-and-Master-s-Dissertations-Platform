using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Project.Services;
using Project.View;

namespace Project.ViewModels
{
    public class SideBarViewModel : ViewModelBase
    {


        private bool _isHomeExpanded;
        public bool IsHomeExpanded
        {
            get { return _isHomeExpanded; }
            set
            {
                if(_isHomeExpanded != value)
                {
                    _isHomeExpanded = value;
                    OnPropertyChanged(nameof(IsHomeExpanded));
                }   
 
            }
        }

        public ICommand ToggleHomeCommand { get; }
        public ICommand NavigateReportedCommentsCommand { get; }
        public ICommand NavigateAutoFlaggedCommentsCommand { get; }

        
        public ICommand NavigateReportedThesesCommand { get; }
        public ICommand NavigateReportedAccountsCommand { get; }



        public ICommand NavigateHomeOption3Command { get; }
        public ICommand NavigateSearchCommand { get; }
        public ICommand NavigateSettingsCommand { get; }
        public ICommand NavigateHelpCommand { get; }
        public ICommand NavigateReportsCommand { get; }
        public ICommand NavigateInboxCommand { get; }

        private readonly IWindowManager _windowManager;
        private readonly ViewModelLocator _viewModelLocator;

        public SideBarViewModel(IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {
            _windowManager = windowManager;
            _viewModelLocator = viewModelLocator;

            NavigateAutoFlaggedCommentsCommand = new ViewModelCommand(_ =>
            {
                _windowManager.CloseWindow();
                _windowManager.ShowWindow(_viewModelLocator.MODCommentViewModel);
            });
            NavigateAutoFlaggedCommentsCommand = new ViewModelCommand(_ => NavigateTo(_viewModelLocator.MODCommentViewModel));

            


            
            
            
            
            
            
            
            
            ToggleHomeCommand = new ViewModelCommand(_ => IsHomeExpanded = ! IsHomeExpanded);
            
        }

        private void NavigateTo (ViewModelBase viewModel)
        {
            _windowManager.CloseWindow();
            _windowManager.ShowWindow(viewModel);
        }
    }
}
