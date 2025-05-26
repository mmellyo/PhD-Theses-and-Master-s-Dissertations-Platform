using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Repos;
using Project.Utils;
using Project.View;
using System.Windows.Input;
using System.Windows;
using Project.Services;
using Project.Models;
using gestion.Model;
using System.Collections.ObjectModel;
using MySqlX.XDevAPI.Common;
using MailKit.Search;
using Project.Stores;
using Project.Commands;
using System.Windows.Media;

namespace Project.ViewModels
{
    public class RechercheAvanceViewModel : ViewModelBase
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

        //setter / getter
        public string NomAuteur { get; set; }
        public string NomEncadrant { get; set; }
        public string NomThese { get; set; }
        public string MotCle { get; set; }

        private ObservableCollection<string> _langues;
        public ObservableCollection<string> Langues
        {
            get => _langues;
            set { _langues = value; OnPropertyChanged(nameof(Langues)); }
        }

        private string _langue;
        public string Langue
        {
            get => _langue;
            set { _langue = value; OnPropertyChanged(nameof(Langue)); }
        }

        private ObservableCollection<string> _departements;
        public ObservableCollection<string> Departements
        {
            get => _departements;
            set { _departements = value; OnPropertyChanged(nameof(Departements)); }
        }

        private string _departement;
        public string Departement
        {
            get => _departement;
            set { _departement = value; OnPropertyChanged(nameof(Departement)); }
        }


        private void InitializeFilters()
        {
            Langues = new ObservableCollection<string> { "Arabe", "Français", "Anglais" };
            Departements = new ObservableCollection<string>
    {
        "Informatique",
        "Biologie",
        "Mathématiques",
        "Physique",
        "Chimie",
        "Génie civil",
        "Génie électrique"
    };
            Annees = new ObservableCollection<string>
    {
        "2012/2013", "2013/2014", "2014/2015", "2015/2016", "2016/2017",
        "2017/2018", "2018/2019", "2019/2020", "2020/2021", "2021/2022",
        "2022/2023", "2023/2024", "2024/2025"
    };
            Facultes = new ObservableCollection<string>
    {
        "Civil Engineering",
        "Biological Sciences",
        "Earth Sciences, Geography And Territorial Planning",
        "Chemistry",
        "Computer Science",
        "Electrical Engineering",
        "Physics",
        "Process And Mechanical Engineering",
        "Mathematics"
    };
        }


        private ObservableCollection<string> _annees;
        public ObservableCollection<string> Annees
        {
            get => _annees;
            set { _annees = value; OnPropertyChanged(nameof(Annees)); }
        }

        private string _annee;
        public string Annee
        {
            get => _annee;
            set { _annee = value; OnPropertyChanged(nameof(Annee)); }
        }



        private ObservableCollection<string> _facultes;
        public ObservableCollection<string> Facultes
        {
            get => _facultes;
            set { _facultes = value; OnPropertyChanged(nameof(Facultes)); }
        }

        private string _faculte;
        public string Faculte
        {
            get => _faculte;
            set { _faculte = value; OnPropertyChanged(nameof(Faculte)); }

        }

        private bool _isAdvancedSearch;
        private NavigationStore navigationStore;
        private int userid;

        public bool IsAdvancedSearch
        {
            get => _isAdvancedSearch;
            set
            {
                _isAdvancedSearch = value;
                OnPropertyChanged(nameof(IsAdvancedSearch));
            }
        }

        public ITheseService TheseService { get; }








        //commands
        public ICommand RechercherCommand { get; }

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

        //constructor
        /*public RechercheAvanceViewModel(ITheseService theseService, IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {
            //fields
            _userModel = new UserModel();
            _userRepos = new UserRepos();
            _theseRepo = new TheseRepo();
            _reportRepo = new ReportsRepo();
            _theseResultatRepo = new theseResultatRepo();
            _windowManager = windowManager;
            _viewModelLocator = viewModelLocator;
            TheseService = theseService;

            /// used for dropdowns
            InitializeFilters();

            // Ensure collection is initialized
            if (TheseService.Theses == null)
            {
                Console.WriteLine("comment collection is null - Initializing Comments collection");
                TheseService.Theses = new ObservableCollection<theseResultat>();
            }

            //commands
            RechercherCommand = new ViewModelCommand(
                execute: obj =>
                {
                    var filteredResults = RechercherAvecFiltres();
                    TheseService.Theses = new ObservableCollection<theseResultat>(filteredResults);

                    _viewModelLocator.ResultPageViewModel.TheseService.Theses = TheseService.Theses;
                    _windowManager.CloseWindow();
                    _windowManager.ShowWindow(_viewModelLocator.ResultPageViewModel);
                }
            );
        }*/

        public RechercheAvanceViewModel(NavigationStore navigationStore, int userid)
        {
            this.navigationStore = navigationStore;
            this.userid = userid;
            _userModel = new UserModel();
            _userRepos = new UserRepos();
            _theseRepo = new TheseRepo();
            _reportRepo = new ReportsRepo();
            _theseResultatRepo = new theseResultatRepo();
            TheseService = new TheseService();

            // 👉 Initialiser les filtres pour les ComboBox
            InitializeFilters();

            if (TheseService.Theses == null)
            {
                TheseService.Theses = new ObservableCollection<theseResultat>();
            }

            RechercherCommand = new SearchCommand(this, navigationStore, userid);
            UploadCommand = new NavigateCommand<UploadViewModel>(navigationStore, () => new UploadViewModel(navigationStore, userid));

            _userName = _userRepos.GetuserName(userid);
            user_profilepic = ByteArrayToImageConverter.LoadImageSourceFromBytes(_userRepos.GetProfilepicFromId(userid));
            Email = _userRepos.GetuserEmail(userid);
            User_role = _userRepos.GetUserRole(userid);
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
        }


        public List<theseResultat> RechercherAvecFiltres()
        {
            var results = new List<theseResultat>();
            try
            {
                TheseService.Theses.Clear();

                // Load from DB
                var resultats = _theseResultatRepo.rechercherAvecFiltres(NomAuteur, NomEncadrant, NomThese, MotCle, Langue, Departement, Annee, Faculte);


                if (resultats != null)
                {
                    foreach (var r in resultats)
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
                            TheseId = r.TheseId
                        };

                        //each thses resultat with its own command
                        try
                        {
                            result.consulterTheseCommand = new ViewModelCommand(
                            execute: obj =>
                            {
                                int tId = result.TheseId;
                                _viewModelLocator.CommentViewModel.InitializeWithTheseId(tId);
                                Console.WriteLine("TheseId that is sending from ADVNCVM to COMMENTVM is: " + tId);
                                _windowManager.CloseWindow();
                                _windowManager.ShowWindow(_viewModelLocator.CommentViewModel);
                            }
                            );
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error creating command: {ex.Message}");
                        }

                        results.Add(result);
                    }

                    Console.WriteLine($"Loaded {results.Count} result successfully");
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
            return results;
        }


    }

}

