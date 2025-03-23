using System.Text.Json;

namespace ParameterListComparer
{
    internal static class JsonReader
    {
        private static Device Read(string fileName)
        {
            using (StreamReader sr = new(fileName))
            {
                string json = sr.ReadToEnd();
                var deviceParameterList = JsonSerializer.Deserialize<List<DeviceParameters>>(json);

                if (deviceParameterList == null)
                {
                    throw new Exception();
                }

                return new Device(Path.GetFileName(fileName), deviceParameterList);
            }
        }

        public static List<Device> ReadAllFiles(string inputFolderPath) 
        {
            var inputJsonFiles = Directory.GetFiles(inputFolderPath);
            var devices = new List<Device>();

            foreach (var item in inputJsonFiles)
            {
                devices.Add(Read(item));
            }

            return devices;
        }
    }
}
