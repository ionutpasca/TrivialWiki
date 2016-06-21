using DatabaseManager.Questions;
using Nancy;
using Nancy.ModelBinding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrivialWikiAPI.Questions
{
    public class QuestionModule : NancyModule
    {
        private readonly QuestionsManager questionsManager = new QuestionsManager();
        public QuestionModule()
        {
            Post["/updateQuestions", true] = async (param, p) => await UpdateQuestions();
            Post["/deleteQuestions", true] = async (param, p) => await DeleteQuestions();
        }

        private async Task<object> UpdateQuestions()
        {
            var questions = this.Bind<List<UpdatedQuestion>>();
            await questionsManager.UpdateQuestions(questions);
            return HttpStatusCode.OK;
        }

        private async Task<object> DeleteQuestions()
        {
            var questions = this.Bind<List<int>>();
            await questionsManager.DeleteQuestions(questions);
            return HttpStatusCode.OK;
        }
    }
}