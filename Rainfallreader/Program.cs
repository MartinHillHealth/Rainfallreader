/*
 * Written by Martin Hill for Nov 2023 Coding challenge.
 *
 * CURRENT VERSION: 0.2
 */
namespace RainfallReader
{
    using System.Globalization;

    using CsvHelper;


    public class RainfallReader
    {
        public static void Main()
        {
            Console.WriteLine("Welcome to Fuzion Inc. Flood Detection System v0.2");
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
        }

        private static void ReadRainfall(List<Device> devices)
        {
            // Read all the rainfall per device
            devices.ForEach(device => device.ReadRainfall());
        }
    }
}