using RecipeManager.Core.Models;

namespace RecipeManager.Core.Repositories
{
    public interface IUserRepository
    {
        User GetByAuthId(string authId);
    }
}
