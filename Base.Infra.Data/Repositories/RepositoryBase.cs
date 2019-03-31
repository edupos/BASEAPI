using Dapper;
using Base.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Base.Infra.Data.Repositories
{
    public abstract class RepositoryBase<T> : IDisposable, IRepositoryBase<T> where T : class
    {
        private string _idField;
        private string _idField2;

        private readonly IConfiguration _config;

        public RepositoryBase(IConfiguration config, string idField)
        {
            _idField = idField;
            _config = config;
        }
        public RepositoryBase(IConfiguration config, string idField, string idField2)
        {
            _idField = idField;
            _idField2 = idField2;
            _config = config;
        }

        public int Add(ref T entity)
        {
            try
            {
                var propertyContainer = ParseProperties(entity);
                var sql = string.Format("INSERT INTO [{0}] ({1}) VALUES(@{2})" +
                    (_idField2 == null ? " SELECT CAST(scope_identity() AS int)" : ""),
                typeof(T).Name,
                "[" + string.Join("], [", propertyContainer.ValueNames) + "]" +
                    (_idField2 != null ? ", [" + string.Join("], [", propertyContainer.IdNames) + "]" : ""),
                string.Join(", @", propertyContainer.ValueNames) +
                    (_idField2 != null ? ", @" + string.Join(", @", propertyContainer.IdNames) : "")
                );

                using (var con = GetConnection())
                {
                    if (_idField2 != null)
                    {
                        List<string> id1 = new List<string>();
                        id1.Add(propertyContainer.IdNames.ToList()[0]);
                        List<string> id2 = new List<string>();
                        id2.Add(propertyContainer.IdNames.ToList()[1]);
                        propertyContainer.ValuePairs.Add(propertyContainer.IdNames.ToList()[0], propertyContainer.IdPairs[propertyContainer.IdNames.ToList()[0]]);
                        propertyContainer.ValuePairs.Add(propertyContainer.IdNames.ToList()[1], propertyContainer.IdPairs[propertyContainer.IdNames.ToList()[1]]);
                    }
                    int id = 0;
                    if (_idField2 == null)
                    {
                        id = con.Query<int>
                        (sql, propertyContainer.ValuePairs, commandType: CommandType.Text).FirstOrDefault();
                        SetId(entity, id, propertyContainer.IdPairs);
                    }
                    else
                    {
                        con.Execute(sql, propertyContainer.ValuePairs, commandType: CommandType.Text);
                    }

                    return id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {

        }

        protected IEnumerable<R> GetRelationship<F, S, R>(string sql, Func<F, S, R> map, string splitOn)
        {
            using (var con = GetConnection())
            {
                return con.Query<F, S, R>(sql, map, splitOn: splitOn);
            }
        }
        protected IEnumerable<R> GetRelationship<F, S, TH, R>(string sql, Func<F, S, TH, R> map, string splitOn)
        {
            using (var con = GetConnection())
            {
                return con.Query<F, S, TH, R>(sql, map, splitOn: splitOn);
            }
        }

        protected IEnumerable<R> GetRelationship<F, S, TH, FO, R>(string sql, Func<F, S, TH, FO, R> map, string splitOn)
        {
            using (var con = GetConnection())
            {
                return con.Query<F, S, TH, FO, R>(sql, map, splitOn: splitOn);
            }
        }

        protected IEnumerable<R> GetRelationship<F, S, TH, FO, FI, R>(string sql, Func<F, S, TH, FO, FI, R> map, string splitOn)
        {
            using (var con = GetConnection())
            {
                return con.Query<F, S, TH, FO, FI, R>(sql, map, splitOn: splitOn);
            }
        }

        protected IEnumerable<R> GetRelationship<F, S, TH, FO, FI, SI, R>(string sql, Func<F, S, TH, FO, FI, SI, R> map, string splitOn)
        {
            using (var con = GetConnection())
            {
                return con.Query<F, S, TH, FO, FI, SI, R>(sql, map, splitOn: splitOn, commandTimeout: 0);
            }
        }

        protected IEnumerable<R> GetRelationship<F, S, TH, FO, FI, SI, SE, R>(string sql, Func<F, S, TH, FO, FI, SI, SE, R> map, string splitOn)
        {
            using (var con = GetConnection())
            {
                return con.Query<F, S, TH, FO, FI, SI, SE, R>(sql, map, splitOn: splitOn);
            }
        }

        public IEnumerable<T> GetAll()
        {
            try
            {

                var connectionString = this.GetConnection();
                IEnumerable<T> result;

                using (var con = GetConnection())
                {
                    try
                    {
                        con.Open();
                        var query = "SELECT * FROM " + typeof(T).Name;
                        result = con.Query<T>(query).ToList();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }

                    return result;
                }
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

                var connectionString = this.GetConnection();
                T result;

                using (var con = GetConnection())
                {
                    try
                    {
                        if (!String.IsNullOrEmpty(_idField2) && id2 == null)
                        {
                            throw new Exception("É necessário 2 id's para selecionar esse registro.");
                        }

                        con.Open();
                        if (!String.IsNullOrEmpty(_idField2))
                        {
                            var query = "SELECT * FROM " + typeof(T).Name + " WHERE " + _idField + " = " + id + " and " + _idField2 + " = " + id2;
                            result = con.Query<T>(query).FirstOrDefault();
                        }
                        else
                        {
                            var query = "SELECT * FROM " + typeof(T).Name + " WHERE " + _idField + " = " + id;
                            result = con.Query<T>(query).FirstOrDefault();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }

                    return result;
                }
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

                var propertyContainer = ParseProperties(entity);
                var sqlIdPairs = GetSqlPairs(propertyContainer.IdNames);
                var sql = string.Format("DELETE FROM [{0}] WHERE {1}", typeof(T).Name, sqlIdPairs);
                using (var con = GetConnection())
                {
                    con.Execute(sql, propertyContainer.AllPairs, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<T> Filter(string criteria, object parameters)
        {
            var sql = string.Format("SELECT * FROM [{0}] WHERE {1}", typeof(T).Name, criteria);
            return CustomSQL<T>(CommandType.Text, sql, parameters);
        }

        public IEnumerable<T> FilterOnlyID(string criteria, object parameters)
        {
            string idField = "";
            if (!String.IsNullOrEmpty(_idField2))
            {
                idField =  _idField + ", " + _idField2;
            }
            else
            {
                idField = _idField;
            }

            var sql = string.Format("SELECT " + idField + " FROM [{0}] WHERE {1}", typeof(T).Name, criteria);
            return CustomSQL<T>(CommandType.Text, sql, parameters);
        }

        protected IEnumerable<Z> CustomSQL<Z>(CommandType commandType, string sql, object parameters)
        {
            using (var con = GetConnection())
            {
                return con.Query<Z>(sql, parameters, commandType: commandType);
            }
        }

        public void Update(T entity)
        {
            try
            {
                var propertyContainer = ParseProperties(entity);
                var sqlIdPairs = GetSqlPairs(propertyContainer.IdNames);
                var sqlValuePairs = GetSqlPairs(propertyContainer.ValueNames);
                var sql = string.Format("UPDATE [{0}] SET {1} WHERE {2}", typeof(T).Name, sqlValuePairs, sqlIdPairs.Replace(", ", " and "));
                using (var con = GetConnection())
                {
                    con.Execute(sql, propertyContainer.AllPairs, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlConnection GetConnection()
        {
            try
            {
                string connectionString = _config["ConnectionStrings:DefaultConnection"];
                return new SqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region métodos de apoio para o Dapper Generico

        private class PropertyContainer
        {
            private readonly Dictionary<string, object> _ids;
            private readonly Dictionary<string, object> _values;

            #region Properties

            internal IEnumerable<string> IdNames
            {
                get { return _ids.Keys; }
            }

            internal IEnumerable<string> ValueNames
            {
                get { return _values.Keys; }
            }

            internal IEnumerable<string> AllNames
            {
                get { return _ids.Keys.Union(_values.Keys); }
            }

            internal IDictionary<string, object> IdPairs
            {
                get { return _ids; }
            }

            internal IDictionary<string, object> ValuePairs
            {
                get { return _values; }
            }

            internal IEnumerable<KeyValuePair<string, object>> AllPairs
            {
                get { return _ids.Concat(_values); }
            }

            #endregion

            #region Constructor

            internal PropertyContainer()
            {
                _ids = new Dictionary<string, object>();
                _values = new Dictionary<string, object>();
            }

            #endregion

            #region Methods

            internal void AddId(string name, object value)
            {
                _ids.Add(name, value);
            }

            internal void AddValue(string name, object value)
            {
                _values.Add(name, value);
            }

            #endregion
        }

        private void SetId(T obj, int id, IDictionary<string, object> propertyPairs)
        {
            if (propertyPairs.Count == 1)
            {
                var propertyName = propertyPairs.Keys.First();
                var propertyInfo = obj.GetType().GetProperty(propertyName);
                if (propertyInfo.PropertyType == typeof(int))
                {
                    propertyInfo.SetValue(obj, id, null);
                }
            }
        }

        private PropertyContainer ParseProperties(T obj)
        {
            var propertyContainer = new PropertyContainer();

            var typeName = typeof(T).Name;
            var validKeyNames = new[] { _idField }; // { "Id", string.Format("{0}Id", typeName), string.Format("{0}_Id", typeName) };
            if (!String.IsNullOrEmpty(_idField2)) validKeyNames = new[] { _idField, _idField2 };

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                // Skip reference types (but still include string!)
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    continue;

                // Skip methods without a public setter
                if (property.GetSetMethod() == null)
                    continue;

                // Skip methods specifically ignored
                if (property.IsDefined(typeof(DapperIgnore), false))
                    continue;

                var name = property.Name;
                var value = typeof(T).GetProperty(property.Name).GetValue(obj, null);

                if (property.IsDefined(typeof(DapperKey), false) || validKeyNames.Contains(name))
                {
                    propertyContainer.AddId(name, value);
                }
                else
                {
                    propertyContainer.AddValue(name, value);
                }
            }

            return propertyContainer;
        }

        private static string GetSqlPairs(IEnumerable<string> keys, string separator = ", ")
        {
            var pairs = keys.Select(key => string.Format("[{0}]=@{0}", key)).ToList();
            return string.Join(separator, pairs);
        }

        #endregion

    }
    [AttributeUsage(AttributeTargets.Property)]
    public class DapperKey : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DapperIgnore : Attribute
    {
    }
}
