using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class DbConnectionFactory 
    {
       
        private readonly IMongoDatabase _mongodbDatabase = null;
        private readonly IConfiguration _configuration;
        public DbConnectionFactory(IConfiguration configuration)
        {
           
            this._configuration = configuration;
            var mongodbConfig = _configuration["mongo:connectionString"];
            if (mongodbConfig != null)
            {
                var client = new MongoClient(mongodbConfig);
                if (client != null)
                    _mongodbDatabase = client.GetDatabase(configuration["mongo:databaseName"]);
            }
        }
        public IMongoDatabase GetMongoDatabase
        {
            get
            {
                return _mongodbDatabase;
            }
        }
    }
}
