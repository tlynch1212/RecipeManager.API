using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using RecipeManager.API.Controllers;
using RecipeManager.Core.Models;
using RecipeManager.Core.Repositories;

namespace RecipeManager.Test.Controllers
{
    class BrowseControllerTests
    {
        private BrowseController _controller;
        private Mock<IRecipeRepository> _mockRepo;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRecipeRepository>();
            _controller = new BrowseController(_mockRepo.Object);
        }

        [Test]
        public void GetPassedWithFetchCount_ReturnsCorrectCount()
        {
            var expected = SetExpectedRecipes(250);
            _mockRepo.Setup(s => s.GetRecipes(250)).Returns(expected);

            var result = _controller.Get(250);
            Assert.AreEqual(expected, result);
        }

        private static List<Recipe> SetExpectedRecipes(int fetchCount)
        {
            var expected = new List<Recipe>();
            for (int x = 1; x >= fetchCount; x++)
            {
                expected.Add(new Recipe());
            }


            return expected;
        }
    }
}
