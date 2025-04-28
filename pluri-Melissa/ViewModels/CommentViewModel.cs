using System;
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

namespace Project.ViewModels
{
    public class CommentViewModel : ViewModelBase
    {
        private readonly MLContext _mlContext;

       

        private readonly UserModel _userModel;
        private readonly IUserSessionService _userSession;
        private readonly UserRepos _userRepos;
        private readonly CommentRepo _commentRepo;
        private readonly ReportsRepo _reportRepo;
        private readonly EmailVerificationRepo _emailVerificationRepo;
        private readonly IWindowManager _windowManager;
        private readonly ViewModelLocator _viewModelLocator;

        public ICommentService CommentService { get; }

        private string _result;
        private string _comment;

        // getters / setters

        public string Email { get; set; }
        public int UserId { get; }
        public int TheseId { get; set; }
        public string Username { get; }

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

        // commands
        public ICommand AddCommentCommand { get; } 

        // constructor
        public CommentViewModel(IUserSessionService userSession, ICommentService commentService, IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {
            //fields
            _userModel = new UserModel();
            _userSession = userSession;
            _userRepos = new UserRepos();
            _commentRepo = new CommentRepo();
            _reportRepo = new ReportsRepo();

            _emailVerificationRepo = new EmailVerificationRepo();

            _windowManager = windowManager;
            _viewModelLocator = viewModelLocator;
            CommentService = commentService;
            _mlContext = new MLContext();
            

            Email = _userSession.Email;
           // Username = _userRepos.GetUsernameFromEmail(Email);
            //  UserId = _userRepos.GetUserId(Email);
            TheseId = 1;

            //***************************** TEST
          Username = _userRepos.GetUsernameFromEmail("melly@etu.usthb.dz");
            UserId = _userRepos.GetUserId("melly@etu.usthb.dz");

            AddCommentCommand = new ViewModelCommand(
                execute: obj =>
                {
                   // try
                   // {

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





                        if ( sentiment == "Positive")
                        {

                            //save cmnt is db
                            bool success = _commentRepo.AddCommentInDb(TheseId, Comment, UserId);
                            MessageBox.Show(success ? "Comment added successfully!" : "adding comment failed.");

                            //display cmnt
                            CommentService.DisplayComment(Username, Comment);


                        } else if (sentiment == "Negative")
                        {
                            MessageBox.Show("This comment may include restricted content. It will only be published upon approval by an administrator.", "Negative comment", MessageBoxButton.OK, MessageBoxImage.Error);
                            
                            //save cmnt in db (reports)
                            _reportRepo.ReportComment(Comment, null);
                       


                            //temp switching to MODcmnt
                            _windowManager.CloseWindow();
                            _windowManager.ShowWindow(_viewModelLocator.MODCommentViewModel);

                            // _emailVerificationRepo.SendCommentToAdmin(Username, Comment, TheseId);
                        }





                   // }
                    //catch (Exception ex)
                    //{
                     //   Result = $"Error: {ex.Message}";
                    //}
                },
                canExecute: obj => true
                //!string.IsNullOrEmpty(Comment) // Only enable the command if there's text to analyze
            );
        }
    }
}
    
