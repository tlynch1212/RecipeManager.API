using Microsoft.ML.Data;
using System.Collections.Generic;

namespace RecipeManager.Core.Recommendations.Models
{
    public class RecipeModel
    {
        [LoadColumn(0)]
        public string UserId { get; set; }

        [LoadColumn(1)]
        public string RecipeName { get; set; }

        [LoadColumn(2)]
        public int Liked { get; set; }
    }
}