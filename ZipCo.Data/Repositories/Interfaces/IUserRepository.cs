using ZipCo.Data.Entities;

namespace ZipCo.Data.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User Get(string emailAddress);
    }
}