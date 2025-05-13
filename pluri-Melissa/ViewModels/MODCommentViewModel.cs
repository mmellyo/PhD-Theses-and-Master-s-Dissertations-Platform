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
        private readonly CommentRepo _commentRepo;
        private readonly TheseRepo _theseRepo;
        private IReportsRepo _reportsRepo;
        private ViewModelLocator _viewModelLocator;
        private Comment _approvedComment;
        IWindowManager _windowManager;
        private int commentId;
        private int cId;

        public string Email { get; set; }
        public string Username { get; set; }
        public string CommentText { get; set; }
        public int TheseId { get; set; }
        public int UserId { get; }
        public ICommentService CommentService { get; }
        public ObservableCollection<Comment> FlaggedComments { get; set; }



        //commands
       // public ICommand ApproveCommand { get; set; }
       // public ICommand DenyCommand { get; set; }
        public ICommand MODDisplayTheseCommand { get; set; }

        



            //contructor   
            public MODCommentViewModel(IUserSessionService userSession, ICommentService commentService, IWindowManager windowManager, ViewModelLocator viewModelLocator)
            {

                //fields 
                 _userSession = userSession;
                 _userRepos = new UserRepos();
                 _reportsRepo = new ReportsRepo();
                _commentRepo = new CommentRepo();
                _theseRepo = new TheseRepo();


                _windowManager = windowManager;
                _viewModelLocator = viewModelLocator;
                CommentService = commentService;
                _windowManager = windowManager;

                FlaggedComments = new ObservableCollection<Comment>();


                Email = _userSession.Email;
                Username = _userSession.Username;
                UserId = _userRepos.GetUserId(Email);

                LoadFlaggedComments();




            //commands
            MODDisplayTheseCommand = new ViewModelCommand(
            execute: obj =>
            {
                _theseRepo.ShowPdf(2);
                // switch to these with that id
                //_windowManager.CloseWindow();
                //_windowManager.ShowWindow(_viewModelLocator.CommentViewModel);
                //_viewModelLocator.CommentViewModel.TheseId = TheseId;


            }
            // canExecute: obj =>  !string.IsNullOrWhiteSpace(InputVerificationCode) && InputVerificationCode.Length >= 6
            );

        }


        private void LoadFlaggedComments()
        {
            FlaggedComments.Clear();

            foreach (var c in _reportsRepo.LoadFlaggedComments())
            {
                if (c.State == 2)
                    continue; // Skip this cmnt if its state is 2 (approved)

                var comment = new Comment
                {
                                 
                    CommentText = c.CommentText,
                    commentId = _commentRepo.GetCommentId(CommentText),

                    TheseId = c.TheseId,
                    //TEMP
                    //Username = "melly",
                    //UserId = 1,

                    //original
                    Username = this.Username,
                    UserId = c.UserId,
                    State = c.State        
                };

                //each cmnt with its own commands
                comment.ApproveCommand = new ViewModelCommand(
                    execute: obj =>
                    {
                        cId = _commentRepo.GetCommentId(comment.CommentText);

                        // Update comment state in db from 1 (flagged) to 2 (approved)
                        bool success = _commentRepo.UpdateCommentState(cId, 2);

                        MessageBox.Show(success ? "Comment approved successfully!" : "Approving comment failed.");

                        if (success) { 
                        // Display cmnt in theseView
                        CommentService.DisplayComment(comment.Username, comment.CommentText);

                        // Remove from flagged cmnts list
                        FlaggedComments.Remove(comment);
                        }
                    }
                );

                comment.DenyCommand = new ViewModelCommand(
                    execute: obj =>
                    {
                        cId = _commentRepo.GetCommentId(comment.CommentText);
                        // Update comment state in db from 1 (flagged) to 3 (denied)
                        bool success = _commentRepo.UpdateCommentState(cId, 3);
                        MessageBox.Show(success ? "Comment denied successfully!" : "Denying comment failed.");
                        if (success)
                        {
                            // Remove from flagged comments list
                            FlaggedComments.Remove(comment);
                        }
                    }
                );

                FlaggedComments.Add(comment);
            }
        }
    

}



}

