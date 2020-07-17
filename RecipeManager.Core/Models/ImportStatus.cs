using Microsoft.EntityFrameworkCore;

namespace RecipeManager.Core.Models
{
    public class ImportStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImportStatus>().HasData(
                new ImportStatus
                {
                    Id = 1,
                    Name = "Success"
                },
                new ImportStatus
                {
                    Id = 2,
                    Name = "Working"
                },
                new ImportStatus
                {
                    Id = 3,
                    Name = "Fail"
                }
            );
        }
    }
}
