using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeManager.Core.Models
{
    public class ImportModel
    {
        public int Id { get; set; }
        public string title { get; set; }
        public List<string> ingredients { get; set; }
        public string instructions { get; set; }
        public string picture_link { get; set; }
    }
}
