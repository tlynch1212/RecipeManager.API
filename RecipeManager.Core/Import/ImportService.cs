using RecipeManager.Core.Models;
using RecipeManager.Core.Repositories;
using System;
using System.Collections.Generic;

namespace RecipeManager.Core.Import
{
    public class ImportService : IImportService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IImportJobRepository _importJobRepository;
        private readonly IImportStatusRepository _importStatusRepository;
        public ImportService(IRecipeRepository recipeRepository, IImportJobRepository importJobRepository, IImportStatusRepository importStatusRepository)
        {
            _recipeRepository = recipeRepository;
            _importJobRepository = importJobRepository;
            _importStatusRepository = importStatusRepository;
        }


        public void RestartImport(ImportJob importJob)
        {
            if (importJob.Status.Id == (int)ImportStatus.Fail)
            {
                SetJobWorking(importJob);
                ImportData(importJob);
            }
        }

        public ImportJob CheckStatusLite(int jobId)
        {
            return _importJobRepository.GetJobLite(jobId);
        }

        public ImportJob CheckStatus(int jobId)
        {
            return _importJobRepository.GetJob(jobId);
        }

        public void StartImport(List<ImportModel> data)
        {
            var importJob = StartJob(data);
            ImportData(importJob);
        }

        private void ImportData(ImportJob importJob)
        {
            try
            {
                foreach (var importModel in importJob.DataToImport.ToArray())
                {
                    var convertedRecipe = ConvertToRecipe(importModel);
                    if (convertedRecipe != null)
                    {
                        if (_recipeRepository.CheckDuplication(convertedRecipe) == null)
                        {
                            _recipeRepository.CreateRecipe(convertedRecipe);
                            UpdateWorkingJob(importJob, importModel, true);
                        }
                        else
                        {
                            UpdateWorkingJob(importJob, importModel, false);
                        }
                    }
                }
                SetJobSuccessful(importJob);
            }
            catch (Exception)
            {
                SetJobFailure(importJob);
            }
        }

        private void UpdateWorkingJob(ImportJob importJob, ImportModel importModel, bool gotImported)
        {
            importJob.DataToImport.Remove(importModel);
            if (gotImported)
            {
                importJob.ImportedCount += 1;
            }
            _importJobRepository.Update(importJob);
        }

        private void SetJobFailure(ImportJob importJob)
        {
            importJob.Status = _importStatusRepository.GetStatus((int)ImportStatus.Fail);
            _importJobRepository.Update(importJob);
        }

        private void SetJobWorking(ImportJob importJob)
        {
            importJob.Status = _importStatusRepository.GetStatus((int)ImportStatus.Working);
            _importJobRepository.Update(importJob);
        }

        private void SetJobSuccessful(ImportJob importJob)
        {
            importJob.Status = _importStatusRepository.GetStatus((int)ImportStatus.Success);
            importJob.EndDate = DateTime.Now;
            _importJobRepository.Update(importJob);
        }

        private ImportJob StartJob(List<ImportModel> data)
        {
            var importJob = new ImportJob
            {
                DataToImport = data,
                StartDate = DateTime.Now,
                Status = _importStatusRepository.GetStatus((int)ImportStatus.Working),
                TotalCount = data.Count,
                ImportedCount = 0
            };
            _importJobRepository.CreateImportJob(importJob);

            return importJob;
        }

        private Recipe ConvertToRecipe(ImportModel importModel)
        {
            return new Recipe
            {
                Name = importModel.title,
                Instructions = importModel.instructions,
                Ingredients = ConvertIngredients(importModel.ingredients),
                IsPublic = true,
                IsShared = false
            };
        }

        private static List<Ingredient> ConvertIngredients(List<string> ingredients)
        {
            var convertedIngredients = new List<Ingredient>();

            foreach (string ingredient in ingredients)
            {
                convertedIngredients.Add(new Ingredient
                {
                    Value = ingredient
                });
            }

            return convertedIngredients;
        }
    }
}