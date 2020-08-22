using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RecipeManager.Core;
using RecipeManager.Core.Models;
using RecipeManager.Core.Repositories;
using RecipeManager.Test.TestHelpers;

namespace RecipeManager.Test.Repositories
{
    [TestFixture]
    public class RecipeRepositoryTests
    {
        private RecipeRepository _recipeRepository;
        private RecipeManagerContext _recipeManagerContext;

        [SetUp]
        public void SetUp()
        {
            _recipeManagerContext = RecipeManagerContextHelper.CreateTestContext();
            _recipeRepository = new RecipeRepository(_recipeManagerContext);
        }

        [Test]
        public void GetRecipe_ReturnsExpected()
        {
            var expectedRecipe = new Recipe
            {
                Id = 1,
                ChangedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                TimeToCook = 10,
                Description = "description of recipe",
                Ingredients = new List<Ingredient>
                {
                    new Ingredient
                    {
                        Id = 1,
                        Value = "Cheese"
                    }
                },
                Instructions = new List<Instruction>
                {
                    new Instruction
                    {
                        Id = 1,
                        Value = "Add Cheese"
                    }
                },
                IsPublic = true,
                IsShared = true,
                SharedWith = new List<User>(),
                Name = "recipe name"
            };

            RecipeManagerContextHelper.AddRecipe(_recipeManagerContext, expectedRecipe);
            var result = _recipeRepository.GetRecipeById(expectedRecipe.Id);

            AssertRecipeCorrect(expectedRecipe, result);
        }

        [Test]
        public void GetRecipes_ReturnsExpected()
        {
            var expectedRecipes = new List<Recipe>
            {
                new Recipe
                {
                    Id = 1,
                    IsPublic = true,
                },
                new Recipe
                {
                    Id = 2,
                    IsPublic = true,
                }
            };

            AddRecipesToContext(expectedRecipes);
            var result = _recipeRepository.GetRecipes(2);

            AssertEachRecipeExists(expectedRecipes, result);
        }

        [Test]
        public void GetRecipesIds_ReturnsExpected()
        {
            var recipes = new List<Recipe>
            {
                new Recipe
                {
                    Id = 1,
                    IsPublic = true,
                },
                new Recipe
                {
                    Id = 2,
                    IsPublic = true,
                },
                new Recipe
                {
                    Id = 3,
                    IsPublic = false,
                }
            };

            AddRecipesToContext(recipes);
            var expectedRecipeIds = new List<int>
            {
               1, 2
            };

            var result = _recipeRepository.GetRecipeIds();

            foreach (var recipeId in expectedRecipeIds)
            {
                var resultRecipeId = result.FirstOrDefault(r => r == recipeId);
                Assert.AreEqual(recipeId, resultRecipeId);
            }
        }

        [Test]
        public void GetRecipesForUser_ReturnsExpected()
        {
            var user = new User
            {
                Id = 2
            };
            RecipeManagerContextHelper.AddUser(_recipeManagerContext, user);
            var expectedRecipes = new List<Recipe>
            {
                new Recipe
                {
                    Id = 1,
                    UserId = "2",
                    IsPublic = true,
                },
                new Recipe
                {
                    Id = 45,
                    IsShared = true,
                    SharedWith = new List<User>
                    {
                        user
                    }
                }
            };

            AddRecipesToContext(expectedRecipes);
            var result = _recipeRepository.GetRecipesForUser("2");

            AssertEachRecipeExists(expectedRecipes, result);
        }

        [Test]
        public void FavoriteRecipe_AddsUserToSharedWith()
        {
            var user = new User
            {
                Id = 2
            };
            RecipeManagerContextHelper.AddUser(_recipeManagerContext, user);

            var recipe = new Recipe
            {
                Id = 1,
                IsPublic = true,
                IsShared = false,
                SharedWith = null
            };

            var expectedRecipe = new Recipe
            {
                Id = 1,
                IsPublic = true,
                IsShared = true,
                SharedWith = new List<User>
                {
                    user
                }
            };

            RecipeManagerContextHelper.AddRecipe(_recipeManagerContext, recipe);

            _recipeRepository.FavoriteRecipe(recipe.Id, user.Id.ToString(),true);
            var result = _recipeRepository.GetRecipeById(recipe.Id);
            
            Assert.AreEqual(expectedRecipe.SharedWith.First().Id, result.SharedWith.First().Id);
            Assert.AreEqual(expectedRecipe.IsShared, result.IsShared);

        }

        [Test]
        public void FavoriteRecipe_DoesNotAddUserIfAlreadyAdded()
        {
            var user = new User
            {
                Id = 2
            };
            RecipeManagerContextHelper.AddUser(_recipeManagerContext, user);

            var recipe = new Recipe
            {
                Id = 1,
                IsPublic = true,
                IsShared = true,
                SharedWith = new List<User>
                {
                    user
                }
            };

            RecipeManagerContextHelper.AddRecipe(_recipeManagerContext, recipe);

            _recipeRepository.FavoriteRecipe(recipe.Id, user.Id.ToString(), true);
            var result = _recipeRepository.GetRecipeById(recipe.Id);

            Assert.AreEqual(recipe.SharedWith.First().Id, result.SharedWith.First().Id);
            Assert.AreEqual(recipe.IsShared, result.IsShared);

        }

