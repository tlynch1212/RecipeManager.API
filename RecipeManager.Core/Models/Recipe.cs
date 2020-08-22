using System;
using System.Collections.Generic;

namespace RecipeManager.Core.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TimeToCook { get; set; }
        public List<Instruction> Instructions { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public bool IsPublic { get; set; }
        public bool IsShared { get; set; }
        public List<RecipeUser> SharedWith { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ChangedDate { get; set; }
    }
}