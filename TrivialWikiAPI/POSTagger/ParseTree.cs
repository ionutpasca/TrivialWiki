using System.Collections;
using System.Collections.Generic;

namespace POSTagger
{
    public class ParseTree
    {
        public ParseTree()
        {
            Children = new List<ParseTree>();
        }
        public ParseTree(string parse, int level)
        {
            Children = new List<ParseTree>();
            this.StringParse = parse;
            this.Level = level;

        }
        public ParseTree(ArrayList parse, int level)
        {
            Children = new List<ParseTree>();
            this.ListParse = parse;
            this.Level = level;
        }

        public int Level { get; set; }
        public List<ParseTree> Children { get; set; }
        public string Value { get; set; }
        public string Token { get; set; }
        public ParseTree Parent { get; set; }
        public string StringParse { get; set; }
        public ArrayList ListParse { get; set; }

        public void ParseSubTrees()
        {
            if (ListParse.Count == 0) return;
            var lastIndex = -1;
            for (var i = 0; i < ListParse.Count; i++)
            {
                var element = (string)ListParse[i];
                var elemParts = element.Split(new char[] { '(' }, 2);
                var elemLevel = int.Parse(elemParts[0]);
                var elemValue = elemParts[1];
                var elemValueParts = elemValue.Split(new char[] { ' ' }, 2);
                if (elemLevel / 2 == Level)
                {
                    this.Value = elemValueParts[0];
                    if (elemValueParts.Length == 1)
                    {
                        continue;
                    }
                    this.Token = elemValueParts[1];
                    TrimToken();
                    continue;
                }
                if (elemLevel / 2 == Level + 1)
                {
                    if (lastIndex != -1)
                    {
                        var subTree = new ParseTree(CopyPart(lastIndex, i - 1), Level + 1);
                        Children.Add(subTree);
                    }
                    lastIndex = i;
                }
                if (i == ListParse.Count - 1)
                {
                    var subTree = new ParseTree(CopyPart(lastIndex, i), Level + 1);
                    Children.Add(subTree);
                }
            }

            foreach (var child in Children)
            {
                child.ParseSubTrees();
            }
        }

        private ArrayList CopyPart(int start, int end)
        {
            var returnList = new ArrayList();
            if (end >= ListParse.Count) return returnList;

            for (var i = start; i <= end; i++)
            {
                returnList.Add(ListParse[i]);
            }

            return returnList;
        }

        public override string ToString()
        {
            var toReturn = "";

            toReturn = Value + " " + Token + "\r\n";
            foreach (var child in Children)
            {
                toReturn += child.ToString();
            }

            return toReturn;
        }

        private void TrimToken()
        {
            int pIndex = 0;
            for (var i = 0; i < Token.Length; i++)
            {
                if (Token[i] == '(')
                {
                    pIndex--;
                }
                else if (Token[i] == ')')
                {
                    pIndex++;
                }
            }
            if (pIndex < 1) return;
            Token = Token.Remove(Token.Length - pIndex);
        }
    }
}
