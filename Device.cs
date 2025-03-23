namespace ParameterListComparer
{
    internal class Device
    {
        public string Name { get; private set; }
        public Dictionary<string, DeviceParameters> Parameters { get; set; } = new();
        public int DeviceParameterPropertyCount { get; private set; }

        public Device(string name, List<DeviceParameters> parameters)
        {
            Name = name;
            parameters.ForEach(item => Parameters.Add(item.Address, item));
            DeviceParameterPropertyCount = typeof(DeviceParameters).GetProperties().Count();
        }

        public (string Name, CompareResults CompareResults) Compare(Device other)
        {
            var deviceResults = new CompareResults();
            foreach (var inputParameter in Parameters)
            {
                if (other.Parameters.TryGetValue(inputParameter.Key, out var libraryParameter))
                {
                    var parameterResults = inputParameter.Value.Compare(libraryParameter);
                    deviceResults.Score += parameterResults.Score;
                    deviceResults.Differences.AddRange(parameterResults.Differences);
                }
                else
                {
                    deviceResults.Differences.Add($"Address \"{inputParameter.Key}\" missing in library");
                }
            }
            return (other.Name, deviceResults);
        }
    }
}
