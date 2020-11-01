using System.Collections.Generic;
using System.Linq;
using ZipCo.Common.Domain;
using ZipCo.Data.Entities;

namespace ZipCo.Data.Migrations
{
    public static class DbInitialiser
    {
        public static void Initialise(ZipCoContext context)
        {
            context.Database.EnsureCreated();

            // Look for any user
            if (context.Users.Any())
            {
                return; // DB has been seeded
            }

            // Create users
            List<User> users = new List<User>
            {
                new User { Name = "Administrator", EmailAddress = "admin@zip.co", Password = Encryptor.EncryptMD5("Izi5XK0sLgpoHc56Nisv") }
            };

            foreach (User user in users)
            {
                context.Users.Add(user);
            }

            context.SaveChanges();
        }
    }
}