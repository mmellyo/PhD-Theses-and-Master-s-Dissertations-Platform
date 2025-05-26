using Project.Commands;
using Project.Models;
using Project.Repos;
using Project.Stores;
using Project.Utils;
using Project.View.userControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PdfiumViewer;

namespace Project.ViewModels
{
    public class ThesePageViewModel : ViewModelBase
    {
        // useful
        private NavigationStore navigationStore;
        public int userid;
        public int theseId;
        private TheseRepo _repo;

        //sidebar
        public string Role;
        public bool isMember { get; set; }
        public bool isUser { get; set; }

        public object SideBarViewModel { get; }


        // top bar
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
        public ObservableCollection<BitmapImage> BySameUploader { get; set; }





        //article
        private string _Apercutaepdf;
        public string Apercutaepdf
        {
            get => _Apercutaepdf;
            set
            {
                _Apercutaepdf = value;
                OnPropertyChanged(nameof(Apercutaepdf));
            }
        }

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

        public ObservableCollection<string> keys { get; set; } = new ObservableCollection<string>();
        private List<string> _keys {  get; set; }
        public ObservableCollection<string> Encadrant { get; set; } = new ObservableCollection<string>();
        private List<string> _encadrants { get; set; }
        public ObservableCollection<string> Authors { get; set; } = new ObservableCollection<string>();
        private List<string> _authors { get; set; }

        public ImageSource apercutaepdf => LoadPdfImage(theseId);

        public ICommand SaveCommand { get; set; }
        public ICommand DownloadCommand { get; set; }
        public ICommand ShareCommand { get; set; }
        public ICommand ReportCommand { get; set; }

        private bool _isReportPopupVisible;
        public bool IsReportPopupVisible
        {
            get => _isReportPopupVisible;
            set
            {
                _isReportPopupVisible = value;
                OnPropertyChanged(nameof(IsReportPopupVisible));
            }
        }

        private ReportFormViewModel _reportViewModel;
        public ReportFormViewModel ReportViewModel
        {
            get => _reportViewModel;
            set
            {
                _reportViewModel = value;
                OnPropertyChanged(nameof(ReportViewModel));
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
            _Enregistrement = article.saves.ToString();
            _Visite = article.visits.ToString();
            _repo.IncVisitCount(theseId);

            _keys = _repo.GetKeywords(theseId);
            _encadrants = _repo.getSupervisors(theseId);
            _authors = _repo.getAuthors(theseId);

            keys.Clear();
            foreach (var keyword in _keys)
            {
                keys.Add(keyword);
            }

            Encadrant.Clear();
            foreach (var supervisor in _encadrants)
            {
                Encadrant.Add(supervisor);
            }

            Authors.Clear();
            foreach (var author in _authors)
            {
                Authors.Add(author);
            }





            SaveCommand = new SaveCommand(navigationStore, this);
            DownloadCommand = new DownloadCommand(navigationStore, this);
            ReportCommand = new ReportCommand(navigationStore, this);


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
            

            UserModel user = _repo.GetUploaderInfo(theseId);
            Username = user.user_name;
            Email = user.user_email;
            Role = user.user_role;


        }
        private ImageSource LoadPdfImage(int articleId)
        {
            byte[]? pdfBytes = _repo.GetPdfBytesFromDatabase(articleId);
            ImageSource PdfFirstPageImage;

            PdfFirstPageImage = GetFirstPageImage(pdfBytes);

            return PdfFirstPageImage;
        }

        private ImageSource GetFirstPageImage(byte[] pdfBytes)
        {
            using var stream = new MemoryStream(pdfBytes);
            using var document = PdfDocument.Load(stream);
            using var image = document.Render(0, 300, 300, true); // Render first page
            return BitmapToImageSource((Bitmap)image);
        }



        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using MemoryStream memory = new();
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;

            BitmapImage bitmapImage = new();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = memory;
            bitmapImage.EndInit();
            bitmapImage.Freeze();

            return bitmapImage;
        }
    }
}
