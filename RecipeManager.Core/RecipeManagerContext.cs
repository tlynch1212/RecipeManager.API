using Microsoft.EntityFrameworkCore;
using RecipeManager.Core.Interfaces;
using RecipeManager.Core.Models;

namespace RecipeManager.Core
{
    public class RecipeManagerContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        public RecipeManagerContext(DbContextOptions<RecipeManagerContext> options)
    : base(options)
        { }
    }

}
