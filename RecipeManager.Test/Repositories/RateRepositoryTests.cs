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
    public class RateRepositoryTests
    {
        private RateRepository _rateRepository;
        private RecipeManagerContext _recipeManagerContext;

        [SetUp]
        public void SetUp()
        {
            _recipeManagerContext = RecipeManagerContextHelper.CreateTestContext();
            _rateRepository = new RateRepository(_recipeManagerContext);
        }

        [Test]
        public void GetRating_ReturnsExpected()
        {
            var expectedRate = new Rating
            {
                UserId = "1",
                RecipeId = 1
            };

            RecipeManagerContextHelper.AddRating(_recipeManagerContext, expectedRate);
            var result = _rateRepository.Get(expectedRate.UserId, expectedRate.RecipeId);

            Assert.AreEqual(expectedRate, result);
        }

        [Test]
        public void GetAll_ReturnsExpected()
        {
            var expectedRates = new List<Rating>
            {
                new Rating
                {
                    UserId = "1",
                    RecipeId = 1,
                },
                new Rating
                {
                    UserId = "2",
                    RecipeId = 2,
                }
            };

            RecipeManagerContextHelper.AddRating(_recipeManagerContext, expectedRates.First());
            RecipeManagerContextHelper.AddRating(_recipeManagerContext, expectedRates.Last());
            var result = _rateRepository.GetAll();

            Assert.AreEqual(expectedRates, result);
        }

        [Test]
        public void CreateRating_CreatesTheExpectedRating()
        {
            var expectedRate = new Rating
            {
                Id = 1,
                UserId = "1",
                RecipeId = 1,
                Rate = 5
            };

             _rateRepository.CreateRating(expectedRate, true);
             var result = _rateRepository.Get(expectedRate.UserId, expectedRate.RecipeId);

             Assert.AreEqual(expectedRate.Id, result.Id);
             Assert.AreEqual(expectedRate.UserId, result.UserId);
             Assert.AreEqual(expectedRate.RecipeId, result.RecipeId);
             Assert.AreEqual(expectedRate.Rate, result.Rate);
        }
    }
}
