using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using RecipeManager.Core.Recommendations.Models;
using RecipeManager.Core.Repositories;
using System;
using System.Data;
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

        public RegressionMetrics Train(int numberOfIterations, int approximationRank, double learningRate)
        {
            var mlContext = new MLContext();
            (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);
            var trainedModel = BuildAndTrainModel(mlContext, trainingDataView, numberOfIterations, approximationRank, learningRate);
            mlContext.Model.Save(trainedModel, trainingDataView.Schema, "model.zip");
            return EvaluateModel(mlContext, testDataView, trainedModel);
        }

        public RegressionMetrics TestModel()
        {
            var mlContext = new MLContext();
            (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);
            var model = mlContext.Model.Load("model.zip", out _);
            return EvaluateModel(mlContext, testDataView, model);
        }

        private (IDataView training, IDataView test) LoadData(MLContext mlContext)
        {

            var r = new Random();
            var testData = _rateRepository.GetAll().Where(t => t.UserId == "100026" || t.UserId == "283788" || t.UserId == "163112").Select(s => new RecipeModel
            {
                UserId = int.Parse(s.UserId),
                RecipeId = s.RecipeId,
                Rating = s.Rate
            }).OrderBy(t => r.Next()).ToList();

            var r2 = new Random();
            var trainingData = _rateRepository.GetAll().Where(t => t.UserId != "1000026" || t.UserId != "283788" || t.UserId != "163112").Select(s => new RecipeModel
            {
                UserId = int.Parse(s.UserId),
                RecipeId = s.RecipeId,
                Rating = s.Rate
            }).OrderBy(t => r2.Next()).ToList();

            IDataView trainingDataView = mlContext.Data.LoadFromEnumerable(trainingData);
            IDataView testDataView = mlContext.Data.LoadFromEnumerable(testData);


            var preview = ToDataTable(trainingDataView);

            return (trainingDataView, testDataView);
        }

        public static DataTable ToDataTable(IDataView dataView)
        {
            DataTable dt = null;
            if (dataView != null)
            {
                dt = new DataTable();
                var preview = dataView.Preview();
                dt.Columns.AddRange(preview.Schema.Select(x => new DataColumn(x.Name)).ToArray());
                foreach (var row in preview.RowView)
                {
                    var r = dt.NewRow();
                    foreach (var col in row.Values)
                    {
                        r[col.Key] = col.Value;
                    }
                    dt.Rows.Add(r);

                }
            }
            return dt;
        }

        private ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView, int numberOfIterations, int approximationRank, double learningRate)
        {
            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "UserIdEncoded",
                MatrixRowIndexColumnName = "RecipeIdEncoded",
                LabelColumnName = "Rating",
                NumberOfIterations = numberOfIterations,
                NumberOfThreads = 1,
                ApproximationRank = approximationRank,
                LearningRate = learningRate
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
