using RecipeManager.Core.Models;

namespace RecipeManager.Core.Repositories
{
    public interface IRecipeUserRepository
    {
        RecipeUser Get(int userId, int recipeId);
    }
}