using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services
{
    public interface ICommentService
    {
        public ObservableCollection<string> Comments { get; set; }
        void AddComment(string comment);
    }


    internal class CommentService : ICommentService
    {
        public ObservableCollection<string> Comments { get; set; } = new();

        public void AddComment(string comment)
        {
            Comments.Add(comment);    
         }
    }
}
