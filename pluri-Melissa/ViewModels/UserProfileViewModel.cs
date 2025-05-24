using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Project.Commands;
using Project.Models;
using Project.Repos;
using Project.Services;
using Project.Stores;
using Project.Utils;

namespace Project.ViewModels
{
    public class UserProfileViewModel : ViewModelBase
    {
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


        public  ObservableCollection<ArticleViewModel> Theses { get; set; } = new ObservableCollection<ArticleViewModel>();
        public ObservableCollection<CommentModel> Comments { get; set; } = new ObservableCollection<CommentModel>();
        public ObservableCollection<ArticleViewModel> Saved { get; set; } = new ObservableCollection<ArticleViewModel>();


        //commands
        public ICommand GoToTheseCommand { get; }

        UserRepos _userRepos;
        TheseRepo theseRepo;
        CommentRepo commentRepo;
       

        public UserProfileViewModel(NavigationStore navigationStore, int userId)
        {
            _userRepos = new UserRepos();
            theseRepo = new TheseRepo();
            commentRepo = new CommentRepo();
            Role = _userRepos.GetUserRole(userId);
            InitializeWithUserId(userId);

            List<ArticleModel> Posted = InitialiseTheses(userId);
            List<ArticleModel> saved = InitialiseSavedTheses(userId);
            foreach (ArticleModel article in Posted)
            {
                ArticleViewModel a = new ArticleViewModel(article);
                Theses.Add(a);
            }
            foreach (ArticleModel article in saved)
            {
                ArticleViewModel a = new ArticleViewModel(article);
                Saved.Add(a);
            }
            Comments = new ObservableCollection<CommentModel>(InitialiseComments(userId));


            Role = _userRepos.GetUserRole(userId);
            if (Role == "Student")
            {
                isUser = true;
                isMember = false;
                SideBarViewModel = new UserSideBarViewModel(userId, navigationStore);
            }
            if (Role == "Member")
            {
                isMember = true;
                isUser = false;
                SideBarViewModel = new MemberSideBarViewModel(userId, navigationStore);
            }

            UploadCommand = new NavigateCommand<UploadViewModel>(navigationStore, () => new UploadViewModel(navigationStore, userId));

            _userName = _userRepos.GetuserName(userId);
            user_profilepic = ByteArrayToImageConverter.LoadImageSourceFromBytes(_userRepos.GetProfilepicFromId(userId));
            Email = _userRepos.GetuserEmail(userId);
            User_role = _userRepos.GetUserRole(userId);

        }

        public List<ArticleModel> InitialiseSavedTheses(int user_id)
        {
            List<ArticleModel> these = new List<ArticleModel>();

            these = theseRepo.GetSavedThesesByUser(user_id);

            return these;
        }

        public List<CommentModel> InitialiseComments(int user_id)
        {
            var comments = new List<CommentModel>();

            comments = commentRepo.GetUserComments(user_id);

            return comments;
        }
        public void InitializeWithUserId(int userId)
        {
            Email = _userRepos.GetuserEmail(userId);
            _userName = _userRepos.GetuserName(userId);
            user_profilepic = ByteArrayToImageConverter.LoadImageSourceFromBytes(_userRepos.GetProfilepicFromId(userId));
        }


        public List<ArticleModel> InitialiseTheses (int user_id)
        {
            List <ArticleModel> these = new List<ArticleModel>();

            these = theseRepo.GetThesesByUser(user_id);

            return these;
        }

    }
}
