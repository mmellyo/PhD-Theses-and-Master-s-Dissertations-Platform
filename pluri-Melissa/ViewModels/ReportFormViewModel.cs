using Project.Commands;
using Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.ViewModels
{
    public class ReportFormViewModel : ViewModelBase
    {
        public int article_id;
        public int user_id;

        private string _selectedReason;
        public string SelectedReason
        {
            get => _selectedReason;
            set
            {
                _selectedReason = value;
                OnPropertyChanged(nameof(SelectedReason));
            }
        }

        private string _reason;
        public string Reason
        {
            get => _reason;
            set
            {
                _reason = value;
                OnPropertyChanged(nameof(Reason));
            }
        }

        public ICommand Report { get; }
        public ICommand Cancel { get; }

        private Action _onClose;

        public ReportFormViewModel(Action onClose, int user_id, int article_id)
        {
            _onClose = onClose;
            this.user_id = user_id;
            this.article_id = article_id;

            Report = new SubmitReportCommand(this);
            Cancel = new CancelReportCommand(this);
        }


        public void ClosePopup()
        {
            _onClose?.Invoke();
        }
    }
}
