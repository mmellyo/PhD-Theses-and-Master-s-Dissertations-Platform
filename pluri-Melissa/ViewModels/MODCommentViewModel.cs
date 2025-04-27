using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Project.Models;
using Project.Repos;
using Project.Services;

namespace Project.ViewModels
{
    public class MODCommentViewModel : ViewModelBase
    {
        
        //fields
        private IUserSessionService _userSession;
        private UserRepos _userRepos;

        public string Email { get; set; }
        public string Username { get; set; }
        public string CommentText { get; set; }
        public int TheseId { get; set; }

        public ObservableCollection<Comment> FlaggedComments { get; set; }









        //commands
        public ICommand ApproveCommand { get; set; }
        public ICommand DenyCommand { get; set; }







            //contructor   
            public MODCommentViewModel(IUserSessionService userSession, ICommentService commentService, IWindowManager windowManager, ViewModelLocator viewModelLocator)
            {

                //fields 
                 _userSession = userSession;
                 _userRepos = new UserRepos();

                 FlaggedComments = new ObservableCollection<Comment>();




                Email = _userSession.Email;
                Username = _userRepos.GetUsernameFromEmail(Email);


            // Example
            FlaggedComments.Add(new Comment
                {
                    Username= this.Username,
                    CommentText = "This is a negative commentTTTTT!",
                    TheseId = 1,

                });






            //commands
            ApproveCommand = new ViewModelCommand(
            execute: obj =>
            {
                

                
            }
            // canExecute: obj =>  !string.IsNullOrWhiteSpace(InputVerificationCode) && InputVerificationCode.Length >= 6
            );






            DenyCommand = new ViewModelCommand(
            execute: obj =>
            {



            }
            // canExecute: obj =>  !string.IsNullOrWhiteSpace(InputVerificationCode) && InputVerificationCode.Length >= 6
            );


        }

    }


}

