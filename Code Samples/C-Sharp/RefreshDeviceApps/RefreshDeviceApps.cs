// Description: Refreshing device application info using SureMDM apis.

using System;
using SureMdm;

namespace RefreshDeviceApps
{
    class RefreshDeviceApps
    {
        static void Main(string[] args)
        {
            string deviceName = "Device 1";     // Name of the device 
            string DeviceID = Helper.GetDeviceID(deviceName);
            if (DeviceID != null)
            {
                // Refreshing device app info
                string status = RefreshApps(DeviceID);
                Console.WriteLine(status);
            }
            else
            {
                Console.WriteLine("Device not found!");
            }
            Console.ReadKey();
        }

        // Function to refresh device app info
        private static string RefreshApps(string deviceID)
        {
            return Helper.ExecuteDynamicJob(deviceID, "GET_DEVICE_APPS");
        }

    }
}
