using System;
using System.Collections.Generic;
using System.Linq;
using ZipCo.Data.Entities;
using ZipCo.Data.Repositories.Interfaces;

namespace ZipCo.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ZipCoContext _context;

        public UserRepository(ZipCoContext context)
        {
            _context = context;
        }

        public User Create(User user)
        {
            _context.Add(user);
            _context.SaveChanges();

            return user;
        }

        public IEnumerable<User> List()
        {
            return _context.Users.ToList();
        }

        public User Get(string emailAddress)
        {
            return _context.Users.ToList().FirstOrDefault(x => string.Equals(x.EmailAddress, emailAddress, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}