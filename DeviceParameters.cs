using System.Text.Json.Serialization;

namespace ParameterListComparer
{
    internal class DeviceParameters
    {
        [JsonPropertyName("address")]
        public required string Address { get; set; }

        [JsonPropertyName("reference")]
        public string? Reference { get; set; }

        [JsonPropertyName("application")]
        public string? Application { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("writeable")]
        public string? Writeable { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }

        [JsonPropertyName("scale")]
        public string? Scale { get; set; }

        public CompareResults Compare(DeviceParameters other)
        {
            var results = new CompareResults();
            var locationSpecifierPrefix = $"At address {Address}: ";

            foreach (var property in typeof(DeviceParameters).GetProperties())
            {
                var thisValue = property.GetValue(this);
                var otherValue = property.GetValue(other);
                
                if (Equals(thisValue, otherValue))
                {
                    results.Score++;
                }
                else
                {
                    string differenceMessage;
                    if (thisValue == null)
                    {
                        differenceMessage = $"{locationSpecifierPrefix} {property.Name} missing in input";
                    }
                    else if (otherValue == null)
                    {
                        differenceMessage = $"{locationSpecifierPrefix} {property.Name} missing in library";
                    }
                    else
                    {
                        differenceMessage = $"{locationSpecifierPrefix} {property.Name}: {thisValue} != {otherValue}";
                    }

                    results.Differences.Add(differenceMessage);
                }
            }

            return results;
        }
    }
}
