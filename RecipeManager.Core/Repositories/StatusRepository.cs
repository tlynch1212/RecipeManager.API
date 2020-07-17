using RecipeManager.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManager.Core.Repositories
{
    public class ImportStatusRepository : IImportStatusRepository
    {
        private readonly RecipeManagerContext _dbContext;

        public ImportStatusRepository(RecipeManagerContext context)
        {
            _dbContext = context;
        }

        public List<ImportStatus> GetStatuses()
        {
            return _dbContext.ImportStatus.ToList();
        }

        public ImportStatus GetStatus(int statusId)
        {
            return _dbContext.ImportStatus.First(t => t.Id == statusId);
        }
    }
}
