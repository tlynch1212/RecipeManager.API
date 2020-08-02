using RecipeManager.Core.Models;
using System.Collections.Generic;

namespace RecipeManager.Core.Recommendations
{
    public interface IPredictor
    {
        List<Recipe> Predict(int userId, int amount);
    }
}