namespace POSTagger
{
    public class SentenceDependency
    {
        public SentenceDependency(string dep, string governor, string governorGloss, string dependent,
            string dependentGloss)
        {
            Dep = dep;
            Governor = governor;
            GovernorGloss = governorGloss;
            Dependent = dependent;
            DependentGloss = dependentGloss;
        }
        public string Dep { get; set; }
        public string Governor { get; set; }
        public string GovernorGloss { get; set; }
        public string Dependent { get; set; }
        public string DependentGloss { get; set; }

        public override string ToString()
        {
            return Dep + "(" + Governor + "-" + GovernorGloss + "," + Dependent + "-" + DependentGloss + ")";
        }
    }
}
