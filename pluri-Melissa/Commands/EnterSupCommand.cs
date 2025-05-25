using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class EnterSupCommand : CommandBase
    {
        private UploadViewModel _viewModel;

        public EnterSupCommand(UploadViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public override void Execute(object parameter)
        {
            _viewModel.supervisors.Add(_viewModel.Encadrant);
            _viewModel.Encadrant = string.Empty;
        }
    }
}

