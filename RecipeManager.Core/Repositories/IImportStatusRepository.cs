using RecipeManager.Core.Models;
using System.Collections.Generic;

namespace RecipeManager.Core.Repositories
{
    public interface IImportStatusRepository
    {
        ImportStatus GetStatus(int statusId);
        List<ImportStatus> GetStatuses();
    }
}