using Microsoft.EntityFrameworkCore;
using RecipeManager.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManager.Core.Repositories
{
    public class RateRepository: IRateRepository
    {
        private readonly RecipeManagerContext _dbContext;

        public RateRepository(RecipeManagerContext context)
        {
            _dbContext = context;
        }

        public List<Rating> GetAll()
        {
            return _dbContext.Ratings.ToList();
        }

        public void CreateRating(Rating rating, bool save)
        {
            _dbContext.Ratings.Add(rating);
            if (save)
            {
                _dbContext.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
