using Newtonsoft.Json;

namespace JsonSerialization.ViewModel
{
    public class Color : IHasFieldStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public  Dictionary<string, bool> FieldStatus { get; set; }

        public void check()
        {
            if (FieldStatus.ContainsKey("Name"))
            {
                Name = "";
            }
        }
    }
}
