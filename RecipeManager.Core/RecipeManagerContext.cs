using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using RecipeManager.Core.Models;


namespace RecipeManager.Core
{
    [ExcludeFromCodeCoverage]
    public class RecipeManagerContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RecipeUser> RecipeUsers { get; set; }

        public RecipeManagerContext(DbContextOptions<RecipeManagerContext> options)
            : base(options)
        {
            Database.SetCommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds);
        }
    }

}
