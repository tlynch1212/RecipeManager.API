using RecipeManager.Core.Models;

namespace RecipeManager.Core.Repositories
{
    public interface IUserRepository
    {
        void CreateUser(User user, bool save);
        User GetByAuthId(string authId);
        void SaveChanges();
    }
}
