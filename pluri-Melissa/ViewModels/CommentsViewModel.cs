﻿using Org.BouncyCastle.Asn1.X509;
using Project.Commands;
using Project.Models;
using Project.Repos;
using Project.Stores;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Project.ViewModels
{
    public class CommentsViewModel : ViewModelBase
    {
        private MODCommentViewModel viewModel;
        private NavigationStore _navigationStore;
        private CommentModel comment { get;  }
        private TheseRepo theseRepo;
        private UserRepos UserRepos;
        private ReportsRepo reportsRepo;
        public Byte[] CommentorProfilePic => UserRepos.GetProfilepicFromId(comment.user_id);

        public string CommentUsername => UserRepos.GetuserName(comment.user_id);

        public int commentid => comment.comment_id;
        public int user_id;
        public string DisplayText => comment.content;

        public string CommentArticle => theseRepo.GetTitleById(comment.article_id);
        
        
        public ICommand CommentUsernameClickCommand { get; set; }
        public ICommand ToggleCommentCommand {  get; set; }


        //modding
        public ICommand DenyCommentCommand { get; set; }
                    

        public ICommand ApproveCommentCommand { get; set; }

        //profile
        public ICommand CommentArticleClickCommand { get; set; }
        //for article page
        private bool _isReportPopupVisible;
        public bool IsReportCommentPopupVisible
        {
            get => _isReportPopupVisible;
            set
            {
                _isReportPopupVisible = value;
                OnPropertyChanged(nameof(IsReportCommentPopupVisible));
            }
        }

        private ReportCommentFormViewModel _reportViewModel;
        public ReportCommentFormViewModel ReportViewModel
        {
            get => _reportViewModel;
            set
            {
                _reportViewModel = value;
                OnPropertyChanged(nameof(ReportViewModel));
            }
        }
        public ICommand ReportComment { get; set; }
        public CommentsViewModel(CommentModel comment, NavigationStore navigationStore, int user_id) 
        {
            this.user_id = user_id;
            this.comment = comment;
            _navigationStore = navigationStore;
            UserRepos = new UserRepos();
            CommentUsernameClickCommand = new NavigateCommand<UserProfileViewModel>(_navigationStore, () => new UserProfileViewModel(_navigationStore, user_id));
            ToggleCommentCommand = new ViewModelCommand(
                execute: param =>
                {
                    if (param is Comment comment)
                    {
                        comment.IsExpanded = !comment.IsExpanded;
                    }
                },
                canExecute: param => param is Comment
            );

            ReportComment = new ReportCommentCommand(navigationStore, this);
        }
        //for profile
        public CommentsViewModel(int user_id, CommentModel comment, NavigationStore navigationStore)
        {
            this.comment = comment;
            this.theseRepo = new TheseRepo();
            this.UserRepos = new UserRepos();
            _navigationStore = navigationStore;

            CommentUsernameClickCommand = new NavigateCommand<UserProfileViewModel>(_navigationStore, () => new UserProfileViewModel(_navigationStore, user_id));
            CommentArticleClickCommand = new NavigateCommand<ThesePageViewModel>(_navigationStore, () => new ThesePageViewModel(navigationStore, user_id, comment.article_id));
            
            ToggleCommentCommand = new ViewModelCommand(
                execute: param =>
                {
                    if (param is Comment comment)
                    {
                        comment.IsExpanded = !comment.IsExpanded;
                    }
                },
                canExecute: param => param is Comment
            );
        }

        //for admin 
        public CommentsViewModel(CommentModel comment, NavigationStore navigationStore, int user_id, int poster_id, MODCommentViewModel viewModel)
        {
            this.comment = comment;
            reportsRepo = new ReportsRepo();
            UserRepos = new UserRepos();
            _navigationStore = navigationStore;
            this.viewModel = viewModel;

            CommentUsernameClickCommand = new NavigateCommand<UserProfileViewModel>(_navigationStore, () => new UserProfileViewModel(_navigationStore, user_id));
            ToggleCommentCommand = new ViewModelCommand(
                execute: param =>
                {
                    if (param is Comment comment)
                    {
                        comment.IsExpanded = !comment.IsExpanded;
                    }
                },
                canExecute: param => param is Comment
            );

            ApproveCommentCommand = new ApproveCommentCommand(comment.comment_id, viewModel);
            DenyCommentCommand = new DenyCommentCommand(comment.comment_id, viewModel);
            
        }

    }
}
