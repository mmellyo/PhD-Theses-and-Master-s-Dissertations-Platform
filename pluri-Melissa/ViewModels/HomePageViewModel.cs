using Project.Commands;
using Project.Models;
using Project.Repos;
using Project.Stores;
using Project.Utils;
using Project.View;
using Project.View.userControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Project.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        

        int user_id;
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public UserRepos UserRepos { get; }

        private string _role;

        private readonly List<UserModel> authors = new List<UserModel>();
        private readonly List<string> keywords = new List<string>();
        private readonly List<string> unregistered_authors = new List<string>();
        private readonly List<string> faculties = new List<string>();

        public ObservableCollection<ArticleViewModel> ArticleViewModels { get; set; } = new ObservableCollection<ArticleViewModel>();
        public List<ArticleModel> articles = new List<ArticleModel>();

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


        private TheseRepo theseRepo;

        public HomePageViewModel(NavigationStore navigationStore, int user_id)
        {


            _navigationStore = navigationStore;
            this.user_id = user_id;
            theseRepo = new TheseRepo();
            UserRepos = new UserRepos();

            LoadArticles();

            



            Role = UserRepos.GetUserRole(user_id);
            if(Role == "Student")
            {
                isUser = true;
                isMember = false;
                SideBarViewModel = new UserSideBarViewModel(user_id, navigationStore);
            }
            if(Role == "Member")
            {
                isMember = true;
                isUser = false;
                SideBarViewModel = new MemberSideBarViewModel(user_id, navigationStore);
            }

            UploadCommand = new NavigateCommand<UploadViewModel>(navigationStore, () => new UploadViewModel(navigationStore, user_id));

            _userName = UserRepos.GetuserName(user_id);
            user_profilepic = ByteArrayToImageConverter.LoadImageSourceFromBytes(UserRepos.GetProfilepicFromId(user_id));
            Email = UserRepos.GetuserEmail(user_id);
            User_role = UserRepos.GetUserRole(user_id);

        }


        List<ArticleModel> GenerateFacultyBasedSuggestions(int userId, int maxSuggestions)
        {
            // Step 1: Get the user's faculty
            string userFaculty = UserRepos.GetUserFaculty(userId);
            List<ArticleModel> facultyArticles = new List<ArticleModel>();
            // Step 2: Get all articles from the same faculty
            List<int> facultyArticlesid = theseRepo.GetThesesByFaculty(userFaculty);
            foreach(int id in facultyArticlesid)
            {
                facultyArticles.Add(theseRepo.GetThesisDetails(id));
            }
            // Step 3: Score the articles
            foreach (var article in facultyArticles)
            {
                article.Score = ComputeScore(article); // see below
            }

            // Step 4: Sort by score in descending order
            facultyArticles.Sort((a, b) => b.Score.CompareTo(a.Score));

            // Step 5: Return top N suggestions
            return facultyArticles.Take(maxSuggestions).ToList();
        }

        // Custom scoring function
        double ComputeScore(ArticleModel article)
        {
            // Example weights — adjust as needed
            double visitWeight = 0.3;
            double saveWeight = 0.7;

            return (visitWeight * article.visits) + (saveWeight * article.saves);
        }

        public void LoadArticles()
        {
            articles = GenerateFacultyBasedSuggestions(user_id, 20);

            ArticleViewModels.Clear(); // Clear the existing items

            foreach (var article in articles)
            {
                ArticleViewModels.Add(new ArticleViewModel(article.id, _navigationStore, user_id));
            }
        }

    }
}
