using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.Data;
using Moq;
using NUnit.Framework;
using RecipeManager.API;
using RecipeManager.API.Controllers;
using RecipeManager.Core.Models;
using RecipeManager.Core.Recommendations;
using RecipeManager.Core.Repositories;

namespace RecipeManager.Test.Controllers
{
    [TestFixture]
    class RecommendationControllerTests
    {
        private RecommendationController _controller;
        private Mock<IUserRepository> _mockRepo;
        private Mock<ITrainer> _mockTrainer;
        private Mock<IPredictor> _mockPredictor;
        private Mock<ILoggerWrapper> _mockLogger;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILoggerWrapper>();
            _mockPredictor = new Mock<IPredictor>();
            _mockTrainer = new Mock<ITrainer>();
            _controller = new RecommendationController(_mockLogger.Object,_mockTrainer.Object, _mockPredictor.Object, _mockRepo.Object);
        }

        [Test]
        public void Train_ReturnsExpected()
        {
            var expected = FormatterServices.GetUninitializedObject(typeof(RegressionMetrics)) as RegressionMetrics;
            _mockTrainer.Setup(t => t.Train(It.IsAny<int>())).Returns(expected);

            var result = _controller.Train(1) as OkObjectResult;
            Assert.AreEqual(expected, result?.Value);
        }

        [Test]
        public void Predict_ReturnsExpected()
        {
            var expected = new List<Recipe>();
            _mockRepo.Setup(s => s.GetByAuthId("test")).Returns(new User
            {
                Id = 1,
                AuthId = "test"
            });
            _mockPredictor.Setup(s => s.Predict(It.IsAny<int>(), It.IsAny<int>())).Returns(expected);

            var result = _controller.Predict("test", 1) as OkObjectResult;
            Assert.AreEqual(expected, result?.Value);
        }

        [Test]
        public void Train_WhenError_LogsAndReturnsExpected()
        {
            var expected = new Exception("test exception");
            _mockTrainer.Setup(s => s.Train(It.IsAny<int>())).Throws(expected);

            var result = _controller.Train(1) as StatusCodeResult;
            _mockLogger.Verify(x => x.LogError(expected, expected.Message), Times.Once);
            Assert.AreEqual(500, result?.StatusCode);
        }

        [Test]
        public void Predict_WhenError_LogsAndReturnsExpected()
        {
            var expected = new Exception("test exception");
            _mockRepo.Setup(s => s.GetByAuthId("test")).Returns(new User
            {
                Id = 1,
                AuthId = "test"
            });
            _mockPredictor.Setup(s => s.Predict(It.IsAny<int>(), It.IsAny<int>())).Throws(expected);

            var result = _controller.Predict("test", 1) as StatusCodeResult;
            _mockLogger.Verify(x => x.LogError(expected, expected.Message), Times.Once);
            Assert.AreEqual(500, result?.StatusCode);
        }
    }
}
