using POSTagger.Model;
using System.Collections.Generic;

namespace POSTagger
{
    public static class SentenceGenerator
    {
        public static IEnumerable<SentenceInformation> GetSentences()
        {
            //var text0 = "She looks very beautiful";
            var text0 = "Our class read Charlotte's Web";

            //var text = "My car is beautiful";
            var text = "Sandra likes chocolate cake";

            //var text1 = "Crina sings beautiful";
            var text1 = "Mom kissed baby Alice on the nose.";

            var text2 = "Ionel walks nice";
            var text3 = "My car runs beautiful";
            var text4 = "Superman is the best superhero";
            var text5 = "Superman is super strong";
            var text6 = "Batman is a superhero and a rich man";
            var text7 = "They are superheroes and rich";
            var text8 = "The new girl is nice and dirty";
            var text9 = "The new car is nice and dirty";
            var text10 = "Visual Studio is the best IDE";
            var text11 = "Linux is the best operating system";
            var text12 = "The continent that has the fewest flowering plants is called Antarctica";
            var text13 = "The character was created by writer Jerry Siegel and artist Joe Shuster, high school students living in Cleveland, Ohio, in 1933. ";
            var text14 = "Animals are multicellular, eukaryotic organisms of the kingdom Animalia (also called Metazoa)";
            //var text14 = "Uranus is the first planet ti be discovered using the telescope, in 1781.";
            var text15 = "All animals are motile, meaning they can move spontaneously and independently, at some point in their lives";
            var text16 = "Their body plan eventually becomes fixed as they develop, although some undergo a process of metamorphosis later on in their lives";
            var text17 = "All animals are heterotrophs: they must ingest other organisms or their products for sustenance.";
            var text18 = "Most known animal phyla appeared in the fossil record as marine species during the Cambrian explosion, about 542 million years ago";
            var tpr = new TextProcessing();
            tpr.ProcessText(text0);
            //TextProcessing.ProcessJson();
            var result = tpr.GetSentencesInformationFromJson();
            tpr.ProcessText(text);
            //TextProcessing.ProcessJson();
            var result0 = tpr.GetSentencesInformationFromJson();
            tpr.ProcessText(text1);
            //TextProcessing.ProcessJson();
            var result1 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text2);
            //TextProcessing.ProcessJson();
            var result2 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text3);
            //TextProcessing.ProcessJson();
            var result3 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text4);
            //TextProcessing.ProcessJson();
            var result4 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text5);
            //TextProcessing.ProcessJson();
            var result5 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text6);
            //TextProcessing.ProcessJson();
            var result6 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text7);
            //TextProcessing.ProcessJson();
            var result7 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text8);
            //TextProcessing.ProcessJson();
            var result8 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text9);
            //TextProcessing.ProcessJson();
            var result9 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text10);
            //TextProcessing.ProcessJson();
            var result10 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text11);
            //TextProcessing.ProcessJson();
            var result11 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text12);
            //TextProcessing.ProcessJson();
            var result12 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text13);
            //TextProcessing.ProcessJson();
            var result13 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text14);
            //TextProcessing.ProcessJson();
            var result14 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text15);
            //TextProcessing.ProcessJson();
            var result15 = tpr.GetSentencesInformationFromJson();


            tpr.ProcessText(text16);
            //TextProcessing.ProcessJson();
            var result16 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text17);
            //TextProcessing.ProcessJson();
            var result17 = tpr.GetSentencesInformationFromJson();

            tpr.ProcessText(text18);
            //TextProcessing.ProcessJson();
            var result18 = tpr.GetSentencesInformationFromJson();

            var resultList = new List<SentenceInformation>
            {
                //result[0],
                //result0[0],
                //result1[0],
                //result13[0],
                result14[0],
                result15[0],
                result16[0],
                result17[0],
                result18[0]
            };
            return resultList;
        }
    }
}
