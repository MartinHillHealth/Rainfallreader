/*
 * Written by Martin Hill for Nov 2023 Coding challenge.
 *
 * CURRENT VERSION: 0.3
 */
namespace RainfallReader
{
    public class RainfallReader
    {
        private static DateTime CurrentTime;

        public static void Main()
        {
            Console.WriteLine("Welcome to Fuzion Inc. Flood Detection System v0.3");
            Console.WriteLine("Please assure datafiles is synced with the latest data before continuing.");

            Console.WriteLine("Press any continue to begin reading data.");

            // Using ReadKey to wait for user consent.
            Console.ReadKey();

            ReadAndDisplay();
        }

        public static void ReadAndDisplay()
        {
            // Parse datafiles into usable objects.
            List<Device> devices = Device.ReadDevices();

            foreach (Device device in devices)
            {
                Console.WriteLine(device.DeviceName);
            }

            ReadRainfall(devices);

            Report(devices);
        }

        private static void ReadRainfall(List<Device> devices)
        {
            CurrentTime = DateTime.MinValue;

            // Read all the rainfall per device
            devices.ForEach(device =>
            {
                DateTime lastDateTime = device.ReadRainfall();

                if (lastDateTime > CurrentTime)
                {
                    CurrentTime = lastDateTime;
                }
            });
        }

        private static void Report(List<Device> devices)
        {
            foreach (Device device in devices)
            {
                float average = device.GetAverage();
                string code = Device.GetCode(average);

                if (device.EmergencyCode(CurrentTime))
                {
                    code = "Red";
                }

                Console.WriteLine("Status report for " + device.DeviceName + ":");
                Console.WriteLine("Location: " + device.Location);

                Console.WriteLine();

                Console.WriteLine("Average rainfall over the last 4 hours: " + average );
                Console.WriteLine("This is a code " + code);
            }
        }
    }
}