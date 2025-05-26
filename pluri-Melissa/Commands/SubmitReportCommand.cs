using Project.Models;
using Project.Repos;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class SubmitReportCommand : CommandBase
    {
        private ReportFormViewModel reportFormViewModel;
        private ReportsRepo ReportsRepo;

        public SubmitReportCommand (ReportFormViewModel reportFormViewModel)
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

            ReportsRepo.SubmitReport(reportFormViewModel.article_id, reportFormViewModel.user_id, finalReason, selected, "Article");
            reportFormViewModel.ClosePopup();
        }
    }
}
