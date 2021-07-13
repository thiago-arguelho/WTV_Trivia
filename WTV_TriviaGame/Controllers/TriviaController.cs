using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using WTV_TriviaGame.Identity.Stores;
using WTV_TriviaGame.Models;

namespace WTV_TriviaGame.Controllers
{
    public class TriviaController : Controller
    {
        public IConfigurationRoot Configuration { get; set; }
        public Trivia NewTrivia = new Trivia();

        [Authorize]
        public async Task<IActionResult> IndexAsync()
        {
            List<TriviaQuestion> triviaQuestionList = await new TriviaQuestion().GetQuestionsFromAPI();

            NewTrivia.Perguntas = triviaQuestionList;
            NewTrivia.Points = "0";
            NewTrivia.UserId = ObjectId.Parse(User.Claims.ToList()[0].Value);
            TempData["Trivia"] = NewTrivia;

            return View();
        }
        public async Task<IActionResult> FinishGame(string userId, string perguntas, string points)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            MongoBuilder mongoBuilder = new MongoBuilder(Configuration);
            Trivia trivia = new Trivia
            {
                UserId = ObjectId.Parse(userId),
                Points = points
            };
            MongoTrivia mongoTrivia = new MongoTrivia(mongoBuilder);
            await mongoTrivia.CreateAsync(trivia, token);
            return RedirectToAction("IndexAsync");
        }
    }
}
