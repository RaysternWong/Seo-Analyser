using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SeoAnalyserWebApp.Models
{
    public class AnalyseModel
    {
        #region Properties
        public string Input { get; set; } = string.Empty;
        public string InvalidMessage { get; set; } = null;
        public bool IsValid { get; set; } = false;
        public bool FilterStopsWordsFlag { get; set; } = true;
        public bool CalNumOfOccuranceOfWordsFlag { get; set; } = true;
        public bool CalNumOfOccuranceOfWordsListedInMetaTagsFlag { get; set; } = true;
        public bool CalNumOfOccuranceOfExternalLinksFlag { get; set; } = true;
        public Dictionary<string, int> OccuranceOfWordsTable { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> OccuranceOfWordsInMetaTagsTable { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> OccuranceOfExternalLinksTable { get; set; } = new Dictionary<string, int>();
       
        public bool IsAnyOptionSelected
        {
            get
            {
               return (FilterStopsWordsFlag | CalNumOfOccuranceOfWordsFlag | CalNumOfOccuranceOfWordsListedInMetaTagsFlag | CalNumOfOccuranceOfExternalLinksFlag) ;
            }
        }
        
        #endregion

        public List<string> stopWordsList = new List<string>()
                {
                    "a","about", "above", "after", "again", "against",
                    "all", "am", "an", "and", "any", "are", "as", "at",
                    "be", "because", "been", "before", "being", "below", "between", "both", "but", "by",
                    "could",
                    "did", "do", "does", "doing", "down", "during",
                    "each",
                    "few", "for", "from", "further",
                    "had", "has", "have", "having", "he", "he'd", "he'll", "he's", "her", "here", "here's", "hers", "herself", "him", "himself", "his", "how", "how's",
                    "i", "i'd", "i'll", "i'm", "i've", "if", "in", "into", "is", "it", "it's", "its", "itself", "let's",
                    "me", "more", "most", "my", "myself",
                    "nor",
                    "of", "on", "once", "only", "or", "other", "ought", "our", "ours", "ourselves", "out", "over", "own",
                    "same", "she", "she'd", "she'll", "she's", "should", "so", "some", "such",
                    "than", "that", "that's", "the", "their", "theirs", "them", "themselves", "then", "there", "there's", "these", "they", "they'd", "they'll", "they're", "they've", "this", "those", "through", "to", "too",
                    "under", "until", "up",
                    "very",
                    "was", "we", "we'd", "we'll", "we're", "we've", "were", "what", "what's", "when", "when's", "where", "where's", "which", "while", "who", "who's", "whom", "why", "why's", "with", "would",
                    "you", "you'd", "you'll", "you're", "you've", "your", "yours", "yourself", "yourselves"
                };

        public string RemoveStopWords(string text)
        {
            string result = string.Empty;

            if(!string.IsNullOrEmpty(text))
            {
                var wordsList = text.Split(' ').ToList();
                wordsList.RemoveAll(x => string.IsNullOrEmpty(x) || stopWordsList.Contains(x.ToLower()));
                result = String.Join(" ", wordsList.ToArray());
            }

            return result;
        }

        public Dictionary<string,int> GetOccuranceWordTable(string text)
        {
            Dictionary<string,int> result = new Dictionary<string, int>();

            if (!string.IsNullOrEmpty(text))
            {
                text = text.ToLower();
                result = text.Split(' ').Where(s => ! string.IsNullOrEmpty(s)).GroupBy(s => s).ToDictionary(g => g.Key, g => g.Count());
            }

            return result;
        }

        public Dictionary<string, int> GetOccuranceWordInMetaTagTable(string text)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();

            if (!string.IsNullOrEmpty(text))
            {
                var matchedData = string.Empty;
                var regMetaTag = new Regex(@"<meta.*?>");
                
                foreach (Match match in regMetaTag.Matches(text))
                {
                    matchedData += " " + match.Value;
                }

                result = GetOccuranceWordTable(matchedData);
            }

            return result;
        }

        public Dictionary<string, int> GetOccuranceExternalLinkTable(string text)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();

            if (!string.IsNullOrEmpty(text))
            {
                var matchedData = string.Empty;
                var regHttpLink = new Regex(@"(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?");

                foreach (Match match in regHttpLink.Matches(text))
                {
                    matchedData += " " + match.Value;
                }

                result = GetOccuranceWordTable(matchedData);
            }

            return result;
        }
    }
}