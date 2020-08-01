using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using RecipeManager.Core.Recommendations.Models;
using System.Collections.Generic;

namespace RecipeManager.Core.Recommendations
{
    public class Trainer : ITrainer
    {
        public RegressionMetrics Train()
        {
            var mlContext = new MLContext();
            (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);
            var model = BuildAndTrainModel(mlContext, trainingDataView);
            return EvaluateModel(mlContext, testDataView, model);
        }

        public static (IDataView training, IDataView test) LoadData(MLContext mlContext)
        {
            var trainingData = new List<RecipeModel>();
            var testData = new List<RecipeModel>();
            IDataView trainingDataView = mlContext.Data.LoadFromEnumerable(trainingData);
            IDataView testDataView = mlContext.Data.LoadFromEnumerable(testData);

            return (trainingDataView, testDataView);
        }

        public static ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView)
        {
            var estimator = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: "userId")
                                                        .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "movieIdEncoded", inputColumnName: "movieId"));
            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "movieIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));

            return trainerEstimator.Fit(trainingDataView);
        }

        public static RegressionMetrics EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model)
        {
            var prediction = model.Transform(testDataView);
            return mlContext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");
        }
    }
}
