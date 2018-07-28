using System.Collections.Generic;
using System.Web.Http;
using hello.lib.alice;
using hello.lib.processors;

namespace hello.Controllers
{
    public class RaccoonController : ApiController
    {
        private static readonly Dictionary<string, RacconProcessor> Raccoons = new Dictionary<string, RacconProcessor>();

        [Route("raccoon"), HttpPost]
        public AliceResponse Post([FromBody] AliceRequest req)
        {
            RacconProcessor raccoon;
            var sid = req.Session.SessionId;

            lock (Raccoons)
            {
                if (!Raccoons.ContainsKey(sid))
                    Raccoons[sid] = new RacconProcessor();

                raccoon = Raccoons[sid];
            }

            var response = raccoon.Process(req);

            if (response.Response.EndSession)
            {
                lock (Raccoons)
                {
                    Raccoons.Remove(sid);
                }
            }

            return response;
        }
    }
}
