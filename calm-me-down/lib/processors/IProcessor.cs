using hello.lib.alice;

namespace hello.lib.processors
{
    public interface IProcessor
    {
        AliceResponse Process(AliceRequest request, LemurState state);
    }
}