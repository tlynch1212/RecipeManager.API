using RecipeManager.Core.Models;
using System.Collections.Generic;

namespace RecipeManager.Core.Repositories
{
    public interface IImportJobRepository
    {
        public void CreateImportJob(ImportJob importJob);
        public void Update(ImportJob importJob);
        ImportJob GetJob(int jobId);
        ImportJob GetJobLite(int jobId);
    }
}