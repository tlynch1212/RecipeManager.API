using RecipeManager.Core.Models;
using System.Collections.Generic;

namespace RecipeManager.Core.Repositories
{
    public interface IRateRepository
    {
        void CreateRating(Rating rating, bool save);
        List<Rating> GetAll();
    }
}