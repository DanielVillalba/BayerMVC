using PSD.Model;
using PSD.Repository.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PSD.Repository.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(PSDContext context)
            : base(context)
        {
        }

        public User Get(int id)
        {
            return PSDContext.Users.Include(x => x.RolesXUser).FirstOrDefault(a => a.Id == id && a.Cat_UserStatusId != 2);//2:inactive (deleted)
        }
        public User GetByNickName(string nickName)
        {
            return PSDContext.Users.Include(x => x.RolesXUser).FirstOrDefault(a => a.NickName == nickName && a.Cat_UserStatusId != 2);//2:inactive (deleted)
        }
        public User GetByToken(string token)
        {
            return PSDContext.Users.Include(x => x.RolesXUser).FirstOrDefault(a => a.LoginToken == token);
        }

    }
}