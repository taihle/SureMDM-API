// Description: Rebooting device using SureMDM apis.

using System;
using SureMdm;

namespace RebootDevice
{
    class RebootDevice
    {
        static void Main(string[] args)
        {
            string deviceName = "Device 1";     // Name of the device 
            string DeviceID = Helper.GetDeviceID(deviceName);
            if (string.IsNullOrEmpty(DeviceID))
            {
                // Rebooting device app info
                string status = Reboot(DeviceID);
                Console.WriteLine(status);
            }
            else
            {
                Console.WriteLine("Device not found!");
            }
            Console.ReadKey();
        }

        // Function to reboot device app info
        private static string Reboot(string deviceID)
        {
            return Helper.ExecuteDynamicJob(deviceID, "Reboot");
        }
    }
}
