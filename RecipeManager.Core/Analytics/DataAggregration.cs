using Microsoft.EntityFrameworkCore.Query.Internal;
using RecipeManager.Core.Models;
using RecipeManager.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeManager.Core.Analytics
{
    public class DataAggregration : IDataAggregation
    {
        private IRecipeRepository _recipeRepository;
        private IRateRepository _rateRepository;

        public DataAggregration(IRateRepository rateRepository, IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
            _rateRepository = rateRepository;
        }

        public TableData GetTopRated(int count)
        {
            var topRated = _rateRepository.GetAll().GroupBy(t => new { t.RecipeId }).Select(group => new TopRated
            {
                RecipeId = group.Key.RecipeId,
                Count = group.Count()
            }).OrderByDescending(t => t.Count).ToList().Take(count);

            foreach (var topRecipe in topRated)
            {
                topRecipe.Recipe = _recipeRepository.GetRecipeById(topRecipe.RecipeId);

            }

            return new TableData
            {
                DataSets = new List<TableDataSet>
                {
                    new TableDataSet
                    {
                        Labels = topRated.Select(t => t.Recipe.Name).ToList(),
                        Data = topRated.Select(t => t.Count).ToList()
                    }
                },
                Title = $"Top Ratings {DateTime.Now.Year}"
            };
        }

        public TableData GetRateDifferences()
        {
            var topRated = _rateRepository.GetAll().GroupBy(t => new { t.Rate }).Select(group => new
            {
                group.Key.Rate,
                Count = group.Count()
            }).OrderByDescending(t => t.Count).ToList();

            return new TableData
            {
                DataSets = new List<TableDataSet>
                {
                    new TableDataSet
                    {
                        Labels = topRated.Select(t => t.Rate.ToString()).ToList(),
                        Data = topRated.Select(t => t.Count).ToList()
                    }
                },
                Title = $"Types of Ratings {DateTime.Now.Year}"
            };
        }

        public TableData GetMostInteraction(int count)
        {
            var topRated = _rateRepository.GetAll().GroupBy(t => new { t.UserId }).Select(group => new
            {
                group.Key.UserId,
                Count = group.Count()
            }).OrderByDescending(t => t.Count).Take(count).ToList();

            return new TableData
            {
                DataSets = new List<TableDataSet>
                {
                    new TableDataSet
                    {
                        Labels = topRated.Select(t => t.UserId.ToString()).ToList(),
                        Data = topRated.Select(t => t.Count).ToList()
                    }
                },
                Title = $"Top 10 User Interaction {DateTime.Now.Year}"
            };
        }
    }
}
