using Microsoft.EntityFrameworkCore;
using RecipeManager.Core.Models;

namespace RecipeManager.Core.Interfaces
{
    public interface IDbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
}
}