using Microsoft.EntityFrameworkCore;
using RecipeManager.Core.Models;

namespace RecipeManager.Core
{
    public interface IDbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<User> Users { get; set; }
}
}