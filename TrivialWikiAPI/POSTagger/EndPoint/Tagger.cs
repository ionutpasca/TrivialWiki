using POSTagger.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WikipediaResourceFinder;
using WikiTrivia.QuestionGenerator;
using WikiTrivia.QuestionGenerator.Model;
using WikiTrivia.Utilities;

// ReSharper disable LoopCanBeConvertedToQuery

namespace POSTagger.EndPoint
{
    public sealed class Tagger
    {
        public async Task GetWikipediaResources(string topic)
        {
            var res = new ResourceFinder();
            var rawResultsPath = DirectoryManager.GetRawResultsPath(topic);
            DirectoryManager.CreateDirectoryForTopic(topic);
            await res.GetWikipediaRawText(topic, rawResultsPath);
        }

        public async Task ProcessWikipediaText(string topic)
        {
            var rawResultsPath = DirectoryManager.GetRawResultsPath(topic);
            var cleanTextPath = DirectoryManager.GetCleanResultsPath(topic);
            var outputJsonPath = DirectoryManager.GetOutputJsonPath(topic);
            var referencesPath = DirectoryManager.GetReferencesPath(topic);

            var text = File.ReadAllText(rawResultsPath);
            text = StringUtils.CleanText(text, referencesPath);

            await DirectoryManager.WriteTextToFile(text, cleanTextPath);

            var tpr = new TextProcessing();
            tpr.ProcessText(text, outputJsonPath);
        }

        public void GenerateQuestions(string topic)
        {
            topic = topic.Replace(" ", "_");
            var outputJsonPath = DirectoryManager.GetOutputJsonPath(topic);

            var tpr = new TextProcessing();
            var resultList = tpr.GetSentencesInformationFromJson(outputJsonPath);

            var questionList = new List<TopicQuestion>();
            foreach (var sentence in resultList)
            {
                var dependencies = GetSentenceDependency(sentence);

                var words = GetSentenceWords(sentence);

                var sentenceInfo = new SentenceInformationDto(sentence.SentenceText, dependencies, words);
                if (Helper.SentenceIsInvalid(sentenceInfo))
                {
                    continue;
                }
                if (sentence.Dependencies.Count > 20 && !Helper.SentenceContainsYear(sentenceInfo))
                {
                    continue;
                }
                 var generatedQuestion = QuestionGenerator.Generate(sentenceInfo);
                if (string.IsNullOrEmpty(generatedQuestion?.Question))
                {
                    continue;
                }
                var cleanQuestion = QuestionCleaner.RemovePunctuationFromEnd(generatedQuestion.Question);
                cleanQuestion = $"{cleanQuestion}?";
                var question = new TopicQuestion()
                {
                    Topic = topic,
                    InitialSentence = sentence.SentenceText,
                    Question = cleanQuestion,
                    Answer = generatedQuestion.Answer
                };
                questionList.Add(question);
            }
            DirectoryManager.WriteQuestionsToFile(questionList, topic);
        }

        private static IEnumerable<SentenceDependencyDto> GetSentenceDependency(SentenceInformation sentence)
        {
            return sentence.Dependencies.Select(s => new SentenceDependencyDto(s.Dep, s.Governor,
                    s.GovernorGloss, s.Dependent, s.DependentGloss))
                    .ToList();
        }

        private static IEnumerable<WordInformationDto> GetSentenceWords(SentenceInformation sentence)
        {
            return sentence.Words.Select(w => new WordInformationDto(w.Word, w.PartOfSpeech,
                w.NamedEntityRecognition, w.Lemma))
                .ToList();
        }
    }
}
