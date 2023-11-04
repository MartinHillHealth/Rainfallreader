using CsvHelper;
using System.Globalization;

namespace RainfallReader
{
    using Rainfallreader;

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

        public void PrintRainfall()
        {
            Console.WriteLine("Device: " + DeviceName);
            Console.WriteLine("Location: " + location);
            rainFallEvents.ForEach(rainfall => Console.WriteLine(rainfall.Rainfall));
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
            string[] files = Directory.GetFiles(@"C:\Users\he134252\source\Repos\Rainfallreader\Rainfallreader\datafiles");

            rainFallEvents = new List<RainFall>();

            DateTime lastDateTime = DateTime.MinValue;

            foreach (string path in files)
            {
                if (!path.ToLower().Contains("data") || !path.ToLower().EndsWith("csv") || path.ToLower().Contains("devices"))
                {
                    continue;
                }

                using StreamReader reader = new StreamReader(path);
                using CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                {
                    csvReader.Read();
                    csvReader.ReadHeader();

                    while (csvReader.Read())
                    {
                        // If for another reader.
                        if (csvReader.GetField<string>("Device ID") != DeviceID)
                        {
                            continue;
                        }

                        rainFallEvents.Add(new RainFall
                        {
                            DeviceId = DeviceID,
                            Rainfall = csvReader.GetField<int>("Rainfall"),
                            Time = csvReader.GetField<DateTime>("Time")
                        });
                    }
                }
            }
        }
    }
}
