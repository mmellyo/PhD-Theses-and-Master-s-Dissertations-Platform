using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using System.Windows.Input;
using Project.Models;
using Project.Services;

namespace Project.ViewModels
{
    public class CommentViewModel : ViewModelBase
    {
        private readonly MLContext _mlContext;
        private readonly IWindowManager _windowManager;
        private readonly ViewModelLocator _viewModelLocator;

        public ICommentService CommentService { get; }
        private ICommentService commentService {  get; set; } 

        private string _result;
        private string _comment;

        // getters / setters
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
        public CommentViewModel(ICommentService commentService, IWindowManager windowManager, ViewModelLocator viewModelLocator)
        {
            //fields
            _windowManager = windowManager;
            _viewModelLocator = viewModelLocator;
            CommentService = commentService;
            _mlContext = new MLContext();


            AddCommentCommand = new ViewModelCommand(
                execute: obj =>
                {
                    try
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
                            SentimentText = Comment
                        });

                        string sentiment = prediction.Prediction ? "Positive" : "Negative";
                        Result = $"The comment is: {sentiment}\nScore: {prediction.Score:F2}\nProbability: {prediction.Probability:P2}";

                        if ( sentiment == "Positive")
                        {
                            CommentService.AddComment(Comment);
                        }
                    
                    }
                    catch (Exception ex)
                    {
                        Result = $"Error: {ex.Message}";
                    }
                },
                canExecute: obj => true
                //!string.IsNullOrEmpty(Comment) // Only enable the command if there's text to analyze
            );
        }
    }
}
    
