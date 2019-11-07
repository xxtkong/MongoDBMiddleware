using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepositoryMongoDB<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();

        Task<TEntity> Get(Expression<Func<TEntity, bool>> filter, FindOptions options = null);
        Task<IEnumerable<TEntity>> GetMany(Expression<Func<TEntity, bool>> filter, FindOptions options = null);
        Task Add(TEntity item);
        Task<bool> Remove(string id);
        /// <summary>
        ///  var update = Builders<TestViewModel>.Update.Set(s => s.Name, "王五");
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        Task<bool> Update(Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> update);
        Task<bool> Update(Expression<Func<TEntity, bool>> filter, TEntity Entity);
        Task<bool> RemoveAll();
        /// <summary>
        /// 这个以后在改把 。。。暂时就这样
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        Task<string> CreateIndex(Expression<Func<TEntity, object>> field);
        ObjectId GetInternalId(string id);
        IEnumerable<TEntity> GetPage(Expression<Func<TEntity, bool>> filter, out long pcount, FindOptions options = null, int? pageIndex = 1, int? pageSize = 10);
        Task<bool> Remove(Expression<Func<TEntity, bool>> filter);
    }
}
