using RecipeManager.Core.Models;

namespace RecipeManager.Core.Analytics
{
    public class TopRated
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int Count { get; set; }
    }
}