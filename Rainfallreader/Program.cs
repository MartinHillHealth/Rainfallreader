﻿/*
 * Written by Martin Hill for Nov 2023 Coding challenge.
 *
 * CURRENT VERSION: 0.4
 */
namespace RainfallReader
{
    public class RainfallReader
    {
        private static DateTime CurrentTime;

        private static List<Device> Devices;

        public static void Main()
        {
            Console.WriteLine("Welcome to Fuzion Inc. Flood Detection System v0.4");
            Console.WriteLine("Please assure datafiles is synced with the latest data before continuing.");
            string? input;

            do
            {
                Console.WriteLine("Do you wish to load data (\"load\"), or report on loaded data (\"report\")? Enter \"quit\" to exit.");

                // Using ReadKey to wait for user consent.
                input = Console.ReadLine();

                if (input == "load")
                {
                    // Parse datafiles into usable objects.
                    Devices = Device.ReadDevices();

                    ReadRainfall(Devices);
                }

                if (input == "report")
                {

                    if (Devices.Count == 0)
                    {
                        Console.WriteLine("No Device values have been loaded. Please load devices first.");

                        continue;
                    }

                    Report(Devices);
                }
            }
            while (input.ToLower() != "quit");

            Console.WriteLine("Goodbye!");
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

                switch (code)
                {
                    case "Green":

                        Console.ForegroundColor = ConsoleColor.Green;

                        break;

                    case "Yellow":
                        Console.ForegroundColor = ConsoleColor.Yellow;

                        break;

                    case "Red":

                        Console.ForegroundColor = ConsoleColor.Red;

                        break;
                }

                Console.WriteLine("This is a code " + code);

                Console.ResetColor();

                Console.WriteLine();
                Console.WriteLine("-----");
                Console.WriteLine();
            }
        }
    }
}