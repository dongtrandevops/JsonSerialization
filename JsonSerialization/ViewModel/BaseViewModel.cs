using Newtonsoft.Json;

namespace JsonSerialization
{
    public class BaseViewModel
    {
        [JsonIgnore]
        public string Test { private set; get; }
    }
}
