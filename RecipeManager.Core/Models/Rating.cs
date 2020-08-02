namespace RecipeManager.Core.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
    }
}
