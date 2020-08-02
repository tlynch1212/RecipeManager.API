namespace RecipeManager.Core.Recommendations.Models
{
    public class RecipeModel
    {
        public int UserId { get; set; }

        public int RecipeId { get; set; }

        public float Rating { get; set; }
    }
}