using System.Web.Http;
using hello.lib;

namespace hello.Controllers
{
    public class AliceController : ApiController
    {
        [Route("alice"), HttpPost]
        public AliceResponse Post([FromBody] AliceRequest req)
        {
            return req.Reply("Привет от Контура");
        }
    }
}
