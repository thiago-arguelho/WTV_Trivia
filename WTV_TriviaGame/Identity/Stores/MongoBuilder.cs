using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTV_TriviaGame.Identity.Stores
{
    public class MongoBuilder
    {
        private readonly string _connectionString;

        public const string COLLECTION_USERS = "Users";
        public const string COLLECTION_ROLES = "Roles";
        public const string COLLECTION_TRIVIA = "Trivia";
        /// <summary>
        /// Provides the connection string
        /// </summary>
        /// <param name="configuration"></param>
        public MongoBuilder(IConfiguration configuration)
        {
            _connectionString = configuration["MongoConnnectionString"];
        }
        /// <summary>
        /// Get Mongo Collection with the provided parameter name 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>(string collection)
        {
            MongoClient client = new MongoClient(_connectionString);
            var db = client.GetDatabase("TriviaDB");
            return db.GetCollection<T>(collection);
        }
    }
}
