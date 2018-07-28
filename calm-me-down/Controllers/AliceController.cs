using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using hello.lib;

namespace hello.Controllers
{
    public class AliceController : ApiController
    {
        public static Dictionary<string, AliceState> States = new Dictionary<string, AliceState>();
        public static Dictionary<string, int> Music = new Dictionary<string, int>();
        public static List<string> Tweets = TweetsReader.Get();
        private static Random random = new Random();

        private static int counterImages = 0;

        private static List<ResponseCard> images = new List<ResponseCard>
        {
            new ResponseCard("Нужно узбагоится.", "997614/c8d1a7b04748167085f4"),
            new ResponseCard("Просто узбагойся. И не реви.", "1030494/7083120723428737f910"),
            new ResponseCard("Охренеть как всё сложно.", "213044/a5d146542e1351a68264"),
            new ResponseCard("Узбагоин узбагаивает.", "937455/8d702c71919063b1d4a2"),
            new ResponseCard("Воу, воу. Позбагойнее.", "965417/4d49286fbb9436f0ff43"),
        };

        private static int counterAns = 0;

        private static List<string> answers = new List<string>
        {
            //"Надеюсь, я не смутила тебя?",
            "Тебе л+учше?",
            "Я помогла тебе?"
        };

        [Route("alice"), HttpPost]
        public AliceResponse Post([FromBody] AliceRequest req)
        {
            lock (States)
            {
                return Answer(req);
            }
        }

        private AliceResponse Answer(AliceRequest req)
        {
            var sid = req.Session.SessionId;
            if (req.Session.New || !States.ContainsKey(sid))
            {
                States[sid] = AliceState.Intro;
                counterAns = 0;
                counterImages = 0;
            }

            if (!Music.ContainsKey(sid))
                Music[sid] = 0;

            var state = States[sid];
            var result = req.Reply("я вас не поняла");
            var newState = AliceState.Start;

            switch (state)
            {
                case AliceState.Intro:
                    result = req.Reply(
                        "Привет, я лемур узбого+ин, расскажи мне что с тобой случилось, и я постараюсь поддержать тебя.");
                    newState = AliceState.Start;
                    break;

                case AliceState.Start:
                case AliceState.Quote:
                    result = req.Reply(GetBestQuote(req.Request.OriginalUtterance) + $" - - - - - - - - - \n{answers[counterAns++ % answers.Count]}");
                    newState = AliceState.Asking;
                    break;

                case AliceState.Asking:
                    if (req.Request.Command.Trim().ToLower() == "да")
                    {
                        result = req.Reply("Я рада, что смогла помочь.", true);
                        States.Remove(sid);
                        return result;
                    }
                    else
                    {
                        if (Music[sid]++ == 1)
                        {
                            result = req.Reply("Может послушаешь музыку?");
                            result.Response.Buttons = new[]
                            {
                                new ButtonModel
                                {
                                    Url = "https://music.yandex.ru/album/1317857/track/12139823",
                                    Title = "Like Lions - All Be Fine"
                                }
                            };

                            newState = AliceState.Asking;
                            break;
                        }

                        var card = images[counterImages++ % images.Count];
                        result = req.Reply(card.Title);
                        result.Response.Card = card;
                        //result.Response.Buttons = new[] {new ButtonModel() {Title = "👍" }, new ButtonModel() { Title = "👎" } };
                        newState = AliceState.Start;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            States[sid] = newState;
            return result;
        }

        private string GetBestQuote(string text)
        {
            var bestScore = 0;
            var best = Tweets[random.Next(Tweets.Count)];

            foreach (var quote in Tweets)
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
            var wa = SplitToWords(a);
            var wb = SplitToWords(b);

            var result = 0;

            foreach (var x in wa)
            {
                foreach (var y in wb)
                {
                    int len;
                    for (len = 0; len < x.Length && len < y.Length && x[len] == y[len]; len++) {}

                    result += len * len;
                }
            }

            return result;
        }

        private IEnumerable<string> SplitToWords(string s)
        {
            string cur = "";
            s += " ";

            s = s.ToLower();

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if ('а' <= c && c <= 'я')
                    cur += c;
                else
                {
                    if (!string.IsNullOrWhiteSpace(cur))
                        yield return cur;

                    cur = "";
                }
            }
        }
    }

    public enum AliceState
    {
        Intro,
        Start,
        Quote,
        Asking,
        Music,
        Picture,
        Finished
    }
    

    public static class TweetsReader
    {
        public static List<string> Get()
        {
            var text = File.ReadAllText(@"C:\alice\memdmitri_tweets.csv");
            var items = text.Split(new [] {"@@@@@"}, StringSplitOptions.RemoveEmptyEntries).ToList();
            items = items
                .Where(s => s.Contains("#мудрые_мысли"))
                .Select(Process)
                .ToList();
             return items;
        }

        private static string Process(string s)
        {
            s = s.Split(new[] { "#мудрые_мысли" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            s = s.Split('\n')[0].Trim();
            return s;
        }
    }
}
