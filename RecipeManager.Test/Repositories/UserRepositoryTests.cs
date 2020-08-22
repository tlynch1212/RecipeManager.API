using NUnit.Framework;
using RecipeManager.Core;
using RecipeManager.Core.Models;
using RecipeManager.Core.Repositories;
using RecipeManager.Test.TestHelpers;

namespace RecipeManager.Test.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private UserRepository _userRepository;
        private RecipeManagerContext _recipeManagerContext;

        [SetUp]
        public void SetUp()
        {
            _recipeManagerContext = RecipeManagerContextHelper.CreateTestContext();
            _userRepository = new UserRepository(_recipeManagerContext);
        }

        [Test]
        public void GetUser_ReturnsExpected()
        {
            var expectedUser = new User
            {
                Id = 1,
                AuthId = "authO|43124214321"
            };

            RecipeManagerContextHelper.AddUser(_recipeManagerContext, expectedUser);
            var result = _userRepository.GetByAuthId(expectedUser.AuthId);

            Assert.AreEqual(expectedUser, result);
        }
    }
}
