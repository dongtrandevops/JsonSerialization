using System.ComponentModel;
using System.Text.Json.Serialization;

namespace JsonSerialization.ViewModel
{
    public class DummyData : IHasFieldStatus
    {
        public byte Byte { get; set; }
        public sbyte SByte { get; set; }
        public short Short { get; set; }
        public ushort UShort { get; set; }
        public int Int { get; set; }
        public uint UInt { get; set; }
        public long Long { get; set; }
        public ulong ULong { get; set; }
        public float Float { get; set; }
        public double Double { get; set; }
        public decimal Decimal { get; set; }
        public char Char { get; set; }
        public bool Bool { get; set; }
        public DateTime DateTime { get; set; }
        public DateOnly DateOnly { get; set; }
        public TimeOnly TimeOnly { get; set; }
        public string String { get; set; }

        public Pet Pet { get; set; }

        public List<Pet> Pets { get; set; }

        public WeekType? Week { get; set; }

        public List<byte> ListByte { set; get; }
        public List<sbyte> ListSByte { set; get; }
        public List<short> ListShort { set; get; }
        public List<ushort> ListUShort { set; get; }
        public List<int> ListInt { set; get; }
        public List<uint> ListUint { set; get; }
        public List<long> ListLong { set; get; }
        public List<ulong> ListUlong { set; get; }
        public List<float> ListFloat { set; get; }
        public List<double> ListDouble { set; get; }
        public List<decimal> ListDecimal { set; get; }
        public List<char> ListChar { set; get; }
        public List<bool> ListBool { set; get; }
        public List<DateTime> ListDateTime { set; get; }
        public List<DateOnly> ListDateOnly { set; get; }
        public List<TimeOnly> ListTimeOnly { set; get; }
        public List<string> ListString { set; get; }
        public List<WeekType> ListWeekType { set; get; }

        public Dictionary<string, bool> FieldStatus { get; set; }
    }
}

