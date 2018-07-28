using System.Collections.Generic;
using hello.lib.alice;

namespace hello.lib.processors
{
    public class SongsProcessor : LoopedProcessor<ButtonModel>, IProcessor
    {
        private static readonly List<ButtonModel> Songs = new List<ButtonModel>
        {
            new ButtonModel
            {
                Url = "https://music.yandex.ru/album/1317857/track/12139823",
                Title = "Like Lions - All Be Fine"
            },
            new ButtonModel
            {
                Url = "https://music.yandex.ru/album/4424106/track/35357241",
                Title = "Flogging Molly - Life Is Good"
            }
        };
        
        public SongsProcessor() : base(Songs)
        {
        }

        public AliceResponse Process(AliceRequest request, LemurState state)
        {
            var response = request.Reply("Может послушаешь музыку?");
            response.Response.Buttons = new[]
            {
                GetNext()
            };
            state.AskedFinishQuestion = true;
            return response;
        }
    }
}