using Base.Domain.Interfaces.Repositories;
using Base.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Base.Domain.Services
{
    public abstract class ServiceBase<T> : IDisposable, IServiceBase<T> where T : class
    {
        private IRepositoryBase<T> _repository;

        public ServiceBase(IRepositoryBase<T> repository)
        {
            try
            {
                _repository = repository;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Add(ref T entity)
        {
            try { 
                return _repository.Add(ref entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            try
            { 
                _repository.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<T> GetAll()
        {
            try
            { 
                return _repository.GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T GetById(long id, long? id2 = null)
        {
            try
            { 
                return _repository.GetById(id, id2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<T> Filter(string criteria, object parameters)
        {
            try
            {
                return _repository.Filter(criteria, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<T> FilterOnlyID(string criteria, object parameters)
        {
            try
            {
                return _repository.FilterOnlyID(criteria, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Remove(T entity)
        {
            try
            {
                _repository.Remove(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(T entity)
        {
            try
            {
                _repository.Update(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
}
    }
}
