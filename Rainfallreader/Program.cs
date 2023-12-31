﻿/*
 * Written by Martin Hill for Nov 2023 Coding challenge.
 *
 * CURRENT VERSION: 0.5
 */
namespace RainfallReader
{
    public class RainfallReader
    {
        private static DateTime CurrentTime;

        private static List<Device> Devices;

        public static void Main()
        {
            Console.WriteLine("Welcome to Fuzion Inc. Flood Detection System v0.5");
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

                    ReadRainfall();
                }

                if (input == "report")
                {
                    // Make sure the user doesn't read an empty list.
                    if (Devices == null ||Devices.Count == 0)
                    {
                        Console.WriteLine("No Device values have been loaded. Please load devices first.");

                        continue;
                    }

                    Report();
                }
            }
            while (input.ToLower() != "quit");

            Console.WriteLine("Goodbye!");
        }

        // Read the rainfall values for all devices.
        private static void ReadRainfall()
        {
            CurrentTime = DateTime.MinValue;

            // Would normally use WinForms for this, but apparently that's not a standard package anymore.
            Console.WriteLine("What is the directory to the location of the rainfall csv data files?");
            string folderPath = Console.ReadLine();

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine("Was unable to read given directory.");

                return;
            }

            // Read all the rainfall per device
            Devices.ForEach(device =>
            {
                DateTime lastDateTime = device.ReadRainfall(folderPath);

                if (lastDateTime > CurrentTime)
                {
                    CurrentTime = lastDateTime;
                }
            });
        }

        // Generate a report based on device rainfall events.
        private static void Report()
        {
            foreach (Device device in Devices)
            {
                float average = device.GetAverage(CurrentTime);
                string code = Device.GetCode(average);

                if (device.EmergencyCode(CurrentTime))
                {
                    code = "Red";
                }

                Console.WriteLine("Status report for " + device.DeviceName + ":");
                Console.WriteLine("Location: " + device.Location);

                Console.WriteLine();

                Console.WriteLine("Average rainfall over the last 4 hours: " + average );

                // Colour the output based on code.
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

                if (device.CalculateTrend(CurrentTime))
                {
                    Console.WriteLine("Currently trending upwards");
                }
                else
                {
                    Console.WriteLine("Currently trending downwards");
                }

                Console.WriteLine();
                Console.WriteLine("-----");
                Console.WriteLine();
            }
        }
    }
}