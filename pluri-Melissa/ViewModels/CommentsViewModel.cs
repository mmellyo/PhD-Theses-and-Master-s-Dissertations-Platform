using Org.BouncyCastle.Asn1.X509;
using Project.Commands;
using Project.Models;
using Project.Stores;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Project.ViewModels
{
    public class CommentsViewModel : ViewModelBase
    {
        private NavigationStore _navigationStore;
        private CommentModel comment { get;  }

        public Byte[] CommentorProfilePic => comment.user_image;

        public string CommentUsername => comment.user_name;

        public string DisplayText => comment.content;
        
        
        public ICommand CommentUsernameClickCommand { get; set; }
        public ICommand ToggleCommentCommand {  get; set; }


        //modding
        public ICommand DenyCommentCommand { get; set; }
        public ICommand ApproveCommentCommand { get; set; }

        public CommentsViewModel(CommentModel comment, NavigationStore navigationStore, int user_id) 
        {
            this.comment = comment;
            _navigationStore = navigationStore;

            CommentUsernameClickCommand = new NavigateCommand<UserProfileViewModel>(_navigationStore, () => new UserProfileViewModel(_navigationStore, user_id));
            ToggleCommentCommand = new ViewModelCommand(
                execute: param =>
                {
                    if (param is Comment comment)
                    {
                        comment.IsExpanded = !comment.IsExpanded;
                    }
                },
                canExecute: param => param is Comment
            );
        }

        public CommentsViewModel(CommentModel comment, NavigationStore navigationStore, int user_id, int poster_id)
        {
            this.comment = comment;
            _navigationStore = navigationStore;

            CommentUsernameClickCommand = new NavigateCommand<UserProfileViewModel>(_navigationStore, () => new UserProfileViewModel(_navigationStore, user_id));
            ToggleCommentCommand = new ViewModelCommand(
                execute: param =>
                {
                    if (param is Comment comment)
                    {
                        comment.IsExpanded = !comment.IsExpanded;
                    }
                },
                canExecute: param => param is Comment
            );

            ApproveCommentCommand = new ApproveCommentCommand(comment.comment_id);
            DenyCommentCommand = new DenyCommentCommand(comment.comment_id);

        }

    }
}
