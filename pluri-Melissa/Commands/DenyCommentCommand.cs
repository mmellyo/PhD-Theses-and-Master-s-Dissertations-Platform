using Project.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class DenyCommentCommand : CommandBase
    {
        private int _comment_id;
        private CommentRepo _repo;
        public DenyCommentCommand(int comment_id)
        {
            _comment_id = comment_id;
            _repo = new CommentRepo();

        }

        public override void Execute(object parameter)
        {
            _repo.UpdateCommentState(_comment_id, 3);
        }
    }
}
