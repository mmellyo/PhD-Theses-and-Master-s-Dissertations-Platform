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
        //useful 
        private int userid;
        private NavigationStore navigationStore;
        private TheseRepo theseRepo;
        private CommentRepo commentRepo;

        //top bar infos
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


        //side bar
        public object SideBarViewModel { get; }
        public string Role;
        public bool isMember { get; set; }
        public bool isUser { get; set; }

        //top bar
        private UserRepos UserRepos;
        public ICommand UploadCommand { get; }

        //Saved list
        public ObservableCollection<MarkedThesesViewModel> SavedArticles { get; set; }
        private List<int> SavedarticleIdList { get; set; }
        private List<ArticleModel> SavedarticleModels { get; set; }

        //Posted list
        public ObservableCollection<MarkedThesesViewModel> PostedArticles { get; set; }
        private List<int> PostedarticleIdList { get; set; }
        private List<ArticleModel> PostedarticleModels { get; set; }

        //comment list
        public ObservableCollection<CommentsViewModel> Comments { get; set; }
        private List<int> CommentIdList { get; set; }
        private List<CommentModel> CommentModels { get; set; }


        //constructor
        public UserProfileViewModel(NavigationStore _navigationStore, int user_id)
        {
            this.userid = user_id;
            this.navigationStore = _navigationStore;
            this.theseRepo = new TheseRepo();
            this.UserRepos = new UserRepos();
            this.commentRepo = new CommentRepo();

            //saved articles
            SavedarticleIdList = theseRepo.GetSavedThesesByUser(userid);
            SavedarticleModels = new List<ArticleModel>();

            foreach (int id in SavedarticleIdList)
            {
                SavedarticleModels.Add(theseRepo.GetThesisDetails(id));
            }
;
            SavedArticles = new ObservableCollection<MarkedThesesViewModel>(
                SavedarticleModels.Select(article => new MarkedThesesViewModel(navigationStore, article, userid))
            );

            //posted articles 
            PostedarticleIdList = theseRepo.GetThesesByUser(userid);
            PostedarticleModels = new List<ArticleModel>();

            foreach (int id in PostedarticleIdList)
            {
                PostedarticleModels.Add(theseRepo.GetThesisDetails(id));
            }
;
            PostedArticles = new ObservableCollection<MarkedThesesViewModel>(
                PostedarticleModels.Select(article => new MarkedThesesViewModel(navigationStore, article, userid))
            );

            //comments 
            //CommentIdList = commentRepo.GetCommentId(userid);
            CommentModels = commentRepo.GetUserComments(userid);

            //foreach (int id in PostedarticleIdList)
            //{
              //  CommentModels.Add(commentRepo.GetUserComments(user_id));
            //}
            Comments = new ObservableCollection<CommentsViewModel>(
                CommentModels.Select(comment => new CommentsViewModel(user_id, comment, navigationStore))
            );



            //sidebar
            Role = UserRepos.GetUserRole(user_id);
            if (Role == "Student")
            {
                isUser = true;
                isMember = false;
                SideBarViewModel = new UserSideBarViewModel(user_id, navigationStore);
            }
            if (Role == "Member")
            {
                isMember = true;
                isUser = false;
                SideBarViewModel = new MemberSideBarViewModel(user_id, navigationStore);
            }

            //topbar
            UploadCommand = new NavigateCommand<UploadViewModel>(navigationStore, () => new UploadViewModel(navigationStore, user_id));
            _userName = UserRepos.GetuserName(user_id);
            user_profilepic = ByteArrayToImageConverter.LoadImageSourceFromBytes(UserRepos.GetProfilepicFromId(user_id));
            Email = UserRepos.GetuserEmail(user_id);
            User_role = UserRepos.GetUserRole(user_id);

        }


        
        public List<CommentModel> InitialiseComments(int user_id)
        {
            var comments = new List<CommentModel>();

            comments = commentRepo.GetUserComments(user_id);

            return comments;
        }
        public void InitializeWithUserId(int userId)
        {
            Email = UserRepos.GetuserEmail(userId);
            _userName = UserRepos.GetuserName(userId);
            user_profilepic = ByteArrayToImageConverter.LoadImageSourceFromBytes(UserRepos.GetProfilepicFromId(userId));
        }


        

    }
}
