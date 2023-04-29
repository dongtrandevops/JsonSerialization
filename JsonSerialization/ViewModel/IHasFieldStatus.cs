using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace JsonSerialization.ViewModel
{
    public interface IHasFieldStatus
    {
        public Dictionary<string, bool> FieldStatus { get; set; }
    }
}
