/*
 * Written by Martin Hill for Nov 2023 Coding challenge.
 */


    using System.Collections.Generic;

    using RainfallReader;


namespace RainfallReader
{
    public class RainfallReader
    {
        public static void Main()
        {
            Console.WriteLine("Welcome to Fuzion Inc. Flood Detection System v0.1");
            Console.WriteLine("Please assure datafiles is synced with the latest data before continuing.");

            Console.WriteLine("Press any continue to begin reading data.");

            // Using ReadKey to wait for user consent.
            Console.ReadKey();
        }

        public void ReadAndDisplay()
        {
            // Parse datafiles into usable objects.
            List<Device> devices = ReadDevices();

            ReadRainfall(devices);
        }

        private List<Device> ReadDevices()
        {
            return null;
        }

        private void ReadRainfall(List<Device> devices)
        {

        }
    }
}