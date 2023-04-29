namespace JsonSerialization.ViewModel
{
    public class Person : BaseViewModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int? Age { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsDead { get; set; }
        public double Number { get; set; }
        public WeekType DayOfWeek { get; set; }
        public IEnumerable<Pet> Dogs { get; set; }
        public Pet Pet { get; set; }
        public IEnumerable<string> Q { get; set; }
    }
}
