using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Base.Domain.Interfaces.Services
{
    public interface IServiceBase<T> where T : class
    {
        int Add(ref T entity);
        void Update(T entity);
        void Remove(T entity);
        T GetById(long id, long? id2 = null);
        IEnumerable<T> GetAll();
        IEnumerable<T> Filter(string criteria, object parameters);
        IEnumerable<T> FilterOnlyID(string criteria, object parameters);
        void Dispose();
    }
}
