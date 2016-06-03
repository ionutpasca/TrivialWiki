using DatabaseManager.DatabaseModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WikiTrivia.TriviaCore.Models;

namespace WikiTrivia.TriviaCore
{
    public static class QuestionsManager
    {
        public static async Task<TriviaQuestionDto> GetNewQuestion()
        {
            using (var databaseContext = new DatabaseContext())
            {
                var rand = new Random();
                var noOfQuestions = await databaseContext.QuestionSets.CountAsync();
                //var questionsToSkip = rand.Next() * noOfQuestions;
                var questionsToSkip = 0;
                return await databaseContext.QuestionSets
                    .OrderBy(u => u.Id)
                    .Skip(questionsToSkip)
                    .Take(1)
                    .Select(q => new TriviaQuestionDto()
                    {
                        QuestionText = q.QuestionText,
                        Answer = q.CorrectAnswer,
                        Timestamp = DateTime.Now
                    }).FirstAsync();
            }
        }
    }
}
