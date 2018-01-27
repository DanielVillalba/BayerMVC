using PSD.Model;
using System.Collections.Generic;

namespace PSD.Repository.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByNickName(string nickName);
        User GetByToken(string token);

    }
}