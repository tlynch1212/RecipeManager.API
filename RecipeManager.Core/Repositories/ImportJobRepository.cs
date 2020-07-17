using Microsoft.EntityFrameworkCore;
using RecipeManager.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManager.Core.Repositories
{
    public class ImportJobRepository : IImportJobRepository
    {
        private readonly RecipeManagerContext _dbContext;

        public ImportJobRepository(RecipeManagerContext context)
        {
            _dbContext = context;
        }

        public ImportJob GetJobLite(int jobId)
        {
            return _dbContext.ImportJobs.Include(x => x.Status).FirstOrDefault(t => t.Id == jobId);
        }

        public ImportJob GetJob(int jobId)
        {
            return _dbContext.ImportJobs.Include(x => x.Status).FirstOrDefault(t => t.Id == jobId);
        }

        public void Update(ImportJob importJob)
        {
            _dbContext.ImportJobs.Update(importJob);
            _dbContext.SaveChanges();
        }

        public void CreateImportJob(ImportJob importJob)
        {
            _dbContext.ImportJobs.Add(importJob);
            _dbContext.SaveChanges();
        }
    }
}
