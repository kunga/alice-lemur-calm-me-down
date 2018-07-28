using System;
using System.Collections.Generic;
using System.Web.Http;
using hello.lib.alice;
using hello.lib.processors;

namespace hello.Controllers
{
    public class RaccoonController : ApiController
    {
        private static readonly Dictionary<string, RacconProcessor> RaccoonStates = new Dictionary<string, RacconProcessor>();

        [Route("raccoon"), HttpPost]
        public AliceResponse RacconCalmMeDown([FromBody] AliceRequest req)
        {
            try
            {
                return RacconTryCalmMeDown(req);
            }
            catch (Exception e)
            {
                return new AliceResponse {Response = new ResponseModel {Text = e.ToString()}};
            }
        }

        private static AliceResponse RacconTryCalmMeDown(AliceRequest req)
        {
            RacconProcessor raccoon;
            var sid = req.Session.SessionId;

            lock (RaccoonStates)
            {
                if (!RaccoonStates.ContainsKey(sid))
                    RaccoonStates[sid] = new RacconProcessor();

                raccoon = RaccoonStates[sid];
            }

            var response = raccoon.Process(req);

            if (response.Response.EndSession)
            {
                lock (RaccoonStates)
                {
                    RaccoonStates.Remove(sid);
                }
            }

            return response;
        }

        [Route("state"), HttpGet]
        public Dictionary<string, RacconProcessor> GetState()
        {
            lock (RaccoonStates)
            {
                return RaccoonStates;
            }
        }
    }
}
