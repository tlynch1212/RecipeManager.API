using Microsoft.EntityFrameworkCore;
using RecipeManager.Core.Models;


namespace RecipeManager.Core
{
    public class RecipeManagerContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        public RecipeManagerContext(DbContextOptions<RecipeManagerContext> options)
    : base(options)
        { }
    }

}
