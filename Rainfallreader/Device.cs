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

        public float GetAverage()
        {
            int total = 0;

            rainFallEvents.ForEach(rainfall => total += rainfall.Rainfall);

            return total / rainFallEvents.Count;
        }

        public DateTime ReadRainfall()
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

                        if (lastDateTime < csvReader.GetField<DateTime>("Time"))
                        {
                            lastDateTime = csvReader.GetField<DateTime>("Time");
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

            return lastDateTime;
        }

        public bool EmergencyCode(DateTime currentDateTime)
        {
            foreach (RainFall rain in rainFallEvents)
            {
                if (rain.Time > currentDateTime.AddHours(-4) && rain.Rainfall > 30)
                    return true;
            }

            return false;
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

        public static string GetCode(float average)
        {
            if (average < 10)
            {
                return "Green";
            }

            if (average < 15)
            {
                return "Yellow";
            }

            return "Red";
        }
    }
}
