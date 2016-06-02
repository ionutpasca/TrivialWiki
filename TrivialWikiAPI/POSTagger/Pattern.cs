using java.util;

namespace POSTagger
{
    public class Pattern
    {
        private static ArrayList _patternList;
        private static string _fileName;
        public Pattern(string filename)
        {
            _fileName = filename;
            _patternList = new ArrayList();
            GetPatterns();
        }

        private static void GetPatterns()
        {
            var lines = System.IO.File.ReadAllLines(_fileName);

            foreach (var line in lines)
            {
                var tags = line.Split('-');
                var toAdd = new ArrayList();
                foreach (var tag in tags)
                {
                    toAdd.add(tag);
                }
                _patternList.add(toAdd);
            }
        }

        private static int SentenceContainsPattern(ArrayList sentence)
        {
            return -1;
        }
    }
}
