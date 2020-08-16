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
    class UserControllerTests
    {
        private UserController _controller;
        private Mock<IUserRepository> _mockRepo;
        private Mock<ILoggerWrapper> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILoggerWrapper>();
            _controller = new UserController(_mockLogger.Object, _mockRepo.Object);
        }

        [Test]
        public void Get_ReturnsExpected()
        {
            var expected = new User();
            _mockRepo.Setup(s => s.GetByAuthId("test")).Returns(expected);

            var result = _controller.Get("test") as OkObjectResult;
            Assert.AreEqual(expected, result?.Value);
        }

        [Test]
        public void Get_WhenError_LogsAndReturnsExpected()
        {
            var expected = new Exception("test exception");
            _mockRepo.Setup(s => s.GetByAuthId("test")).Throws(expected);

            var result = _controller.Get("test") as StatusCodeResult;
            _mockLogger.Verify(x => x.LogError(expected, expected.Message), Times.Once);
            Assert.AreEqual(500, result?.StatusCode);
        }
    }
}
