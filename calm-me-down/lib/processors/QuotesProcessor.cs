using System;
using System.Collections.Generic;
using System.Linq;
using hello.lib.alice;
using hello.lib.data;

namespace hello.lib.processors
{
    public class QuotesProcessor : IProcessor
    {
        private static readonly List<string> Quotes = QuotesReader.Get();

        private readonly Random random = new Random();
        private readonly FinishingQuestionsProcessor finishingQuestionsProcessor = new FinishingQuestionsProcessor();

        public AliceResponse Process(AliceRequest request, LemurState state)
        {
            var response = request.Reply($"{GetBestQuote(request.Request.OriginalUtterance)} - - - - - - - - - \n{finishingQuestionsProcessor.GetNext()}");
            state.AskedFinishQuestion = true;
            return response;
        }

        private string GetBestQuote(string text)
        {
            var bestScore = 0;
            var best = Quotes[random.Next(Quotes.Count)];

            foreach (var quote in Quotes)
            {
                var cur = GetScore(quote, text);
                if (cur > bestScore)
                {
                    bestScore = cur;
                    best = quote;
                }
            }

            return best;
        }

        private int GetScore(string a, string b)
        {
            var wa = SplitToWords(a).ToList();
            var wb = SplitToWords(b).ToList();

            var result = 0;

            foreach (var x in wa)
            {
                foreach (var y in wb)
                {
                    int len;
                    for (len = 0; len < x.Length && len < y.Length && x[len] == y[len]; len++) { }

                    var remaining = Math.Max(x.Length, y.Length) - len;
                    if (len >= 3 && remaining <= 3)
                        result += len * len - remaining * remaining;
                }
            }

            return result;
        }

        private IEnumerable<string> SplitToWords(string s)
        {
            var token = "";
            s += " ";

            s = s.ToLower();

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if ('а' <= c && c <= 'я')
                    token += c;
                else
                {
                    if (!string.IsNullOrWhiteSpace(token))
                        yield return token;

                    token = "";
                }
            }
        }
    }
}