﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using System.Windows.Input;
using Project.Models;
using Project.Services;
using Project.Repos;
using System.Windows;
using System.Collections.ObjectModel;
using Project.Utils;
using MySqlX.XDevAPI.Common;
using SharpVectors.Dom;
using Comment = Project.Models.Comment;

namespace Project.ViewModels
{
    public class CommentViewModel : ViewModelBase
    {
        private readonly MLContext _mlContext;

       

        private readonly UserModel _userModel;
        private readonly IUserSessionService _userSession;
        private readonly UserRepos _userRepos;
        private readonly TheseRepo _theseRepo;
        private readonly CommentRepo _commentRepo;
        private readonly ReportsRepo _reportRepo;
        private readonly EmailVerificationRepo _emailVerificationRepo;
        private readonly IWindowManager _windowManager;
        private readonly ViewModelLocator _viewModelLocator;

        public ICommentService CommentService { get; }
        private readonly ITheseService _theseService;

        private string _result;
        private string _comment;

        // getters / setters
        public string Email { get; set; }
        public string CommentText { get; set; }
        public int UserId { get; set; }
        public int TheseId { get; set; }
        public string Username { get; set; }
        public byte[] user_profilepic { get; set; }

        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
        }


        //these fields
        private string _nomEncadrant;
        public string NomEncadrant
        {
            get => _nomEncadrant;
            set
            {
                _nomEncadrant = value;
                OnPropertyChanged(nameof(NomEncadrant));
            }
        }

        private string _faculte;
        public string Faculte
        {
            get => _faculte;
            set
            {
                _faculte = value;
                OnPropertyChanged(nameof(Faculte));
            }
        }

        public string _nomAuteur;
        public string NomAuteur
        {
            get => _nomAuteur;
            set
            {
                _nomAuteur = value;
                OnPropertyChanged(nameof(NomAuteur));
            }
        }



        // commands
        public ICommand AddCommentCommand { get; }
        public ICommand ToggleCommentCommand { get; }



