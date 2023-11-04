using CsvHelper;
using System.Globalization;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace RainfallReader
{
    internal class Device
    {
        private string deviceID;

        private string deviceName;

        private string location;

        private List<RainFall> rainFallEvents;

        public string DeviceID
        {
            get;
            set;
        }

        public string DeviceName
        {
            get;
            set;
        }

        public string Location
        {
            get;
            set;
        }

        public static List<Device> ReadDevices()
        {
            using (StreamReader reader = new StreamReader(@"C:\Users\he134252\source\Repos\Rainfallreader\Rainfallreader\datafiles\Devices.csv"))
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<Device> devices = new List<Device>();

                csvReader.Read();
                csvReader.ReadHeader();

                while (csvReader.Read())
                {
                    devices.Add(new Device
                    {
                        DeviceID = csvReader.GetField<string>("Device ID"),
                        DeviceName = csvReader.GetField<string>("Device Name"),
                        Location = csvReader.GetField<string>("Location")
                    });
                }

                return devices;
            }
        }

        public void ReadRainfall()
        {
            string[] files = Directory.GetFiles(@"\datafiles");

            rainFallEvents = new List<RainFall>();

            foreach (string path in files)
            {
                if (!path.Contains("data") || !path.EndsWith("csv"))
                {
                    continue;
                }

                using StreamReader reader = new StreamReader(path);
                using CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                {
                    rainFallEvents.Add(new RainFall{});
                }
            }
        }
    }
}
