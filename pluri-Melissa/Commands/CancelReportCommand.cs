using Project.View.userControls;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class CancelReportCommand : CommandBase
    {
        private ReportFormViewModel viewModel;

        public CancelReportCommand (ReportFormViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            viewModel.ClosePopup();
        }
    }
}
