using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WTV_TriviaGame.Models
{
    public class TriviaQuestion
    {
        public string Category { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string Question { get; set; }
        public string UserAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public List<TriviaAnswer> Answers { get; set; }

        public async Task<List<TriviaQuestion>> GetQuestionsFromAPI()
        {
            List<TriviaQuestion> triviaQuestionList = new List<TriviaQuestion>();
            List<JToken> PerguntasJSON = new List<JToken>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://opentdb.com/api.php?amount=10&category=9&difficulty=medium&type=multiple"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    JObject responseObj = JObject.Parse(apiResponse);
                    PerguntasJSON = responseObj["results"].ToList();
                }
            }

            foreach (JToken item in PerguntasJSON)
            {
                TriviaQuestion question = new TriviaQuestion();

                question.Category = item["category"].ToString();
                question.CorrectAnswer = item["correct_answer"].ToString();
                question.Difficulty = item["difficulty"].ToString();
                question.Question = item["question"].ToString();
                //Adds incorrect answers to a list of TriviaAnswer and adds the CorrectAnswer (The API provides separeted data incorrect_answers & correct_answer)
                List<TriviaAnswer> triviaAnswers = item["incorrect_answers"].Select(c => new TriviaAnswer() { Value = c.ToString() }).ToList();
                TriviaAnswer cAnswer = new TriviaAnswer { Value = item.SelectToken("correct_answer").ToString() };
                triviaAnswers.Add(cAnswer);
                ///

                question.Answers = triviaAnswers.OrderBy(x => Guid.NewGuid()).ToList();

                triviaQuestionList.Add(question);
            }
            return triviaQuestionList;
        }
    }
}
