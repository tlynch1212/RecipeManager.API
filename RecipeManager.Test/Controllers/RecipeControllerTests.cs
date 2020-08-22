using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RecipeManager.API;
using RecipeManager.API.Controllers;
using RecipeManager.Core.Models;
using RecipeManager.Core.Repositories;

namespace RecipeManager.Test.Controllers
{
    [TestFixture]
    class RecipeControllerTests
    {
        private RecipeController _controller;
        private Mock<ILoggerWrapper> _mockLogger;
        private Mock<IRecipeRepository> _mockRepo;
        private Mock<IUserRepository> _mockUserRepo;


        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRecipeRepository>();
            _mockLogger = new Mock<ILoggerWrapper>();
            _mockUserRepo = new Mock<IUserRepository>();
            _controller = new RecipeController(_mockLogger.Object, _mockRepo.Object, _mockUserRepo.Object);
        }

        [Test]
        public void Create_CreatesRecipe()
        {
            var expected = new Recipe
            {
                UserId = "test"
            };
            _mockUserRepo.Setup(s => s.GetByAuthId(It.IsAny<string>())).Returns(new User
            {
                Id = 1,
                AuthId = "test"
            });

            var result = _controller.Create(expected) as OkResult;
            _mockRepo.Verify(s => s.CreateRecipe(expected, true), Times.Once);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public void Create_WhenError_LogsAndReturnsExpected()
        {
            var expected = new Exception("test exception");
            _mockUserRepo.Setup(s => s.GetByAuthId(It.IsAny<string>())).Returns(new User
            {
                Id = 1,
                AuthId = "test"
            });
            _mockRepo.Setup(s => s.CreateRecipe(It.IsAny<Recipe>(), It.IsAny<bool>())).Throws(expected);

            var result = _controller.Create(new Recipe()) as StatusCodeResult;
            _mockLogger.Verify(x => x.LogError(expected, expected.Message), Times.Once);
            Assert.AreEqual(500, result?.StatusCode);
        }

        [Test]
        public void Update_UpdatesRecipe()
        {
            var expected = new Recipe();

            var result = _controller.Update(expected) as OkResult;
            _mockRepo.Verify(s => s.UpdateRecipe(expected, true), Times.Once);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public void Update_WhenError_LogsAndReturnsExpected()
        {
            var expected = new Exception("test exception");
            _mockRepo.Setup(s => s.UpdateRecipe(It.IsAny<Recipe>(), It.IsAny<bool>())).Throws(expected);

            var result = _controller.Update(new Recipe()) as StatusCodeResult;
            _mockLogger.Verify(x => x.LogError(expected, expected.Message), Times.Once);
            Assert.AreEqual(500, result?.StatusCode);
        }

        [Test]
        public void Favorite_UpdatesRecipe()
        {
            var expected = new FavoriteModel
            {
                RecipeId = 20,
                UserId = "test"
            };
            _mockUserRepo.Setup(s => s.GetByAuthId("test")).Returns(new User
            {
                Id = 1,
                AuthId = "test"
            });
            var result = _controller.Favorite(expected) as OkResult;
            _mockRepo.Verify(s => s.FavoriteRecipe(expected.RecipeId, 1.ToString(), true), Times.Once);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public void Favorite_WhenError_LogsAndReturnsExpected()
        {
            var expected = new Exception("test exception");
            _mockUserRepo.Setup(s => s.GetByAuthId(It.IsAny<string>())).Returns(new User
            {
                Id = 1,
                AuthId = "test"
            });
            _mockRepo.Setup(s => s.FavoriteRecipe(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Throws(expected);

            var result = _controller.Favorite(new FavoriteModel
            {
                RecipeId = 1,
                UserId = "test"
            }) as StatusCodeResult;
            _mockLogger.Verify(x => x.LogError(expected, expected.Message), Times.Once);
            Assert.AreEqual(500, result?.StatusCode);
        }

        [Test]
        public void UnFavorite_UpdatesRecipe()
        {
            var expected = new FavoriteModel
            {
                RecipeId = 20,
                UserId = "test"
            };
            _mockUserRepo.Setup(s => s.GetByAuthId("test")).Returns(new User
            {
                Id = 1,
                AuthId = "test"
            });
            var result = _controller.UnFavorite(expected) as OkResult;
            _mockRepo.Verify(s => s.UnFavoriteRecipe(expected.RecipeId, 1.ToString(), true), Times.Once);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public void UnFavorite_WhenError_LogsAndReturnsExpected()
        {
            var expected = new Exception("test exception");
            _mockUserRepo.Setup(s => s.GetByAuthId(It.IsAny<string>())).Returns(new User
            {
                Id = 1,
                AuthId = "test"
            });
            _mockRepo.Setup(s => s.UnFavoriteRecipe(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>())).Throws(expected);

            var result = _controller.UnFavorite(new FavoriteModel
            {
                RecipeId = 1,
                UserId = "test"
            }) as StatusCodeResult;
            _mockLogger.Verify(x => x.LogError(expected, expected.Message), Times.Once);
            Assert.AreEqual(500, result?.StatusCode);
        }

        [Test]
        public void Delete_DeletesRecipe()
        {
            var expected = new Recipe();

            var result = _controller.Delete(expected) as OkResult;
            _mockRepo.Verify(s => s.DeleteRecipe(expected, true), Times.Once);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public void Delete_WhenError_LogsAndReturnsExpected()
        {
            var expected = new Exception("test exception");
            _mockRepo.Setup(s => s.DeleteRecipe(It.IsAny<Recipe>(), It.IsAny<bool>())).Throws(expected);

            var result = _controller.Delete(new Recipe()) as StatusCodeResult;
            _mockLogger.Verify(x => x.LogError(expected, expected.Message), Times.Once);
            Assert.AreEqual(500, result?.StatusCode);
        }

        [Test]
        public void Get_ReturnsExpected()
        {
            var expected = new List<Recipe>();
            _mockUserRepo.Setup(s => s.GetByAuthId("test")).Returns(new User
            {
                Id = 1,
                AuthId = "test"
            });
            _mockRepo.Setup(r => r.GetRecipesForUser(It.IsAny<string>())).Returns(expected);
            var result = _controller.Get("test") as OkObjectResult;
            Assert.AreEqual(expected, result?.Value);
        }

        [Test]
        public void GetById_ReturnsExpected()
        {
            var expected = new Recipe();
            _mockRepo.Setup(r => r.GetRecipeById(It.IsAny<int>())).Returns(expected);
            var result = _controller.GetById(1) as OkObjectResult;
            Assert.AreEqual(expected, result?.Value);
        }
    }
}
