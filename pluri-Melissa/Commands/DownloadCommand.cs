using Project.Repos;
using Project.Stores;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.Commands
{
    public class DownloadCommand : CommandBase
    {
        private NavigationStore navigationStore;
        private int theseId;
        private TheseRepo theseRepo;

        public DownloadCommand(NavigationStore navigationStore, int theseId)
        {
            this.navigationStore = navigationStore;
            this.theseId = theseId;
            this.theseRepo = new TheseRepo();
        }

        public override void Execute(object parameter)
        {
            byte[] file = theseRepo.loadFileByte(theseId);
            if (file == null || file.Length == 0)
            {
                MessageBox.Show("The file is empty or could not be loaded.");
                return;
            }

            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = $"article_{theseId}",
                DefaultExt = ".pdf", // adjust depending on file type
                Filter = "PDF documents (*.pdf)|*.pdf|All files (*.*)|*.*"
            };

            bool? result = saveDialog.ShowDialog();

            if (result == true)
            {
                File.WriteAllBytes(saveDialog.FileName, file);
                MessageBox.Show("File downloaded successfully!");
            }
        }
    }
}
