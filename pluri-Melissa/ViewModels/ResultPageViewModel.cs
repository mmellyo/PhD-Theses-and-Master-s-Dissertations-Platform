using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using gestion.Model;
using Project.Commands;
using Project.Models;
using Project.Repos;
using Project.Services;
using Project.Stores;
using Project.Utils;
using SharpVectors.Dom;

namespace Project.ViewModels
{
    public class ResultPageViewModel : ViewModelBase
    {

        //fields
        private readonly UserModel _userModel;
        private readonly IUserSessionService _userSession;
        private readonly UserRepos _userRepos;
        private readonly TheseRepo _theseRepo;
        private readonly theseResultatRepo _theseResultatRepo;
        private readonly ReportsRepo _reportRepo;
        private readonly EmailVerificationRepo _emailVerificationRepo;
        private readonly IWindowManager _windowManager;
        private readonly ViewModelLocator _viewModelLocator;


        //setters / getters
        public int TheseId { get; set; }
        public string NomThese { get; set; }
        public string NomAuteur { get; set; }
        public string MotCles { get; set; }
        public string Faculte { get; set; }
        public string NomEncadrant { get; set; }
        public string Langue { get; set; }
        public string Diplome { get; set; }
        public string AnneeUniversitaire { get; set; }
        public string Departement { get; set; }
        public string Resume { get; set; }

        public ITheseService TheseService { get; }



        private ObservableCollection<theseResultat> _results;
        public ObservableCollection<theseResultat> Results
        {
            get { return _results; }
            set
            {
                _results = value;
                OnPropertyChanged(nameof(Results));
                IsAdvancedSearch = true;
            }
        }

        private bool _isAdvancedSearch;
        public bool IsAdvancedSearch
        {
            get => _isAdvancedSearch;
            set
            {
                _isAdvancedSearch = value;
                OnPropertyChanged(nameof(IsAdvancedSearch));
            }
        }

        private string _searchKey;
        public string SearchKey
        {
            get { return _searchKey; }
            set
            {
                _searchKey = value;
                OnPropertyChanged(nameof(SearchKey));
                if (!IsAdvancedSearch)
                    LoadResults();
            }
        }
        private readonly ITheseService _theseService;
        private theseResultat _selectedThese;
        private NavigationStore navigationStore;
        private int userid;

        //commands
        //        public ICommand consulterTheseCommand { get; }



        //constructors
        /*public ResultPageViewModel(ITheseService theseService, IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {
            //fields
            
            
            _windowManager = windowManager;
            _viewModelLocator = viewModelLocator;
            
            _theseService = theseService;

            // Ensure these collection is initialized
            if (TheseService.Theses == null)
            {
                Console.WriteLine("Theses collection is null - Initializing Theses collection");
                TheseService.Theses = new ObservableCollection<theseResultat>();
            }


            
            else
            {
                Console.WriteLine("Theses collection is not empty - we came from ADVNCD SEARCH");
            }





          
        }*/

        public string Role;
        public bool isMember { get; set; }
        public bool isUser { get; set; }

        public object SideBarViewModel { get; }

        public ICommand UploadCommand { get; }

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
        public ResultPageViewModel(NavigationStore navigationStore, int userid, ITheseService theseService)
        {
            this.navigationStore = navigationStore;
            this.userid = userid;
            TheseService = theseService;
            _theseResultatRepo = new theseResultatRepo();
            _userModel = new UserModel();
            _userRepos = new UserRepos();
            _theseRepo = new TheseRepo();
            _reportRepo = new ReportsRepo();

            if (TheseService.Theses == null)
            {
                TheseService.Theses = new ObservableCollection<theseResultat>();
            }

            if (TheseService.Theses.Count == 0)
            {
                TheseService.Theses = new ObservableCollection<theseResultat>();
                LoadResults();
            }

            Role = _userRepos.GetUserRole(userid);
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

            _userName = _userRepos.GetuserName(userid);
            user_profilepic = ByteArrayToImageConverter.LoadImageSourceFromBytes(_userRepos.GetProfilepicFromId(userid));
            Email = _userRepos.GetuserEmail(userid);
            User_role = _userRepos.GetUserRole(userid);
        }


        //methods
        private void LoadResults()
        {
            try
            {
                TheseService.Theses.Clear();
                Console.WriteLine("Cleared  collections");

                // Load from DB
                var results = _theseResultatRepo.rechercheThese(SearchKey);
                Console.WriteLine("Loaded comments from repo");

                if (results != null)
                {
                    foreach (var r in results)
                    {
                        var result = new theseResultat
                        {
                            NomThese = r.NomThese,
                            NomAuteur = r.NomAuteur,
                            MotCles = r.MotCles,
                            Faculte = r.Faculte,
                            NomEncadrant = r.NomEncadrant,
                            Langue = r.Langue,
                            Diplome = r.Diplome,
                            AnneeUniversitaire = r.AnneeUniversitaire,
                            Departement = r.Departement,
                            Resume = r.Resume,
                            TheseId = r.TheseId,
                            consulterTheseCommand = new NavigateCommand<ThesePageViewModel>(navigationStore,() => new ThesePageViewModel(navigationStore, userid, r.TheseId))
                        };
                        Console.WriteLine("this.TheseId = r.TheseId; " + this.TheseId);  //works


                        //each thses resultat with its own command
                        try
                        {
                            result.consulterTheseCommand = new NavigateCommand<ThesePageViewModel>(navigationStore, () => new ThesePageViewModel(navigationStore, userid, r.TheseId));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error creating command: {ex.Message}");
                        }

                        TheseService.Theses.Add(result);

                    }

                    Console.WriteLine($"Loaded {TheseService.Theses.Count} result successfully");
                }
                else
                {
                    Console.WriteLine("No comments found for this these");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading comments: {ex.Message}");
                MessageBox.Show($"Failed to load comments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


 
    }
}
