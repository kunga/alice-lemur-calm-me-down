using hello.lib.alice;

namespace hello.lib.processors
{
    public interface IProcessor
    {
        AliceResponse Process(AliceRequest request, RaccoonState state);
    }
}