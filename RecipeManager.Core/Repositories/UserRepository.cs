using RecipeManager.Core.Models;
using System.Linq;

namespace RecipeManager.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly RecipeManagerContext _dbContext;

        public UserRepository(RecipeManagerContext context)
        {
            _dbContext = context;
        }

        public User GetByAuthId(string authId)
        {
            return _dbContext.Users.FirstOrDefault(t => t.AuthId == authId);
        }
    }
}
