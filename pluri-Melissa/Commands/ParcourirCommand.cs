using Microsoft.Win32;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class ParcourirCommand : CommandBase
    {
        private UploadViewModel _viewModel;

        public ParcourirCommand(UploadViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            OpenFileDialog fileDialogue = new OpenFileDialog();
            fileDialogue.Filter = "PDF files | *.pdf";

            bool? success =fileDialogue.ShowDialog();
            if (success == true) 
            { 
                string filePath = fileDialogue.FileName;
                Byte[] selectedFile = File.ReadAllBytes(filePath);
                
                
                    _viewModel.FileContent = selectedFile;
                
            
            }

        }
    }
}
