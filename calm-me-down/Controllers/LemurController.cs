using System;
using System.Collections.Generic;
using System.Web.Http;
using hello.lib.alice;
using hello.lib.processors;

namespace hello.Controllers
{
    public class LemurController : ApiController
    {
        private static readonly Dictionary<string, LemurProcessor> LemurStates = new Dictionary<string, LemurProcessor>();

        [Route("lemur"), HttpPost]
        public AliceResponse LemurCalmMeDown([FromBody] AliceRequest req)
        {
            try
            {
                return LemurTryCalmMeDown(req);
            }
            catch (Exception e)
            {
                return new AliceResponse {Response = new ResponseModel {Text = e.ToString()}};
            }
        }

        private static AliceResponse LemurTryCalmMeDown(AliceRequest req)
        {
            LemurProcessor lemur;
            var sid = req.Session.SessionId;

            lock (LemurStates)
            {
                if (!LemurStates.ContainsKey(sid))
                    LemurStates[sid] = new LemurProcessor();

                lemur = LemurStates[sid];
            }

            var response = lemur.Process(req);

            if (response.Response.EndSession)
            {
                lock (LemurStates)
                {
                    LemurStates.Remove(sid);
                }
            }

            return response;
        }

        [Route("state"), HttpGet]
        public Dictionary<string, LemurProcessor> GetState()
        {
            lock (LemurStates)
            {
                return LemurStates;
            }
        }
    }
}
