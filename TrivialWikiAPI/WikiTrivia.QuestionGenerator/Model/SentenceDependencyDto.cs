namespace WikiTrivia.QuestionGenerator.Model
{
    public class SentenceDependencyDto
    {
        public string Dep { get; }
        public string Governor { get; }
        public string GovernorGloss { get; }
        public string Dependent { get; }
        public string DependentGloss { get; }

        public SentenceDependencyDto() { }

        public SentenceDependencyDto(string dep, string governor, string governorGloss, string dependent,
          string dependentGloss)
        {
            Dep = dep;
            Governor = governor;
            GovernorGloss = governorGloss;
            Dependent = dependent;
            DependentGloss = dependentGloss;
        }
    }
}
