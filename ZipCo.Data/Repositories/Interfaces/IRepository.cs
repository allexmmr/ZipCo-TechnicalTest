using System.Collections.Generic;

namespace ZipCo.Data.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        IEnumerable<T> List();
        T Create(T obj);
    }
}