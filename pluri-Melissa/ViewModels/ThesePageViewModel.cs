using Project.Commands;
using Project.Models;
using Project.Repos;
using Project.Stores;
using Project.Utils;
using Project.View.userControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Project.ViewModels
{
    public class ThesePageViewModel : ViewModelBase
    {
        private NavigationStore navigationStore;
        public int userid;
        public int theseId;


        public string Apercutaepdf { get; set; }
               





    


        public ICommand SaveCommand { get; set; }
        public ICommand DownloadCommand { get; set; }
        public ICommand ShareCommand { get; set; }
        public ICommand ReportCommand { get; set; }


        private TheseRepo _repo;


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




        //author

        private string _AuthorUsername;
        public string AuthorUsername
        {
            get => _AuthorUsername;
            set
            {
                _AuthorUsername = value;
                OnPropertyChanged(nameof(AuthorUsername));
            }
        }

        private string _AuthorEmail;
        public string AuthorEmail
        {
            get => _AuthorEmail;
            set
            {
                _AuthorEmail = value;
                OnPropertyChanged(nameof(AuthorEmail));
            }
        }


        private string _AuthorRole;
        public string AuthorRole
        {
            get => _AuthorRole;
            set
            {
                _userrole = value;
                OnPropertyChanged(nameof(AuthorRole));
            }
        }

        private ImageSource _imageUtilisateur;
        public ImageSource imageUtilisateur
        {
            get => _imageUtilisateur;
            set
            {
                _imageUtilisateur = value;
                OnPropertyChanged(nameof(imageUtilisateur));
            }
        }


        //article
        private string _Title;
        public string Title
        {
            get => _Title;
            set
            {
                _Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _AuteurString;
        public string AuteurString
        {
            get => _AuteurString;
            set
            {
                _AuteurString = value;
                OnPropertyChanged(nameof(AuteurString));
            }
        }

        private string _Departement;
        public string Departement
        {
            get => _Departement;
            set
            {
                _Departement = value;
                OnPropertyChanged(nameof(Departement));
            }
        }

        private string _AnneeUniv;
        public string AnneeUniv
        {
            get => _AnneeUniv;
            set
            {
                _AnneeUniv = value;
                OnPropertyChanged(nameof(AnneeUniv));
            }
        }

        private string _NomEncadrant;
        public string NomEncadrant
        {
            get => _NomEncadrant;
            set
            {
                _NomEncadrant = value;
                OnPropertyChanged(nameof(NomEncadrant));
            }
        }

        private string _Keyword1;
        public string Keyword1
        {
            get => _Keyword1;
            set
            {
                _Keyword1 = value;
                OnPropertyChanged(nameof(Keyword1));
            }
        }

        private string _Keyword2;
        public string Keyword2
        {
            get => _Keyword2;
            set
            {
                _Keyword2 = value;
                OnPropertyChanged(nameof(Keyword2));
            }
        }

        private string _Keyword3;
        public string Keyword3
        {
            get => _Keyword3;
            set
            {
                _Keyword3 = value;
                OnPropertyChanged(nameof(Keyword3));
            }
        }

        private string _Description;
        public string Description
        {
            get => _Description;
            set
            {
                _Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _Visite;
        public string Visite
        {
            get => _Visite;
            set
            {
                _Visite = value;
                OnPropertyChanged(nameof(Visite));
            }
        }

        private string _Enregistrement;
        public string Enregistrement
        {
            get => _Enregistrement;
            set
            {
                _Enregistrement = value;
                OnPropertyChanged(nameof(Enregistrement));
            }
        }

        private string _Faculte;
        public string Faculte
        {
            get => _Faculte;
            set
            {
                _Faculte = value;
                OnPropertyChanged(nameof(Faculte));
            }
        }


        //Comment stuff
        public ICommand AddCommentCommand { get; set; }
        public ICommand ToggleCommentCommand { get; set; }

        public ObservableCollection<CommentsViewModel> Comments { get; set; }
        private List<CommentModel> CommentModels { get; set; }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        private string _result;
        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }

        private CommentRepo _commentRepo;


        public ThesePageViewModel(NavigationStore navigationStore, int userid, int theseId)
        {
            this.navigationStore = navigationStore;
            this.userid = userid;
            this.theseId = theseId;
            _repo = new TheseRepo();
            _commentRepo = new CommentRepo();
            loadThese();
            SaveCommand = new SaveCommand(navigationStore, this);

            UserRepos UserRepos = new UserRepos();

            //side bar
            Role = UserRepos.GetUserRole(userid);
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


            //comment
            AddCommentCommand = new AddCommentCommand(this);
            CommentModels = _commentRepo.LoadTheseComments2(theseId);
            Comments = new ObservableCollection<CommentsViewModel>(
                CommentModels.Select(comment => new CommentsViewModel(comment, navigationStore, userid))
            );

            //top bar
            UploadCommand = new NavigateCommand<UploadViewModel>(navigationStore, () => new UploadViewModel(navigationStore, userid));
            _userName = UserRepos.GetuserName(userid);
            user_profilepic = ByteArrayToImageConverter.LoadImageSourceFromBytes(UserRepos.GetProfilepicFromId(userid));
            Email = UserRepos.GetuserEmail(userid);
            User_role = UserRepos.GetUserRole(userid);


            //article
            ArticleModel article = _repo.GetThesisDetails(theseId);

            _Title = article.title;
            _Description = article.description;
            _AnneeUniv = article.date;
            _Departement = article.department;
            _Faculte = article.faculty;
            _AuteurString = article.uploader;
            _Keyword1 = article.keywords[0];
            _Keyword2 = article.keywords[1];
            //_Keyword3 = article.keywords[2];
            _NomEncadrant = article.supervisor;
            _Enregistrement = article.saves.ToString();
            _Visite = article.visits.ToString();

            //author
            _AuthorUsername = article.uploader;
            _AuthorEmail = article.uploader_email;
            _AuthorRole = article.uploader_role;
            _imageUtilisateur = ByteArrayToImageConverter.LoadImageSourceFromBytes(article.uploader_pic);

        }


        private void loadThese()
        {
            ArticleModel article = _repo.GetThesisDetails(theseId);
            Title = article.title;
            Description = article.description;
            AuteurString = string.Join(" ", article.authors ?? new List<string>());
            Departement = article.department;
            Faculte = article.faculty;
            AnneeUniv = article.date;
            NomEncadrant = article.supervisor;
            Keyword1 = article.keywords.FirstOrDefault();
            Keyword2 = article.keywords.Skip(1).FirstOrDefault();
            Keyword3 = article.keywords.Skip(2).FirstOrDefault();

            UserModel user = _repo.GetUploaderInfo(theseId);
            Username = user.user_name;
            Email = user.user_email;
            Role = user.user_role;


        }
    }
}
