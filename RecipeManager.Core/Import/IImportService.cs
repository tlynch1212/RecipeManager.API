using RecipeManager.Core.Models;
using System.Collections.Generic;

namespace RecipeManager.Core.Import
{
    public interface IImportService
    {
        public ImportJob CheckStatus(int jobId);
        ImportJob CheckStatusLite(int jobId);
        void RestartImport(ImportJob importJob, List<ImportModel> data);
        void StartImport(List<ImportModel> data);
    }
}