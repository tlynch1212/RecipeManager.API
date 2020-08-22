using RecipeManager.Core.Models;
using System.Linq;

namespace RecipeManager.Core.Repositories
{
    public class RecipeUserRepository : IRecipeUserRepository
    {
        private readonly RecipeManagerContext _dbContext;

        public RecipeUserRepository(RecipeManagerContext context)
        {
            _dbContext = context;
        }

        public RecipeUser Get(int userId, int recipeId)
        {
            return _dbContext.RecipeUsers.FirstOrDefault(t => t.User.Id == userId && t.Recipe.Id == recipeId);
        }
    }
}
