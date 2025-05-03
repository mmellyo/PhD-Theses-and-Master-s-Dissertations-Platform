using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gestion.Model;
using Project.Utils;
using System.Windows.Input;
using Project.Services;
using Project.Repos;
using Project.View;

namespace Project.ViewModels
{
    public class rechercheWinViewModel : ViewModelBase
    {
        private readonly IWindowManager _windowManager;

        private ObservableCollection<suggestionRecherche> _suggestions;
        public ObservableCollection<suggestionRecherche> Suggestions
        {
            get => _suggestions;
            set
            {
                _suggestions = value;
                OnPropertyChanged(nameof(Suggestions));
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                Recherche(value);
            }
        }


        //commands
        public ICommand OpenResultPageCommand { get; }
        public ICommand OpenAdvancedSearchCommand { get; }


        //constructor
        public rechercheWinViewModel(IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {
            Console.WriteLine("rechercheWinViewModel constructor called");
            _windowManager = windowManager;

            Suggestions = new ObservableCollection<suggestionRecherche>();


            //commands
            OpenResultPageCommand = new ViewModelCommand(
                execute: obj =>
                {
                    Console.WriteLine("OpenResultPageCommand  clicked");
                    OpenResultPage();
                }
            );

            OpenAdvancedSearchCommand = new ViewModelCommand(
                execute: obj => OpenAdvancedSearch(),
                canExecute: obj => true
            );
        }



        private void OpenResultPage()
        {
            Resultaaat resultPage = new Resultaaat(SearchText);
            resultPage.Show();
        }











        private void OpenAdvancedSearch()
        {
            var advancedSearchWindow = new rechercheAvance();
            advancedSearchWindow.Show();
        }

        private void Recherche(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                Suggestions.Clear();
                return;
            }

            if (key != SearchText) return;

            var repo = new suggestionRechercheRepos();
            var gets = repo.recherche(key).Take(3);

            Suggestions.Clear();
            foreach (var get in gets)
            {
                Suggestions.Add(get);
            }
        }
    }
}
