using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Mysqlx.Prepare;
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
        IReportsRepo _reportsRepo;
        private ViewModelLocator _viewModelLocator;
        IWindowManager _windowManager;

        public ICommentService CommentService { get; }

        public string Email { get; set; }
        public string Username { get; set; }
        public string CommentText { get; set; }
        public int TheseId { get; set; }

        public ObservableCollection<Comment> FlaggedComments { get; set; }









        //commands
        public ICommand ApproveCommand { get; set; }
        public ICommand DenyCommand { get; set; }
        public ICommand DisplayTheseCommand { get; set; }

        






            //contructor   
            public MODCommentViewModel(IUserSessionService userSession, ICommentService commentService, IWindowManager windowManager, ViewModelLocator viewModelLocator)
            {

                //fields 
                 _userSession = userSession;
                 _userRepos = new UserRepos();
                 _reportsRepo = new ReportsRepo();

                _windowManager = windowManager;
                _viewModelLocator = viewModelLocator;
                CommentService = commentService;
                _windowManager = windowManager;

                FlaggedComments = new ObservableCollection<Comment>();




                Email = _userSession.Email;
                Username = _userSession.Username;

                foreach (var c in _reportsRepo.LoadFlaggedComments())
                {
                    FlaggedComments.Add(new Comment
                    {
                        CommentText = c.CommentText,
                        TheseId = c.TheseId,
                        Username = this.Username
                    });
                }






            //commands
            DisplayTheseCommand = new ViewModelCommand(
            execute: obj =>
            {

                //temp switching to MODcmnt
                _windowManager.CloseWindow();
                _windowManager.ShowWindow(_viewModelLocator.MODCommentViewModel);

            }
            // canExecute: obj =>  !string.IsNullOrWhiteSpace(InputVerificationCode) && InputVerificationCode.Length >= 6
            );



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

