using Project.Models;
using Project.Repos;
using Project.Stores;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Project.ViewModels
{
    public class MODManuallyComments : ViewModelBase
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


        //side bar
        public object SideBarViewModel { get; }

        //top bar
        private UserRepos UserRepos;

        //Items Contorl
        public ObservableCollection<CommentsViewModel> ModManComments { get; set; }
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

        //constructor
        public MODCommentViewModel(int user_id, NavigationStore _navigationStore)
        {
            this.userid = user_id;
            this.navigationStore = _navigationStore;
            this.theseRepo = new TheseRepo();
            this.UserRepos = new UserRepos();
            this._commentRepo = new CommentRepo();

            //items control
            ModManComments = new ObservableCollection<CommentsViewModel>();
            LoadComments();


            //side bar
            SideBarViewModel = new MemberSideBarViewModel(user_id, navigationStore);

            //top bar
            _userName = UserRepos.GetuserName(user_id);
            user_profilepic = ByteArrayToImageConverter.LoadImageSourceFromBytes(UserRepos.GetProfilepicFromId(user_id));
            Email = UserRepos.GetuserEmail(user_id);
            User_role = UserRepos.GetUserRole(user_id);

        }


        public void LoadComments()
        {
            CommentModels = _commentRepo.LoadManFlaggedComments();

            ModComments.Clear(); // Clear the existing items

            foreach (var comment in CommentModels)
            {
                ModComments.Add(new CommentsViewModel(comment, navigationStore, userid, comment.user_id, this));
            }
        }

    }
}
}
