using Project.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.ViewModels
{
    public class ReportCommentFormViewModel : ViewModelBase
    {
        public int comment_id;
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

        public ReportCommentFormViewModel(Action onClose, int user_id, int comment_id)
        {
            _onClose = onClose;
            this.user_id = user_id;
            this.comment_id = comment_id;

            Report = new SubmitCommentReportCommand(this);
            Cancel = new CancelCommentReportCommand(this);
        }


        public void ClosePopup()
        {
            _onClose?.Invoke();
        }
    }
}
