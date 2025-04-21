using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Models;

namespace Project.Services
{
    public interface ICommentService
    {
       

        public ObservableCollection<Comment> Comments { get; set; }
        void AddComment(string email, string commentText);
    }


    internal class CommentService : ICommentService
    {
        public ObservableCollection<Comment> Comments { get; set; } = new();

        public void AddComment(string email, string commentText)
        {
            Comments.Add(new Comment { Email = email ,CommentText = commentText });    
         }
    }
}
