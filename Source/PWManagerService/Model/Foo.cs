using System.Text.Json.Serialization;

namespace PWManagerService.Model
{
    [JsonDerivedType(typeof(BarOne))]
    [JsonDerivedType(typeof(BarTwo))]
    public abstract class Foo
    {
        public string FooField { get; set; } = "Foo";
    }
}
