using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using RecipeManager.Core.Recommendations.Models;
using RecipeManager.Core.Repositories;
using System;
using System.Linq;

namespace RecipeManager.Core.Recommendations
{
    public class Trainer : ITrainer
    {
        private readonly IRateRepository _rateRepository;

        public Trainer(IRateRepository rateRepository)
        {
            _rateRepository = rateRepository;
        }

        public RegressionMetrics Train(int numberOfIterations)
        {
            var mlContext = new MLContext();
            (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);
            var trainedModel = BuildAndTrainModel(mlContext, trainingDataView, numberOfIterations);
            mlContext.Model.Save(trainedModel, trainingDataView.Schema, "model.zip");
            return EvaluateModel(mlContext, testDataView, trainedModel);
        }

        private (IDataView training, IDataView test) LoadData(MLContext mlContext)
        {
            var r = new Random();
            var data = _rateRepository.GetAll().Select(s => new RecipeModel
            {
                UserId = int.Parse(s.UserId),
                RecipeId = s.RecipeId,
                Rating = s.Rate
            }).OrderBy(t => r.Next()).ToList();
            var splitAmount = (int)(data.Count * 0.80);
            var trainingData = data.Take(splitAmount);
            var testData = data.Skip(splitAmount);
            IDataView trainingDataView = mlContext.Data.LoadFromEnumerable(trainingData);
            IDataView testDataView = mlContext.Data.LoadFromEnumerable(testData);

            return (trainingDataView, testDataView);
        }

        private ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView, int numberOfIterations)
        {
            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "UserIdEncoded",
                MatrixRowIndexColumnName = "RecipeIdEncoded",
                LabelColumnName = "Rating",
                NumberOfIterations = numberOfIterations,
                ApproximationRank = 100
            };
            var estimator = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "UserIdEncoded", inputColumnName: "UserId")
                                                        .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "RecipeIdEncoded", inputColumnName: "RecipeId"));
            var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));

            return trainerEstimator.Fit(trainingDataView);
        }

        private RegressionMetrics EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model)
        {
            var prediction = model.Transform(testDataView);
            return mlContext.Regression.Evaluate(prediction, "Rating");
        }
    }
}
