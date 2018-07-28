using Newtonsoft.Json;

namespace hello.lib.alice
{
    public class ResponseModel
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("tts")]
        public string Tts { get; set; }

        [JsonProperty("end_session")]
        public bool EndSession { get; set; }

        [JsonProperty("buttons")]
        public ButtonModel[] Buttons { get; set; }

        [JsonProperty("card")]
        public ResponseCard Card { get; set; }
    }

    public class ResponseCard
    {
        [JsonProperty("type")]
        public string Type = "BigImage";

        [JsonProperty("title")]
        public readonly string Title;

        [JsonProperty("image_id")]
        public string ImageId;

        [JsonProperty("description")]
        public string Descriptin = "";

        public ResponseCard(string title, string imageId)
        {
            Title = title;
            ImageId = imageId;
        }
    }
}