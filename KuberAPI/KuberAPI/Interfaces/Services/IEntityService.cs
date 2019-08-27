using System;
using System.Collections.Generic;

namespace KuberAPI.Interfaces.Services
{
    public interface IEntityService<T>
    {
        int Create(T model);
        T Get(Func<T, bool> filter);
        int Count(Func<T, bool> filter);
        IEnumerable<T> GetMany(Func<T, bool> filter);
        T Update(Action updateFn, T model);
        void Delete(int id);
    }
}
