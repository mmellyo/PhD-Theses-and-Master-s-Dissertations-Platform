using Project.Repos;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class EnterkeywordCommand : CommandBase
    {
        private UploadViewModel _viewModel;
        
        public EnterkeywordCommand(UploadViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public override void Execute(object parameter)
        {
            _viewModel.keywords.Add(_viewModel.Keywords);
            _viewModel.Keywords = string.Empty;
        }
    }
}
