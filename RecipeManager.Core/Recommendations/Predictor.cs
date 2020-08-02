using Microsoft.ML;
using RecipeManager.Core.Recommendations.Models;

namespace RecipeManager.Core.Recommendations
{
    public class Predictor : IPredictor
    {
        public RecipePrediction Predict(RecipeModel dataToPredict)
        {
            MLContext mlContext = new MLContext();
            var predictionPipeline = mlContext.Model.Load("model.zip", out _);
            var predictionEngine = mlContext.Model.CreatePredictionEngine<RecipeModel, RecipePrediction>(predictionPipeline);

            var t = predictionEngine.Predict(dataToPredict);
            return t;
        }
    }
}
