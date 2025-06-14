﻿using System;
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
using Project.Stores;
using Project.Commands;
using System.Windows.Media;
using System.Data;
using System.Windows;


//this will search - get results - send them to resultvm
// resultvm will display them


namespace Project.ViewModels
{
    public class rechercheWinViewModel : ViewModelBase
    {
        
        public ITheseService TheseService { get; }

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
        private NavigationStore navigationStore;
        private int userid;

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

        private UserRepos userRepos;

        public string Role;
        public bool isMember { get; set; }
        public bool isUser { get; set; }

        public object SideBarViewModel { get; }
        private ICommand UploadCommand { get; }

        private string _userName;
        public string Username
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        private string _userrole;
        public string User_role
        {
            get => _userrole;
            set
            {
                _userrole = value;
                OnPropertyChanged(nameof(User_role));
            }
        }

        private ImageSource _userpic;
        public ImageSource user_profilepic
        {
            get => _userpic;
            set
            {
                _userpic = value;
                OnPropertyChanged(nameof(user_profilepic));
            }
        }
        //constructor
        /*public rechercheWinViewModel(ITheseService theseService, IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {
            //fields
            Suggestions = new ObservableCollection<suggestionRecherche>();

            
            TheseService = theseService;


            //initialize the theses collection
            if (TheseService.Theses == null)
            {
                Console.WriteLine(" collection is null - Initializing  collection");
                TheseService.Theses = new ObservableCollection<theseResultat>();
            }


        


            
        }*/



        public rechercheWinViewModel(NavigationStore navigationStore, int userid) 
        {
            Suggestions = new ObservableCollection<suggestionRecherche>();


            OpenAdvancedSearchCommand = new NavigateCommand<RechercheAvanceViewModel>(
                navigationStore,
                () => new RechercheAvanceViewModel(navigationStore, userid)
            );
            TheseService = new TheseService();

            if (TheseService.Theses == null)
            {
                Console.WriteLine(" collection is null - Initializing  collection");
                TheseService.Theses = new ObservableCollection<theseResultat>();
            }

            userRepos = new UserRepos();
            this.navigationStore = navigationStore;
            this.userid = userid;

            Role = userRepos.GetUserRole(userid);
            if (Role == "Student")
            {
                isUser = true;
                isMember = false;
                SideBarViewModel = new UserSideBarViewModel(userid, navigationStore);
            }
            if (Role == "Member")
            {
                isMember = true;
                isUser = false;
                SideBarViewModel = new MemberSideBarViewModel(userid, navigationStore);
            }

            UploadCommand = new NavigateCommand<UploadViewModel>(navigationStore, () => new UploadViewModel(navigationStore, userid));

            _userName = userRepos.GetuserName(userid);
            user_profilepic = ByteArrayToImageConverter.LoadImageSourceFromBytes(userRepos.GetProfilepicFromId(userid));
            Email = userRepos.GetuserEmail(userid);
            User_role = userRepos.GetUserRole(userid);

            OpenResultPageCommand = new NavigateCommand<ResultPageViewModel>(navigationStore, ()=> new ResultPageViewModel(navigationStore, userid, TheseService));
        }


        private void Recherche(string key)
        {
            if (string.IsNullOrWhiteSpace(key) || key.Length < 3)
            {
                Suggestions.Clear();
                TheseService.Theses.Clear();
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

            var theseRepo = new theseResultatRepo();
            var resultats = theseRepo.rechercheThese(key);

            TheseService.Theses.Clear();
            foreach (var item in resultats)
            {
                var result = new theseResultat
                {
                    NomThese = item.NomThese,
                    NomAuteur = item.NomAuteur,
                    MotCles = item.MotCles,
                    Faculte = item.Faculte,
                    NomEncadrant = item.NomEncadrant,
                    Langue = item.Langue,
                    Diplome = item.Diplome,
                    AnneeUniversitaire = item.AnneeUniversitaire,
                    Departement = item.Departement,
                    Resume = item.Resume,
                    TheseId = item.TheseId
                };

                //each thses resultat with its own command

                try
                {
                    result.consulterTheseCommand = new ViewModelCommand(
                    execute: obj =>
                    {

                        MessageBox.Show("Vous allez consulter cette these", "Consulter These", MessageBoxButton.OK, MessageBoxImage.Information);
                        int tId = result.TheseId;
                        navigationStore.CurrentViewModel = new ThesePageViewModel(navigationStore, userid, tId);
                        MessageBox.Show("TheseId that is sending from ADVNCVM to THESEPAGEVM is: " + tId, "TheseId", MessageBoxButton.OK, MessageBoxImage.Information);


                        //go to ThesePageViewModel

                        //  _viewModelLocator.CommentViewModel.InitializeWithTheseId(tId);
                        // Console.WriteLine("TheseId that is sending from ADVNCVM to COMMENTVM is: " + tId);
                        // _windowManager.CloseWindow();
                        // _windowManager.ShowWindow(_viewModelLocator.CommentViewModel);
                    }
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating command: {ex.Message}");
                }




                TheseService.Theses.Add(result);

            }
        }

    }
}
