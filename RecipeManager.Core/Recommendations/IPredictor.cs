using RecipeManager.Core.Recommendations.Models;

namespace RecipeManager.Core.Recommendations
{
    public interface IPredictor
    {
        RecipePrediction Predict(RecipeModel dataToPredict);
    }
}