using Project.Repos;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class SubmitCommentReportCommand : CommandBase
    {
        private ReportCommentFormViewModel reportFormViewModel;
        private ReportsRepo ReportsRepo;

        public SubmitCommentReportCommand(ReportCommentFormViewModel reportFormViewModel)
        {
            this.reportFormViewModel = reportFormViewModel;
            ReportsRepo = new ReportsRepo();
        }

        public override void Execute(object parameter)
        {
            string finalReason = reportFormViewModel.Reason;
            string selected = reportFormViewModel.SelectedReason;
            if (string.IsNullOrWhiteSpace(finalReason) || string.IsNullOrWhiteSpace(selected))
            {
                // Optional: notify user to pick or type something
                return;
            }

            ReportsRepo.SubmitReport(reportFormViewModel.comment_id, reportFormViewModel.user_id, finalReason, selected, "Comment");
            reportFormViewModel.ClosePopup();
        }
    }
}
