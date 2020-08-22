namespace RecipeManager.Core.Models
{
    public class RecipeUser
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Recipe Recipe { get; set; }
    }
}