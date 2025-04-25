using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
        public ICommand NavigateHomeOption1Command { get; }
        public ICommand NavigateHomeOption2Command { get; }
        public ICommand NavigateHomeOption3Command { get; }
        public ICommand NavigateSearchCommand { get; }
        public ICommand NavigateSettingsCommand { get; }
        public ICommand NavigateHelpCommand { get; }

        public SideBarViewModel()
        {
            ToggleHomeCommand = new ViewModelCommand(_ => IsHomeExpanded = ! IsHomeExpanded);
            NavigateHelpCommand = new ViewModelCommand(_ => NavigateTo("Help"));
            NavigateSearchCommand = new ViewModelCommand(_ => NavigateTo("Search"));
            NavigateSettingsCommand = new ViewModelCommand(_ => NavigateTo("Settings"));
            NavigateHomeOption1Command = new ViewModelCommand(_ => NavigateTo("HomeOption1"));
            NavigateHomeOption2Command = new ViewModelCommand(_ => NavigateTo("HomeOption2"));
            NavigateHomeOption3Command = new ViewModelCommand(_ => NavigateTo("HomeOption3"));

        }

        private void NavigateTo (string destination)
        {
            //Navigation Logic (when i have the windows)
            Console.WriteLine($"Navigating to {destination}");
        }
    }
}
