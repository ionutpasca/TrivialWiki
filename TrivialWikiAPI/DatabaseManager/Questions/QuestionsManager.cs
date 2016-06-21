using DatabaseManager.DatabaseModels;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseManager.Questions
{
    public sealed class QuestionsManager
    {

        public async Task UpdateQuestions(IEnumerable<UpdatedQuestion> questions)
        {
            using (var databaseContext = new DatabaseContext())
            {
                foreach (var question in questions)
                {
                    var dbQuestion = await databaseContext.QuestionSets.SingleOrDefaultAsync(q => q.Id == question.Id);
                    if (dbQuestion == null)
                    {
                        continue;
                    }
                    dbQuestion.QuestionText = question.Question;
                    dbQuestion.IsValidated = true;
                }
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task DeleteQuestions(IEnumerable<int> questionIds)
        {
            using (var databaseContext = new DatabaseContext())
            {
                foreach (var questionId in questionIds)
                {
                    databaseContext.QuestionSets
                        .Remove(databaseContext.QuestionSets.SingleOrDefault(q => q.Id == questionId));
                }
                await databaseContext.SaveChangesAsync();
            }
        }

    }
}
