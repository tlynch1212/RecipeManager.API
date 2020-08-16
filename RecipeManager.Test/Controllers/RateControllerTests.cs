using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RecipeManager.API;
using RecipeManager.API.Controllers;
using RecipeManager.Core.Models;
using RecipeManager.Core.Repositories;

namespace RecipeManager.Test.Controllers
{
    class RateControllerTests
    {
        private RateController _controller;
        private Mock<IRateRepository> _mockRepo;
        private Mock<ILoggerWrapper> _mockLogger;
        private Mock<IUserRepository> _mockUserRepo;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILoggerWrapper>();
            _mockUserRepo = new Mock<IUserRepository>();
            _mockRepo = new Mock<IRateRepository>();
            _controller = new RateController(_mockLogger.Object, _mockRepo.Object, _mockUserRepo.Object);
        }

        [Test]
        public void Create_CreatesRating()
        {
            var expected = new Rating
            {
                UserId = "test"
            };
            _mockUserRepo.Setup(s => s.GetByAuthId("test")).Returns(new User
            {
                Id = 1,
                AuthId = "test"
            });
            var result = _controller.Create(expected) as OkResult;
            _mockRepo.Verify(s => s.CreateRating(expected, true), Times.Once);
            Assert.AreEqual(200, result?.StatusCode);
        }

        [Test]
        public void Create_WhenError_LogsAndReturnsExpected()
        {
            var expected = new Exception("test exception");
            _mockUserRepo.Setup(s => s.GetByAuthId("test")).Returns(new User
            {
                Id = 1,
                AuthId = "test"
            });
            _mockRepo.Setup(s => s.CreateRating(It.IsAny<Rating>(), It.IsAny<bool>())).Throws(expected);

            var result = _controller.Create(new Rating
            {
                UserId = "test"
            }) as StatusCodeResult;
            _mockLogger.Verify(x => x.LogError(expected, expected.Message), Times.Once);
            Assert.AreEqual(500, result?.StatusCode);
        }

        [Test]
        public void Get_GetsExpected()
        {
            var expected = new Rating
            {
                UserId = "1",
                RecipeId = 1
            };
            _mockRepo.Setup(s => s.Get(It.IsAny<string>(), It.IsAny<int>())).Returns(expected);

            var result = _controller.Get(1, 1) as OkObjectResult;
            Assert.AreEqual(expected, result?.Value);
        }
    }
}
