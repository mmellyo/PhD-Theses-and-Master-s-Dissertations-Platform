using Project.Stores;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class ReportCommentCommand : CommandBase
    {
        private NavigationStore _navigationStore;
        private CommentsViewModel _CommentsViewModel;

        public ReportCommentCommand(NavigationStore navigationStore, CommentsViewModel CommentsViewModel)
        {
            _navigationStore = navigationStore;
            _CommentsViewModel = CommentsViewModel;

        }

        public override void Execute(object parameter)
        {
            _CommentsViewModel.ReportViewModel = new ReportCommentFormViewModel(() => _CommentsViewModel.IsReportCommentPopupVisible = false, _CommentsViewModel.user_id, _CommentsViewModel.commentid);
            _CommentsViewModel.IsReportCommentPopupVisible = true;
        }
    }
}