        [Test]
        public void UnFavoriteRecipe_RemovesUserFromSharedWith()
        {
            var user = new User
            {
                Id = 2
            };
            RecipeManagerContextHelper.AddUser(_recipeManagerContext, user);

            var recipe = new Recipe
            {
                Id = 1,
                IsPublic = true,
                IsShared = true,
                SharedWith = new List<User>
                {
                    user
                }
            };

            var expectedRecipe = new Recipe
            {
                Id = 1,
                IsPublic = true,
                IsShared = false,
                SharedWith = new List<User>()
            };

            RecipeManagerContextHelper.AddRecipe(_recipeManagerContext, recipe);

            _recipeRepository.UnFavoriteRecipe(recipe.Id, user.Id.ToString(), true);
            var result = _recipeRepository.GetRecipeById(recipe.Id);

            Assert.AreEqual(expectedRecipe.SharedWith.Count, result.SharedWith.Count);
            Assert.AreEqual(expectedRecipe.IsShared, result.IsShared);

        }

        [Test]
        public void UpdateRecipe_ChangesRecipeToExpected()
        {
            var recipe = new Recipe
            {
                Id = 1,
                IsPublic = true,
                IsShared = true,
            };
            RecipeManagerContextHelper.AddRecipe(_recipeManagerContext, recipe);
            RecipeManagerContextHelper.DetachAllEntities(_recipeManagerContext);


            var expectedRecipe = new Recipe
            {
                Id = 1,
                ChangedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                Description = "description of recipe",
                Ingredients = new List<Ingredient>(),
                Instructions = new List<Instruction>(),
                IsPublic = true,
                IsShared = true,
                SharedWith = new List<User>(),
                Name = "recipe name"
            };

            _recipeRepository.UpdateRecipe(expectedRecipe, true);
            var result = _recipeRepository.GetRecipeById(expectedRecipe.Id);

            Assert.AreEqual(expectedRecipe, result);
        }

        [Test]
        public void DeleteRecipe_RemovesExpectedRecipe()
        {
            var recipe = new Recipe
            {
                Id = 1,
                IsPublic = true,
                IsShared = true,
            };
            RecipeManagerContextHelper.AddRecipe(_recipeManagerContext, recipe);
            RecipeManagerContextHelper.DetachAllEntities(_recipeManagerContext);

            _recipeRepository.DeleteRecipe(recipe, true);
            var result = _recipeRepository.GetRecipeById(recipe.Id);

            Assert.IsNull(result);
        }

        [Test]
        public void CreateRecipe_AddRecipeToContext()
        {
            var expectedRecipe = new Recipe
            {
                Id = 1,
                ChangedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                Description = "description of recipe",
                Ingredients = new List<Ingredient>(),
                Instructions = new List<Instruction>(),
                IsPublic = true,
                IsShared = true,
                SharedWith = new List<User>(),
                Name = "recipe name"
            };

            _recipeRepository.CreateRecipe(expectedRecipe, true);
            var result = _recipeRepository.GetRecipeById(expectedRecipe.Id);

            Assert.AreEqual(expectedRecipe, result);
        }

        private void AddRecipesToContext(List<Recipe> recipes)
        {
            foreach (var recipe in recipes)
            {
                RecipeManagerContextHelper.AddRecipe(_recipeManagerContext, recipe);
            }
        }

        private static void AssertEachRecipeExists(List<Recipe> expectedRecipes, List<Recipe> result)
        {
            foreach (var recipe in expectedRecipes)
            {
                var resultRecipe = result.FirstOrDefault(r => r.Id == recipe.Id);
                Assert.AreEqual(recipe, resultRecipe);
            }
        }


        private void AssertRecipeCorrect(Recipe expectedRecipe, Recipe result)
        {
            Assert.AreEqual(expectedRecipe.Id, result.Id);
            Assert.AreEqual(expectedRecipe.ChangedDate, result.ChangedDate);
            Assert.AreEqual(expectedRecipe.CreatedDate, result.CreatedDate);
            Assert.AreEqual(expectedRecipe.TimeToCook, result.TimeToCook);
            Assert.AreEqual(expectedRecipe.Description, result.Description);
            Assert.AreEqual(expectedRecipe.Ingredients.First().Id, result.Ingredients.First().Id);
            Assert.AreEqual(expectedRecipe.Ingredients.First().Value, result.Ingredients.First().Value);
            Assert.AreEqual(expectedRecipe.Instructions.First().Id, result.Instructions.First().Id);
            Assert.AreEqual(expectedRecipe.Instructions.First().Value, result.Instructions.First().Value);
            Assert.AreEqual(expectedRecipe.IsPublic, result.IsPublic);
            Assert.AreEqual(expectedRecipe.IsShared, result.IsShared);
            Assert.AreEqual(expectedRecipe.IsShared, result.IsShared);
            Assert.AreEqual(expectedRecipe.Name, result.Name);
        }
    }
}
