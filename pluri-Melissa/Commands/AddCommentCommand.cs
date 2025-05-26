using Microsoft.ML;
using MySqlX.XDevAPI.Common;
using Project.Models;
using Project.Repos;
using Project.Services;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.Commands
{
    public class AddCommentCommand : CommandBase
    {

        private readonly MLContext _mlContext;
        private ThesePageViewModel _pageViewModel;
        private CommentRepo _commentRepo;
        private UserRepos _userRepo;
        private ReportsRepo _reportsRepo;

        public AddCommentCommand(ThesePageViewModel thesePageViewModel)
        {
            _mlContext = new MLContext();
            _pageViewModel = thesePageViewModel;
            _commentRepo = new CommentRepo();
            _userRepo = new UserRepos();
            _reportsRepo = new ReportsRepo();
        }
        public override void Execute(object parameter)
        {
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
                SentimentText = _pageViewModel.Comment
            });

            string sentiment = prediction.Prediction ? "Positive" : "Negative";
            _pageViewModel.Result = $" The comment is: {sentiment}\nScore: {prediction.Score:F2}\nProbability: {prediction.Probability:P2}";


            Console.WriteLine($"detecting finished");










            if (sentiment == "Positive")
            {
                //display in console
                Console.WriteLine($"comment is : {_pageViewModel.Comment} and :{sentiment} ");

                //save cmnt in db with state (0) => Positive
                bool success = _commentRepo.AddCommentInDb(_pageViewModel.theseId, _pageViewModel.Comment, _pageViewModel.userid, 0);

                //i should get id as a return from the function that saved the cmnt
                int CommentId = _commentRepo.GetCommentId(_pageViewModel.Comment);

                MessageBox.Show(success ? "Comment added successfully!" : "adding comment failed.");



                //display cmnt
                var newComment = new Comment
                {
                    UserId = _pageViewModel.userid,
                    CommentText = _pageViewModel.Comment,
                    Username = _pageViewModel.Username,
                    commentId = CommentId,
                    TheseId = _pageViewModel.theseId,
                    user_profilepic = _userRepo.GetProfilepicFromId(_pageViewModel.userid),
                    IsExpanded = false
                };
                /*try
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
                }*/


                _commentRepo.AddCommentInDb(newComment.TheseId, newComment.CommentText, newComment.UserId, newComment.State);
                _pageViewModel.Comment = string.Empty;
            }
            else if (sentiment == "Negative")
            {
                //display in console
                Console.WriteLine($"comment is : {_pageViewModel.Comment} and :{sentiment} ");

                MessageBox.Show("Ce commentaire peut contenir du contenu restreint. Il ne sera publié qu'après approbation par un administrateur.", "commentaire negatif", MessageBoxButton.OK, MessageBoxImage.Error);

                //save cmnt in db (comment) with state (1) => Negative
                _commentRepo.AddCommentInDb(_pageViewModel.theseId, _pageViewModel.Comment, _pageViewModel.userid, 1);

                //get cmnt id
                int CommentId = _commentRepo.GetCommentId(_pageViewModel.Comment);

                //save cmnt in db (reports)
                _reportsRepo.ReportComment(CommentId, null);

                _pageViewModel.Comment = string.Empty;

                //temp switching to MODcmnt
                //_windowManager.CloseWindow();
                // _emailVerificationRepo.SendCommentToAdmin(Username, Comment, TheseId);

            }
        }
    }
}
