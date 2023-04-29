namespace JsonSerialization.ViewModel
{
    public class Pet : IHasFieldStatus
    {
        public string Name { get; set; }
        public string Kind { get; set; }
        public int PersonId { get; set; }
        public Color Color { get; set; }
        public Dictionary<string, bool> FieldStatus { get ; set ; }
    }
}
