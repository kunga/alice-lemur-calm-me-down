using hello.lib.alice;

namespace hello.lib.processors
{
    public class GoodByeProcessor : IProcessor
    {
        public AliceResponse Process(AliceRequest request, LemurState state)
        {
            return request.Reply("Я рада, что смогла помочь.", true);
        }
    }
}