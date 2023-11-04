using CsvHelper;
using System.Globalization;

namespace RainfallReader
{
    // Tracks a given device at a given location and rainfall associated with it.
    internal class Device
    {
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

        // Get the average rainfall over the last 4 hours.
        public float GetAverage(DateTime currentTime)
        {
            int total = 0;

            rainFallEvents.ForEach(rainfall =>
            {
                if(rainfall.Time >= currentTime.AddHours(-4))
                    total += rainfall.Rainfall;
            });

            return total / rainFallEvents.Count;
        }

        // Read rainfall data from the CSV files.
        public DateTime ReadRainfall(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath);

            // Empty any existing data and initialise.
            rainFallEvents = new List<RainFall>();

            DateTime lastDateTime = DateTime.MinValue;

            // parse all files in the given folder;
            foreach (string path in files)
            {
                // Skip non-csv files.
                if (!path.ToLower().EndsWith("csv"))
                {
                    continue;
                }

                using StreamReader reader = new StreamReader(path);
                using CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                {
                    csvReader.Read();
                    csvReader.ReadHeader();

                    bool skipFile = false;

                    // Check the headers are correct
                    foreach (string header in csvReader.HeaderRecord)
                    {
                        if (header != "Device ID" && header != "Rainfall" && header != "Time")
                        {
                            skipFile = true;
                        }
                    }

                    if (skipFile)
                    {
                        continue;
                    }

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
                            Rainfall = csvReader.GetField<int>("Rainfall"),
                            Time = csvReader.GetField<DateTime>("Time")
                        });
                    }
                }
            }

            return lastDateTime;
        }

        // Checks if there has been a rainfall even over 30 mm in the last 4 hours.
        public bool EmergencyCode(DateTime currentDateTime)
        {
            foreach (RainFall rain in rainFallEvents)
            {
                if (rain.Time > currentDateTime.AddHours(-4) && rain.Rainfall > 30)
                    return true;
            }

            return false;
        }

        // Read device data from CSV file.
        public static List<Device> ReadDevices()
        {
            // Would normally use WinForms for this, but apparently that's not a standard package anymore.
            Console.WriteLine("What is the path to the location of the device csv data file?");
            string filePath = Console.ReadLine();

            if (!filePath.EndsWith("csv"))
            {
                Console.WriteLine("CSV file not supplied");
                    
                return new List<Device>();
            }

            using (StreamReader reader = new StreamReader(filePath))
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

        // Gets the appropriate code for the given average.
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
