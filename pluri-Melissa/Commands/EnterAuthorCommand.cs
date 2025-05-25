using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class EnterAuthorCommand : CommandBase
    {
        private UploadViewModel _viewModel;

        public EnterAuthorCommand(UploadViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public override void Execute(object parameter)
        {
            _viewModel.authors.Add(_viewModel.Writers);
            _viewModel.Writers = string.Empty;
        }
    }
}
