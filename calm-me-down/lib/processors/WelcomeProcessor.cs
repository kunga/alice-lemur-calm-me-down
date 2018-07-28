using hello.lib.alice;

namespace hello.lib.processors
{
    public class WelcomeProcessor : IProcessor
    {
        public AliceResponse Process(AliceRequest request, RaccoonState state)
        {
            return request.Reply("Привет, я лемур узбого+ин, расскажи мне что с тобой случилось, и я постараюсь поддержать тебя.");
        }
    }
}