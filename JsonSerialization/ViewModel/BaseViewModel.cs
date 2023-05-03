
using System.Text.Json.Serialization;

namespace JsonSerialization
{
    //[JsonConverter(typeof(BasicMyConverter))]
    public class BaseViewModel
    {
        public string Test { set; get; }
    }
}