        // constructor
        public CommentViewModel(ITheseService theseService, IUserSessionService userSession, ICommentService commentService, IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {
            //fields
            _userModel = new UserModel();
            _userSession = userSession;
            _userRepos = new UserRepos();
            _commentRepo = new CommentRepo();
            _reportRepo = new ReportsRepo();
            _theseRepo = new TheseRepo();
            _emailVerificationRepo = new EmailVerificationRepo();
            _windowManager = windowManager;
            _viewModelLocator = viewModelLocator;
            CommentService = commentService;
            _mlContext = new MLContext();
            _theseService = theseService;

            // Ensure Comments collection is initialized
            if (CommentService.Comments == null)
            {
                Console.WriteLine("comment collection is null - Initializing Comments collection");
                CommentService.Comments = new ObservableCollection<Comment>();
            }


            //init user info
            Email = !string.IsNullOrEmpty(_viewModelLocator.LoginViewModel.LoginEmail)
                                        ? _viewModelLocator.LoginViewModel.LoginEmail
                                        : _viewModelLocator.SignUpViewModel.Email;
            TheseId = _theseService.TheseId;



            // commands
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



            AddCommentCommand = new ViewModelCommand(
                execute: obj =>
                {
                    Console.WriteLine($"add cmnts clicked");

                    var data = _mlContext.Data.LoadFromTextFile<CommentData>("MLModel/CommentModel.csv", hasHeader: true, separatorChar: ',', allowQuoting: true);


                    if (data == null)
                    {
                        throw new InvalidOperationException("Data could not be loaded properly.");
                    }


                    var pipeline = _mlContext.Transforms.Expression("Label", "(x) => x == 1 ? true : false", "Sentiment")
                    .Append(_mlContext.Transforms.Text.FeaturizeText("Features", nameof(CommentData.SentimentText)))
                        .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));


                    var model = pipeline.Fit(data);


                    var predictionEngine = _mlContext.Model.CreatePredictionEngine<CommentData, CommentPrediction>(model);


                    var prediction = predictionEngine.Predict(new CommentData
                    {
                        SentimentText = Comment
                    });

                    string sentiment = prediction.Prediction ? "Positive" : "Negative";
                    Result = $" The comment is: {sentiment}\nScore: {prediction.Score:F2}\nProbability: {prediction.Probability:P2}";


                    Console.WriteLine($"detecting finished");










                    if (sentiment == "Positive")
                    {
                        //display in console
                        Console.WriteLine($"comment is : {Comment} and :{sentiment} ");

                        //save cmnt in db with state (0) => Positive
                        bool success = _commentRepo.AddCommentInDb(TheseId, Comment, UserId, 0);
                        
                        //i should get id as a return from the function that saved the cmnt
                        int CommentId = _commentRepo.GetCommentId(Comment);

                        MessageBox.Show(success ? "Comment added successfully!" : "adding comment failed.");



                        //display cmnt
                        var newComment = new Comment
                        {
                            UserId = UserId,
                            CommentText = Comment,
                            Username = Username,
                            commentId = CommentId,
                            TheseId = this.TheseId,
                            user_profilepic = user_profilepic,
                            IsExpanded = false
                        };
                        try
                        {
                            newComment.CommentUsernameClickCommand = new ViewModelCommand(
                            execute: obj =>
                            {
                                Console.WriteLine("CommentUsernameClickCommand CLICKEDDDDDDDDDDDDDDDDDD");
                                Console.WriteLine("ypu email is  " + Email);

                                int uId = newComment.UserId;
                                if (uId == _userRepos.GetUserId(Email))
                                {
                                    Console.WriteLine("You view your own profile");
                                    _windowManager.CloseWindow();
                                    _windowManager.ShowWindow(_viewModelLocator.MyProfileViewModel);

                                    return;
                                }
                            }
                            );
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error creating command: {ex.Message}");
                        }


                        CommentService.Comments.Add(newComment);
                        Comment = string.Empty;
                    }

                    else if (sentiment == "Negative")
                    {
                        //display in console
                        Console.WriteLine($"comment is : {Comment} and :{sentiment} ");

                        MessageBox.Show("This comment may include restricted content. It will only be published upon approval by an administrator.", "Negative comment", MessageBoxButton.OK, MessageBoxImage.Error);

                        //save cmnt in db (comment) with state (1) => Negative
                        _commentRepo.AddCommentInDb(TheseId, Comment, UserId, 1);

                        //get cmnt id
                        int CommentId = _commentRepo.GetCommentId(Comment);

                        //save cmnt in db (reports)
                        _reportRepo.ReportComment(CommentId, null);



                        //temp switching to MODcmnt
                        //_windowManager.CloseWindow();
                        _windowManager.ShowWindow(_viewModelLocator.MODCommentViewModel);

                        // _emailVerificationRepo.SendCommentToAdmin(Username, Comment, TheseId);
                    }

                }
            );




        }

        public void InitializeWithTheseId(int theseId)
        {
            TheseId = theseId;
            NomAuteur = _theseRepo.GetNomAuteurFromTheseId(theseId);
            NomEncadrant = _theseRepo.GetNomEncadrantFromTheseId(theseId);
            //Faculte = _theseRepo.GetFaculteFromTheseId(theseId);
            _theseService.TheseId = theseId;

            Console.WriteLine("***************************** InitializeWithTheseId *******************");
            Console.WriteLine("TheseId that is recieving from THESE SERVICE to COMMENTVM is: " + TheseId);
            Console.WriteLine("Email theseview: " + Email);
            Console.WriteLine("Username theseview: " + Username);
            Console.WriteLine("UserId theseview: " + UserId);

            LoadComments(theseId);
        }

        public void InitializeWithUserId(int userId)
        {
            UserId = userId;
            _userSession.UserId = userId;
            user_profilepic = _userRepos.GetProfilepicFromId(UserId);
            Username = _userRepos.GetUsernameFromEmail(Email);

            Console.WriteLine("***************************** InitializeWithUserId *******************");
            Console.WriteLine("UserId that is recieving from login to COMMENTVM is: " + UserId);
        }

        private void LoadComments(int TheseId)
        {
            Console.WriteLine("loadcomments function is called");
            Console.WriteLine("loading comments from these number :" + TheseId);

            try
            {
                // Clear both collections
                CommentService.Comments.Clear();
                Console.WriteLine("Cleared both comment collections");

                // Load from DB
                var comments = _commentRepo.LoadTheseComments(TheseId);
                Console.WriteLine("Loaded comments from repo");

                if (comments != null)
                {
                    foreach (var c in comments)
                    {
                        var comment = new Comment
                        {
                            UserId = c.UserId,
                            CommentText = c.CommentText,
                            Username = c.Username,
                            TheseId = c.TheseId,
                            user_profilepic = c.user_profilepic,
                            commentId = _commentRepo.GetCommentId(c.CommentText),
                            IsExpanded = false
                        };
                        try
                        {
                            comment.CommentUsernameClickCommand = new ViewModelCommand(
                            execute: obj =>
                            {
                                Console.WriteLine("CommentUsernameClickCommand CLICKEDDDDDDDDDDDDDDDDDD");
                                Console.WriteLine("ypu email is  " +Email);

                                int uId = comment.UserId;
                                if (uId == _userRepos.GetUserId(Email)) 
                                {
                                    Console.WriteLine("You view your own profile");
                                    _windowManager.CloseWindow();
                                    _windowManager.ShowWindow(_viewModelLocator.MyProfileViewModel);

                                    return;
                                }

                                _windowManager.CloseWindow();
                                _viewModelLocator.UserProfileViewModel.InitializeWithUserId(uId);
                                Console.WriteLine("UserId that is sending from COMMENTVM to USERPROFILE is: " + uId);  //works : get id of clicked cmnt
                                _windowManager.ShowWindow(_viewModelLocator.UserProfileViewModel);
                                
                            }
                            );
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error creating command: {ex.Message}");
                        }
                        

                        CommentService.Comments.Add(comment);

                    }

                    Console.WriteLine($"Loaded {CommentService.Comments.Count} comments successfully");
                }
                else
                {
                    Console.WriteLine("No comments found for this these");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading comments: {ex.Message}");
                MessageBox.Show($"Failed to load comments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
    
