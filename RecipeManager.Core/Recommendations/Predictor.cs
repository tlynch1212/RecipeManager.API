using Microsoft.ML;
using RecipeManager.Core.Models;
using RecipeManager.Core.Recommendations.Models;
using RecipeManager.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManager.Core.Recommendations
{
    public class Predictor : IPredictor
    {
        private readonly IRecipeRepository _recipeRepository;

        public Predictor(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public List<Recipe> Predict(int userId, int amount)
        {
            MLContext mlContext = new MLContext();
            var predictionPipeline = mlContext.Model.Load("model.zip", out _);
            var predictionEngine = mlContext.Model.CreatePredictionEngine<RecipeModel, RecipePrediction>(predictionPipeline);

            var topRecommendations = (from recipeId in _recipeRepository.GetRecipeIds() 
                        let p = predictionEngine.Predict(
                           new RecipeModel()
                           {
                               UserId = userId,
                               RecipeId = recipeId
                           })
                        where p.Score > 1
                        orderby p.Score descending
                        select (RecipeId: recipeId, p.Score)).Take(amount);

            var recipes = new List<Recipe>();
            foreach (var prediction in topRecommendations)
            {
                if (prediction.Score >= 3.5)
                {
                    recipes.Add(_recipeRepository.GetRecipeById(prediction.RecipeId));
                }
            }

            return recipes;
        }
    }
}
