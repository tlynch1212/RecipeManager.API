using System;
using System.Collections.Generic;

namespace RecipeManager.Core.Models
{
    public class ImportJob
    {
        public int Id { get; set; }
        public int TotalCount { get; set; }
        public int ImportedCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ImportStatus Status { get; set; }
    }
}