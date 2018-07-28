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
        public static List<string> Tweets = TweetsReader.Get();
        private static Random random = new Random();

        [Route("alice"), HttpPost]
        public AliceResponse Post([FromBody] AliceRequest req)
        {
            if (req.Session.New || !States.ContainsKey(req.Session.SessionId))
                States[req.Session.SessionId] = AliceState.Intro;

            var state = States[req.Session.SessionId];
            var result = req.Reply("я вас не поняла");
            var newState = AliceState.Start;

            switch (state)
            {
                case AliceState.Intro:
                    result = req.Reply("Привет, я лемур успокоин, расскажи мне что с тобой случилось, и я постараюсь поддержать тебя.");
                    newState = AliceState.Start;
                    break;

                case AliceState.Start:
                    result = req.Reply(Tweets[random.Next(Tweets.Count)] + " Тебе лучше?");
                    newState = AliceState.Asking;
                    break;

                case AliceState.Quote:
                    result = req.Reply(Tweets[random.Next(Tweets.Count)] + " Тебе лучше?");
                    newState = AliceState.Asking;
                    break;

                case AliceState.Asking:
                    if (req.Request.Command.Contains("да"))
                    {
                        result = req.Reply("Я рада, что смогла помочь.", true);
                        States.Remove(req.Session.SessionId);
                        return result;
                    }
                    else
                    {
                        result = req.Reply("Тогда держи мемасик");
                        newState = AliceState.Start;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            States[req.Session.SessionId] = newState;
            return result;
        }
    }

    public enum AliceState
    {
        Intro,
        Start,
        Quote,
        Asking,
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
                .Select(s => s.Split(new string[] { "#мудрые_мысли" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim())
                .ToList();
            return items;
        }
    }
}
