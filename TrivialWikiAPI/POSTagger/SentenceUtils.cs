using java.util;

namespace POSTagger
{
    public static class SentenceUtils
    {
        public static Map GetMapOfSentence(string sentence)
        {
            Map toReturn = new HashMap();
            var pairs = sentence.Split(' ');
            foreach (var pair in pairs)
            {
                var keyVal = pair.Split('/');
                toReturn.put(keyVal[0], keyVal[1]);
            }
            return toReturn;
        }

        public static ArrayList GetSentenceTags(string sentence)
        {
            var toReturn = new ArrayList();
            var pairs = sentence.Split(' ');
            foreach (var pair in pairs)
            {
                var keyVal = pair.Split('/');
                toReturn.add(keyVal[1]);
            }
            toReturn.remove(toReturn.size() - 1);
            return toReturn;
        }



        public static ArrayList GetMatchingPart(ArrayList sentence, ArrayList tags, Map sentenceParts, ArrayList pattern)
        {
            var toReturn = new ArrayList();
            var startingPosition = TagsContainPattern(tags, pattern);
            if (startingPosition == -1) return toReturn;
            var size = pattern.size() + startingPosition;
            for (var i = startingPosition; i < size; i++)
            {
                toReturn.add(sentence.get(i));
            }
            return toReturn;
        }

        public static int TagsContainPattern(ArrayList tags, ArrayList pattern)
        {
            int index = 0, position = 0;
            foreach (var tag in tags)
            {
                if (tag.Equals(pattern.get(index)))
                    index++;
                else
                    index = 0;
                if (index == pattern.size())
                    return position - (index - 1);
                position++;
            }
            return -1;
        }
    }
}
