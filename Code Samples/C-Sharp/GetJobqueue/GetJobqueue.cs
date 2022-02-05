// Description: Get job queue details of the device using SureMDM apis.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Net;
using SureMdm;

namespace GetJobqueue
{
    class GetJobqueue
    {
        static void Main(string[] args)
        {
            string deviceName = "Device 1";     // Name of the device 
            string DeviceID = Helper.GetDeviceID(deviceName);
            if (!string.IsNullOrEmpty(DeviceID))
            {
                string retVal = Jobqueue(DeviceID, true); // true to get all jobs and false to get only pending jobs
                Console.WriteLine(retVal);
            }
            else
            {
                Console.WriteLine("Device not found!");
            }
            Console.ReadKey();
        }

        // Function to retrieve device jobqueue
        private static string Jobqueue(string deviceID, Boolean bShowAll)
        {
            RestClient client = Helper.GetRestClient("/jobqueue/" + deviceID + "/" + bShowAll);
            // Set request method
            var request = new RestRequest(Method.GET);
            // Execute method
            IRestResponse response = client.Execute(request);

            return response.Content.ToString();
        }
    }
}
