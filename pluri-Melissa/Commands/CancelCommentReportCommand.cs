using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class CancelCommentReportCommand : CommandBase
    {
        private ReportCommentFormViewModel viewModel;

        public CancelCommentReportCommand(ReportCommentFormViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            viewModel.ClosePopup();
        }
    }
}
