using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Models;
using Project.Repos;

namespace Project.Services
{
    public interface ICommentService
    {
       

        public ObservableCollection<Comment> Comments { get; set; }
        void DisplayComment(string Username, string commentText);
        void ReloadComments(int theseId);
    }




    public class CommentService : ICommentService
    {
        private int theseId;
        private readonly CommentRepo _commentRepo;

        public CommentService(CommentRepo commentRepo = null)
        {
            _commentRepo = commentRepo ?? new CommentRepo();
            Comments = new ObservableCollection<Comment>();

        }


        public ObservableCollection<Comment> Comments { get; set; } 

        public void DisplayComment(string Username, string commentText)
        {
            if (Comments == null)
            {
                Comments = new ObservableCollection<Comment>();
            }
            Comments.Add(new Comment { Username = Username, CommentText = commentText });    
         }


        public void ReloadComments(int theseId)
        {
            if (Comments == null)
            {
                Comments = new ObservableCollection<Comment>();
            }
            else
            {
                Comments.Clear();
            }

            // Load comments from repository
            var comments = _commentRepo.LoadTheseComments(theseId);

            if (comments != null)
            {
                foreach (var c in comments)
                {
                    Comments.Add(new Comment
                    {
                        CommentText = c.CommentText,
                        Username = c.Username,
                        TheseId = c.TheseId,
                        user_profilepic = c.user_profilepic,
                        commentId = _commentRepo.GetCommentId(c.CommentText),
                        IsExpanded = false
                    });
                }
            }
        }
    }
}
