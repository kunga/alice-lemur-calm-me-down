using System.Collections.Generic;
using hello.lib.alice;

namespace hello.lib.processors
{
    public class LemurProcessor : LoopedProcessor<IProcessor>
    {
        private readonly LemurState state = new LemurState();

        public LemurProcessor()
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