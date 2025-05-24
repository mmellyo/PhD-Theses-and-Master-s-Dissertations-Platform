using Project.Commands;
using Project.Models;
using Project.Repos;
using Project.Stores;
using Project.Utils;
using Project.View;
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

        public readonly ObservableCollection<ArticleViewModel> _articles;
        public IEnumerable<ArticleViewModel> ArticleViewModels => _articles;


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

        public HomePageViewModel(NavigationStore navigationStore, int user_id)
        {


            _navigationStore = navigationStore;
            this.user_id = user_id;

            UserModel user1 = new UserModel("malakbenzahia", "Malak Benzahia");
            authors.Add(user1);
            unregistered_authors.Add("John Doe");
            keywords.Add("blah blah");
            faculties.Add("Faculty of blah");

            ArticleViewModel article1 = new ArticleViewModel(
                                                    new ArticleModel("these bla bla", "blabla bla bla bla", "these de master",
                                                    authors, unregistered_authors, keywords, faculties, "september 2023")
                                        );
            ArticleViewModel article2 = new ArticleViewModel(
                                                    new ArticleModel("these bla bla", "blabla bla bla bla", "these de master",
                                                    authors, unregistered_authors, keywords, faculties, "september 2023")
                                        );
            _articles = new ObservableCollection<ArticleViewModel>()
            {
                article1,
                article2
            };

            UserRepos = new UserRepos();

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

       


    }
}
