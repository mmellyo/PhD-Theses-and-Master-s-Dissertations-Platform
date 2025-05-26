using Project.Models;
using Project.Repos;
using Project.Stores;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Project.ViewModels
{
    public class MODFlaggedArticlesViewModel : ViewModelBase
    {
        //useful 
        private int userid;
        private NavigationStore navigationStore;
        private TheseRepo theseRepo;

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

        public string Role;
        public bool isMember { get; set; }
        public bool isUser { get; set; }


        //top bar
        private UserRepos UserRepos;

        //Items Contorl
        public ObservableCollection<MemberArticleViewModel> Articles { get; set; }
        private List<int> articleIdList { get; set; }
        private List<ArticleModel> articleModels { get; set; }
        public object SideBarViewModel { get; }

        //constructor
        public MODFlaggedArticlesViewModel(int user_id, NavigationStore _navigationStore)
        {
            this.userid = user_id;
            this.navigationStore = _navigationStore;
            this.theseRepo = new TheseRepo();
            this.UserRepos = new UserRepos();

            articleIdList = theseRepo.getUnSupervisedArticles(userid);
            articleModels = new List<ArticleModel>();

            foreach (int id in articleIdList)
            {
                articleModels.Add(theseRepo.GetThesisDetails(id));
            }
;
            Articles = new ObservableCollection<MemberArticleViewModel>(
                articleModels.Select(article => new MemberArticleViewModel(userid, navigationStore, article))
            );

            SideBarViewModel = new AdminSideBarViewModel(user_id, _navigationStore);

            _userName = UserRepos.GetuserName(user_id);
            user_profilepic = ByteArrayToImageConverter.LoadImageSourceFromBytes(UserRepos.GetProfilepicFromId(user_id));
            Email = UserRepos.GetuserEmail(user_id);
            User_role = UserRepos.GetUserRole(user_id);

            Role = UserRepos.GetUserRole(user_id);
            if (Role == "Student")
            {
                isUser = true;
                isMember = false;
                SideBarViewModel = new UserSideBarViewModel(user_id, navigationStore);
            }
            if (Role == "Admin")
            {
                isMember = true;
                isUser = false;
                SideBarViewModel = new MemberSideBarViewModel(user_id, navigationStore);
            }
            //side bar
            SideBarViewModel = new AdminSideBarViewModel(user_id, _navigationStore);

        }
    }
}
