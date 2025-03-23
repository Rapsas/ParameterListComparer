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
    }
}
