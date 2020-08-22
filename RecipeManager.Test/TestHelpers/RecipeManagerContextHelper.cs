using Microsoft.EntityFrameworkCore;
using RecipeManager.Core;
using RecipeManager.Core.Models;

namespace RecipeManager.Test.TestHelpers
{
    public static class RecipeManagerContextHelper
    {
        public static RecipeManagerContext CreateTestContext()
        {
            var options = new DbContextOptionsBuilder<RecipeManagerContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .EnableSensitiveDataLogging()
                .Options;
            var testContext = new RecipeManagerContext(options);
            testContext.Database.EnsureDeleted();

            return testContext;
        }

        public static void AddUser(RecipeManagerContext context, User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        public static void AddRecipe(RecipeManagerContext context, Recipe recipe)
        {
            context.Recipes.Add(recipe);
            context.SaveChanges();
        }

        public static void AddRating(RecipeManagerContext context, Rating rating)
        {
            context.Ratings.Add(rating);
            context.SaveChanges();
        }

        public static void DetachAllEntities(RecipeManagerContext context)
        {
            var changedEntriesCopy = context.ChangeTracker.Entries();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
    }
}
