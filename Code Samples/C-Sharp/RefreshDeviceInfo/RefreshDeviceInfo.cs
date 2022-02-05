// Description: Applying dynamic job to refresh device info using SureMDM apis.

using System;
using SureMdm;

namespace RefreshDeviceInfo
{
    class RefreshDeviceInfo
    {
        static void Main(string[] args)
        {
            string deviceName = "Device 1";     // Name of the device 
            string DeviceID = Helper.GetDeviceID(deviceName);
            if (DeviceID != null)
            {
                // Refreshing device
                string status = RefreshDevice(DeviceID);
                Console.WriteLine(status);
            }
            else
            {
                Console.WriteLine("Device not found!");
            }
            Console.ReadKey();
        }

        // Function to refresh device info
        private static string RefreshDevice(string deviceID)
        {
            return Helper.ExecuteDynamicJob(deviceID, "Refresh_Device");
        }
    }
}
