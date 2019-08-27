using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KuberAPI.Interfaces.Services;
using KuberAPI.Models;

namespace KuberAPI.Services
{
    public class UserService : IEntityService<User>
    {
        private KuberContext Context;
        public UserService(KuberContext context)
        {
            Context = context;
        }

        public int Create(User user)
        {
            Context.Users.Add(user);
            Context.SaveChanges();

            return user.UserId;
        }

        public User Get(Func<User, bool> filter)
        {
            return Context.Users.Where(filter).FirstOrDefault();
        }

        public int Count(Func<User, bool> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetMany(Func<User, bool> filter)
        {
            throw new NotImplementedException();
        }

        public User Update(Action updateFn, User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
