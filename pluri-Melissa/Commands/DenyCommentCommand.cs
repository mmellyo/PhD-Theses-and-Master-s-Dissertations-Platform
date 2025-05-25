using Project.Repos;
using Project.ViewModels;
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
        private MODCommentViewModel viewModel;
        public DenyCommentCommand(int comment_id, MODCommentViewModel viewModel)
        {
            _comment_id = comment_id;
            _repo = new CommentRepo();
            this.viewModel = viewModel;

        }

        public override void Execute(object parameter)
        {
            _repo.UpdateCommentState(_comment_id, 3);
            viewModel.LoadComments();
        }
    }
}
