using System.Collections.Generic;
using hello.lib.alice;

namespace hello.lib.processors
{
    public class RacconProcessor : LoopedProcessor<IProcessor>
    {
        private readonly RaccoonState state = new RaccoonState();

        public RacconProcessor()
            : base(new List<IProcessor> { new QuotesProcessor(), new ImagesProcessor(), new SongsProcessor()})
        {
        }

        public AliceResponse Process(AliceRequest request)
        {
            var processor = state.IsFirstCommand ? new WelcomeProcessor() : GetNext();

            if (state.AskedFinishQuestion)
            {
                if (request.Request.Command.Trim().ToLower() == "да")
                {
                    return new GoodByeProcessor().Process(request, state);
                }
            }

            state.AskedFinishQuestion = false;
            state.IsFirstCommand = false;
            return processor.Process(request, state);
        }
    }
}