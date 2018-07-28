namespace hello.lib
{
    public static class Extensions
    {
        public static AliceResponse Reply(
          this AliceRequest req,
          string text,
          bool endSession = false,
          ButtonModel[] buttons = null) => new AliceResponse
          {
              Response = new ResponseModel
              {
                  Text = text.Replace("+", "").Replace("- ", ""),
                  Tts = text,
                  EndSession = endSession
              },
              Session = req?.Session
          };
    }
}