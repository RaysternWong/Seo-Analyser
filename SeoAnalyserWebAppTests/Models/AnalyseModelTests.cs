using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeoAnalyserWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeoAnalyserWebApp.Models.Tests
{
    [TestClass()]
    public class AnalyseModelTests
    {
        [TestMethod()]
        public void FilterStopWordsTest_basic()
        {
            string input = "I am a person and a man";
            string expectedResult = "person man";

            AnalyseModel model = new AnalyseModel();
            Assert.AreEqual(expectedResult, model.RemoveStopWords(input));
        }

        [TestMethod()]
        public void FilterStopWordsTest_input_is_null()
        {
            string input = null;
            string expectedResult = string.Empty;

            AnalyseModel model = new AnalyseModel();
            Assert.AreEqual(expectedResult, model.RemoveStopWords(input));
        }

        [TestMethod()]
        public void FilterStopWordsTest_input_is_empty()
        {
            string input = string.Empty;
            string expectedResult = string.Empty;

            AnalyseModel model = new AnalyseModel();
            Assert.AreEqual(expectedResult, model.RemoveStopWords(input));
        }

        [TestMethod()]
        public void FilterStopWordsTest_input_have_stopWords_of_UpperCharacter()
        {
            string input = "I AM A person aNd A man";
            string expectedResult = "person man";

            AnalyseModel model = new AnalyseModel();
            Assert.AreEqual(expectedResult, model.RemoveStopWords(input));
        }

        [TestMethod()]
        public void FilterStopWordsTest_input_have_empty_space()
        {
            string input = "I    AM     A person    aNd    A    man";
            string expectedResult = "person man";

            AnalyseModel model = new AnalyseModel();
            Assert.AreEqual(expectedResult, model.RemoveStopWords(input));
        }

        [TestMethod()]
        public void GetOccuranceWordTableTest()
        {
            string input = "apple apple apple";

            AnalyseModel model = new AnalyseModel();
            var result = model.GetOccuranceWordTable(input);

            Assert.IsTrue(result.Any(x => x.Key == "apple" && x.Value == 3));
            Assert.IsFalse(result.Any(x => x.Key == "apple" && x.Value == 1));
        }

        [TestMethod()]
        public void GetOccuranceWordTableTest_upperCharacter()
        {
            string input = "Apple apple APPLE";

            AnalyseModel model = new AnalyseModel();
            var result = model.GetOccuranceWordTable(input);

            Assert.IsTrue(result.Any(x => x.Key == "apple" && x.Value == 3));
        }

        [TestMethod()]
        public void GetOccuranceWordTableTest_two_differenct_words()
        {
            string input = "Apple banana apple Banana APPLE";

            AnalyseModel model = new AnalyseModel();
            var result = model.GetOccuranceWordTable(input);

            Assert.IsTrue(result.Any(x => x.Key == "apple" && x.Value == 3));
            Assert.IsTrue(result.Any(x => x.Key == "banana" && x.Value == 2));
        }

        [TestMethod()]
        public void GetOccuranceWordTableTest_input_is_empty()
        {
            string input = "    ";

            AnalyseModel model = new AnalyseModel();
            var result = model.GetOccuranceWordTable(input);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod()]
        public void GetOccuranceWordInMetaTagTableTest()
        {
            string input = @"<meta charset= ""UTF - 8\"">
                             <meta name = ""keywords"" content = ""HTML,CSS,XML,JavaScript"" >
                             <meta name = ""keywords"" content = ""HTML,CSS,XML,JavaScript"" > 
                             <meta name = ""keywords"" content = ""HTML,CSS,XML,JavaScript"" >";

            AnalyseModel model = new AnalyseModel();
            var result = model.GetOccuranceWordInMetaTagTable(input);

            Assert.IsTrue(result.Any(x => x.Key == "content" && x.Value == 3));
            Assert.IsTrue(result.Any(x => x.Key == "name" && x.Value == 3));
            Assert.IsTrue(result.Any(x => x.Key == "\"keywords\"" && x.Value == 3));
        }

        [TestMethod()]
        public void GetOccuranceExternalLinkTableTest()
        {
            string input = string.Empty;
            string link1 = @"https://visualstudio.microsoft.com/vso/";

            input += link1;


            AnalyseModel model = new AnalyseModel();
            var result = model.GetOccuranceExternalLinkTable(input);

            Assert.IsTrue(result.Any(x => x.Key == link1 && x.Value == 1));
        }

        [TestMethod()]
        public void GetOccuranceExternalLinkTableTest_two_same_link()
        {
            string input = string.Empty;
            string link1 = @"https://visualstudio.microsoft.com/vso/";

            input += (link1 + " " + link1);

            AnalyseModel model = new AnalyseModel();
            var result = model.GetOccuranceExternalLinkTable(input);

            Assert.IsTrue(result.Any(x => x.Key == link1 && x.Value == 2));
        }

        [TestMethod()]
        public void GetOccuranceExternalLinkTableTest_4_link()
        {
            string input = string.Empty;
            string link1 = @"https://visualstudio.microsoft.com/vso/";
            string link2 = @"https://google.com/";

            input += (link1 + " " + link1 + " " + link2 + " " + link2 );

            AnalyseModel model = new AnalyseModel();
            var result = model.GetOccuranceExternalLinkTable(input);

            Assert.IsTrue(result.Any(x => x.Key == link1 && x.Value == 2));
            Assert.IsTrue(result.Any(x => x.Key == link2 && x.Value == 2));
        }
    }
}