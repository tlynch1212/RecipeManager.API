using RecipeManager.Core.Models;

namespace RecipeManager.Core.Repositories
{
    public interface IRateRepository
    {
        void CreateRating(Rating rating, bool save);
    }
}