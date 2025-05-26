using Microsoft.ML;
using Project.Models;
using Project.Repos;
using Project.Services;
using Project.Stores;
using Project.ViewModels;
using System;
using System.Windows;

namespace Project.Commands
{
    public class AddCommentCommand : CommandBase
    {
        private readonly MLContext _mlContext;
        private readonly ThesePageViewModel _pageViewModel;
        private readonly CommentRepo _commentRepo;
        private readonly UserRepos _userRepo;
        private readonly ReportsRepo _reportsRepo;
        private readonly NavigationStore _navigationStore;

        public AddCommentCommand(ThesePageViewModel viewModel, NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _mlContext = new MLContext();
            _pageViewModel = viewModel;
            _commentRepo = new CommentRepo();
            _userRepo = new UserRepos();
            _reportsRepo = new ReportsRepo();
        }

        public override void Execute(object parameter)
        {
            var data = _mlContext.Data.LoadFromTextFile<CommentData>(
                "MLModel/CommentModel.csv",
                hasHeader: true,
                separatorChar: ',',
                allowQuoting: true);

            if (data == null)
                throw new InvalidOperationException("Data could not be loaded properly.");

            var pipeline = _mlContext.Transforms.Expression("Label", "(x) => x == 1 ? true : false", "Sentiment")
                .Append(_mlContext.Transforms.Text.FeaturizeText("Features", nameof(CommentData.SentimentText)))
                .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                    labelColumnName: "Label", featureColumnName: "Features"));

            var model = pipeline.Fit(data);

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<CommentData, CommentPrediction>(model);

            var prediction = predictionEngine.Predict(new CommentData
            {
                SentimentText = _pageViewModel.Comment
            });

            string sentiment = prediction.Prediction ? "Positive" : "Negative";
            _pageViewModel.Result = $" The comment is: {sentiment}\nScore: {prediction.Score:F2}\nProbability: {prediction.Probability:P2}";

            if (sentiment == "Positive")
            {
                bool success = _commentRepo.AddCommentInDb(_pageViewModel.theseId, _pageViewModel.Comment, _pageViewModel.userid, 0);
                int commentId = _commentRepo.GetCommentId(_pageViewModel.Comment);

                MessageBox.Show(success ? "Comment added successfully!" : "Adding comment failed.");

                var newComment = new Comment
                {
                    UserId = _pageViewModel.userid,
                    CommentText = _pageViewModel.Comment,
                    Username = _pageViewModel.Username,
                    commentId = commentId,
                    TheseId = _pageViewModel.theseId,
                    user_profilepic = _userRepo.GetProfilepicFromId(_pageViewModel.userid),
                    IsExpanded = false
                };
                _pageViewModel.CommentService.Comments.Add(newComment);

                //   var newCommentVM = new CommentsViewModel(newComment);
                //     _pageViewModel.Comments.Add(newCommentVM);

                _pageViewModel.Comment = string.Empty;
            }
            else if (sentiment == "Negative")
            {
                MessageBox.Show("Ce commentaire peut contenir du contenu restreint. Il ne sera publié qu'après approbation par un administrateur.",
                    "Commentaire négatif", MessageBoxButton.OK, MessageBoxImage.Error);

                _commentRepo.AddCommentInDb(_pageViewModel.theseId, _pageViewModel.Comment, _pageViewModel.userid, 1);
                int commentId = _commentRepo.GetCommentId(_pageViewModel.Comment);
                _reportsRepo.ReportComment(commentId, null);

                _pageViewModel.Comment = string.Empty;
            }
        }
    }
}
