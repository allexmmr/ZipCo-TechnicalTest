using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ZipCo.Data.Entities;
using ZipCo.Data.Repositories.Interfaces;

namespace ZipCo.Data.Repositories
{
    public class AccountRepository : IRepository<Account>
    {
        private readonly ZipCoContext _context;

        public AccountRepository(ZipCoContext context)
        {
            _context = context;
        }

        public Account Create(Account account)
        {
            _context.Add(account);
            _context.SaveChanges();

            return account;
        }

        public IEnumerable<Account> List()
        {
            return _context.Accounts.Include(x => x.User).Select(x => x).ToList();
        }
    }
}