using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WTV_TriviaGame.Models;

namespace WTV_TriviaGame.Identity.Stores
{
    public class MongoTrivia : Trivia
    {
        private readonly IMongoCollection<Trivia> _trivia;

        public MongoTrivia(MongoBuilder mongoBuilder)
        {
            _trivia = mongoBuilder.GetCollection<Trivia>(MongoBuilder.COLLECTION_TRIVIA);
        }
        /// <summary>
        /// Create trivia on DB
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task CreateAsync(Trivia trivia, CancellationToken cancellationToken)
        {
            _trivia.InsertOneAsync(trivia, cancellationToken: cancellationToken);
            return Task.CompletedTask;
        }
    }
}
