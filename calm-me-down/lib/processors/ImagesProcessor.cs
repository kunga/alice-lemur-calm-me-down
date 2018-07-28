using System.Collections.Generic;
using hello.lib.alice;

namespace hello.lib.processors
{
    public class ImagesProcessor : LoopedProcessor<ResponseCard>, IProcessor
    {
        private static readonly List<ResponseCard> Images = new List<ResponseCard>
        {
            new ResponseCard("Нужно узбагоится.", "997614/c8d1a7b04748167085f4"),
            new ResponseCard("Просто узбагойся. И не реви.", "1030494/7083120723428737f910"),
            new ResponseCard("Охренеть как всё сложно.", "213044/a5d146542e1351a68264"),
            new ResponseCard("Узбагоин узбагаивает.", "937455/8d702c71919063b1d4a2"),
            new ResponseCard("Воу, воу. Позбагойнее.", "965417/4d49286fbb9436f0ff43")
        };

        public ImagesProcessor() : base(Images)
        {
        }
        
        public AliceResponse Process(AliceRequest request, RaccoonState state)
        {
            var image =  GetNext();
            var result = request.Reply(image);
            return result;
        }
    }
}