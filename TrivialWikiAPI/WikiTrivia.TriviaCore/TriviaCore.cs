using DatabaseManager.Trivia;
using Microsoft.AspNet.SignalR;
using WikiTrivia.TriviaCore.Hubs;
using WikiTrivia.Utilities;

namespace WikiTrivia.TriviaCore
{
    public static class CurrentTriviaQuestion
    {
        public static TriviaQuestionDto currentTriviaQuestion { get; set; }
        public static int numberOfWrongAnswers { get; set; }
        public static int hintCommandsCount { get; set; }
    }

    public sealed class TriviaCore
    {
        private static readonly TriviaManager triviaManager = new TriviaManager();

        public void BroadcastQuestion()
        {
            if (TriviaUserHandler.ConnectedIds.Count == 0)
            {
                return;
            }
            InitializeCurrentTriviaQuestion();

            var questionMessage = CurrentTriviaQuestion.currentTriviaQuestion.QuestionText;
            var questionToSend = new TriviaMessageDto() { Sender = "TriviaBot", MessageText = questionMessage };

            triviaManager.AddTriviaMessageToDatabase(questionToSend);

            var context = GlobalHost.ConnectionManager.GetHubContext<TriviaHub>();
            context.Clients.All.AddMessage(questionToSend);
        }
        public void BroadcastHint(int hintNumber)
        {
            var hint = GetHintByNumber(hintNumber);
            if (hint == string.Empty)
            {
                return;
            }
            var questionToSend = new TriviaMessageDto() { Sender = "TriviaBot", MessageText = $"Hint : {hint}" };
            triviaManager.AddTriviaMessageToDatabase(questionToSend);

            var context = GlobalHost.ConnectionManager.GetHubContext<TriviaHub>();
            context.Clients.All.AddMessage(questionToSend);
        }

        private static void InitializeCurrentTriviaQuestion()
        {
            CurrentTriviaQuestion.currentTriviaQuestion = triviaManager.GetNewQuestion();
            CurrentTriviaQuestion.numberOfWrongAnswers = 0;
            InitializeHintsForCurrentQuestion();
        }

        private static void InitializeHintsForCurrentQuestion()
        {
            var answer = CurrentTriviaQuestion.currentTriviaQuestion.Answer;

            var hint1 = TriviaHintGenerator.GenerateHintForQuestion(answer);
            var hint2 = TriviaHintGenerator.GenerateHintForQuestion(answer, hint1);
            var hint3 = TriviaHintGenerator.GenerateHintForQuestion(answer, hint2);

            CurrentTriviaQuestion.currentTriviaQuestion.Hint = new QuestionHint()
            {
                FirstHint = hint1,
                SecondHint = hint2,
                ThirdHint = hint3
            };
        }

        private static string GetHintByNumber(int hintNumber)
        {
            switch (hintNumber)
            {
                case 1:
                    return CurrentTriviaQuestion.currentTriviaQuestion.Hint.FirstHint;
                case 2:
                    return CurrentTriviaQuestion.currentTriviaQuestion.Hint.SecondHint;
                case 3:
                    return CurrentTriviaQuestion.currentTriviaQuestion.Hint.ThirdHint;
                default:
                    return string.Empty;
            }
        }
    }
}
