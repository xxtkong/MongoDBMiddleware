using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryMongoDB<TEntity> : IRepositoryMongoDB<TEntity> where TEntity : class
    {
        private readonly DbConnectionFactory _dbConnection;
        public RepositoryMongoDB(DbConnectionFactory connection)
        {
            this._dbConnection = connection;
        }
        public async Task Add(TEntity item)
        {
            await _dbConnection.GetMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name).InsertOneAsync(item);
        }

        public async Task<string> CreateIndex(Expression<Func<TEntity, object>> field)
        {
            IndexKeysDefinition<TEntity> keys = Builders<TEntity>.IndexKeys.Ascending(field);

            return await _dbConnection.GetMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name)
                            .Indexes.CreateOneAsync(new CreateIndexModel<TEntity>(keys));
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter, FindOptions options = null)
        {
            return await _dbConnection.GetMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name)
                            .Find(filter, options)
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetMany(Expression<Func<TEntity, bool>> filter, FindOptions options = null)
        {
            return await _dbConnection.GetMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name)
                            .Find(filter, options)
                            .ToListAsync();
        }
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter, FindOptions options = null)
        {
            return _dbConnection.GetMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name)
                            .Find(filter, options)
                            .ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _dbConnection.GetMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name).Find(_ => true).ToListAsync();
        }

        public List<TEntity> GetAllList()
        {
            return _dbConnection.GetMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name).Find(_ => true).ToList();
        }


        public async Task<bool> Remove(string id)
        {

            DeleteResult actionResult = await _dbConnection.GetMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name).DeleteOneAsync(
                     Builders<TEntity>.Filter.Eq("id", id));
            return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
        }

        public async Task<bool> RemoveAll()
        {
            DeleteResult actionResult = await _dbConnection.GetMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name).DeleteManyAsync(new BsonDocument());
            return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
        }

        public async Task<bool> Update(Expression<Func<TEntity, bool>> filter, UpdateDefinition<TEntity> update)
        {
            try
            {
                UpdateResult actionResult = await _dbConnection.GetMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name)
                    .UpdateOneAsync(filter, update);
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> Update(Expression<Func<TEntity, bool>> filter, TEntity Entity)
        {
            try
            {
                ReplaceOneResult actionResult = await _dbConnection.GetMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name)
                                                .ReplaceOneAsync(filter
                                                                , Entity
                                                                , new UpdateOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }
        public ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId internalId))
                internalId = ObjectId.Empty;
            return internalId;
        }

        public IEnumerable<TEntity> GetPage(Expression<Func<TEntity, bool>> filter, out long pcount, FindOptions options = null, int? pageIndex = 1, int? pageSize = 10)
        {
            var result = _dbConnection.GetMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name)
                           .Find(filter, options).Skip((pageIndex - 1) * pageSize).Limit(pageSize)
                           .ToList();
            pcount = _dbConnection.GetMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name).Find(filter, options).Count();
            return result;
        }
        public async Task<bool> Remove(Expression<Func<TEntity, bool>> filter)
        {
            DeleteResult actionResult = await _dbConnection.GetMongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name).DeleteOneAsync(filter);
            return actionResult.IsAcknowledged && actionResult.DeletedCount > 0;
        }
    }
}
