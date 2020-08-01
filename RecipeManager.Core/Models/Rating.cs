namespace RecipeManager.Core.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public Recipe Recipe { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
    }
}
