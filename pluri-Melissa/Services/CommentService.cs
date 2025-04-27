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
        void DisplayComment(string Username, string commentText);
    }


    internal class CommentService : ICommentService
    {
        private int theseId;

        public ObservableCollection<Comment> Comments { get; set; } = new();

        public void DisplayComment(string Username, string commentText)
        {
            Comments.Add(new Comment { Username = Username, CommentText = commentText, TheseId = theseId });    
         }
    }
}
