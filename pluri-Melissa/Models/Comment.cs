using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Org.BouncyCastle.Asn1.X509;

namespace Project.Models
{
    public class Comment : INotifyPropertyChanged
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string CommentText { get; set; }
        public int TheseId { get; set; }
        public int UserId { get; set; }
        public int commentId { get; set; }

        private byte[] _userProfilePic;
        public byte[] user_profilepic
        {
            get => _userProfilePic;
            set
            {
                if (_userProfilePic != value)
                {
                    _userProfilePic = value;
                    OnPropertyChanged();
                }
            }
        }
        public int State { get; set; } //1 = flagged, 0 = positive,  2 = approved, 3 = denied

        // Commands for UI binding
        public ICommand ApproveCommand { get; set; }
        public ICommand DenyCommand { get; set; }
        public ICommand CommentUsernameClickCommand { get; set; }



        // Properties for expansion/collapse functionality
        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DisplayText));
                    OnPropertyChanged(nameof(ToggleButtonText));
                }
            }
        }
        // The text to display (shortened or full based on IsExpanded)
        public string DisplayText
        {
            get
            {
                if (IsExpanded || CommentText.Length <= 100)
                    return CommentText;

                return CommentText.Substring(0, 100) + "...";
            }
        }

        // The text for the toggle button
        public string ToggleButtonText => IsExpanded ? "See less" : "See more";

        // Property to determine if the toggle button should be visible
        public bool ShowToggleButton => CommentText.Length > 100;








        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
