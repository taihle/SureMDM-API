// Description: Uninstall application from device

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SureMdm;

namespace UninstallApp
{
    class UninstallApp
    {
        static void Main(string[] args)
        {
            string deviceName = "device 1"; // name of the device
            string appName = "AstroContacts"; // name of the application which you want to uninstall
            // Retrieve Device ID
            string DeviceID = Helper.GetDeviceID(deviceName);
            if (DeviceID != null)
            {
                // Uninstall app
                string status = UninstallApplication(DeviceID, appName);
                Console.WriteLine(status);
            }
            else
            {
                Console.WriteLine("Device not found!");
            }
            Console.ReadKey();
        }

        // method to uninstall application
        private static string UninstallApplication(string deviceID, string appName)
        {
            // Create job specific PayLoad
            var PayLoad = new
            {
                AppIds = new string[] { Helper.GetAppID(deviceID, appName) }
            };
            // convert payload to base64 string
            var payLoadbytes = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(PayLoad));
            var payLoadBase64 = System.Convert.ToBase64String(payLoadbytes);

            return Helper.ExecuteDynamicJob(deviceID, "UNINSTALL_APPLICATION", payLoadBase64);

        }

    }
}
