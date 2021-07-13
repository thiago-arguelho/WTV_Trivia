using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WTV_TriviaGame.Models
{
    public class Trivia
    {
        public ObjectId UserId { get; set; }
        public List<TriviaQuestion> Perguntas { get; set; }
        public string Points { get; set; }
    }
}
