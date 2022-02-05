// Description: Re-apply all pending jobs from device jobqueue

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SureMdm;

namespace ReapplyJob
{
    class ReapplyJob
    {
        static void Main(string[] args)
        {
            string deviceName = "Device 1";     // Name of the device 
            string DeviceID = Helper.GetDeviceID(deviceName);
            if (DeviceID != null)
            {
                string status = Jobqueue(DeviceID, false); // true to get all jobs and false to get only pending jobs
                Console.WriteLine(status);
            }
            else
            {
                Console.WriteLine("Device not found!");
            }
            Console.ReadKey();
        }

        // Function for retrieving jobqueue of device
        private static string Jobqueue(string deviceID, Boolean bShowAll)
        {
            RestClient client = Helper.GetRestClient("/jobqueue/" + deviceID + "/" + bShowAll);
            // Set request method
            var request = new RestRequest(Method.GET);
            // Execute method
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                List<JObject> jsonResponse = JsonConvert.DeserializeObject<List<JObject>>(response.Content);
                if ((jsonResponse != null) && (jsonResponse.Any()))
                {
                    foreach (var status in jsonResponse)
                    {
                        if (status.GetValue("Status").ToString() == "ERROR" || status.GetValue("Status").ToString() == "FAILED" || status.GetValue("Status").ToString() == "SCHEDULED")
                        {
                            string rowID = status.GetValue("RowId").ToString();
                            string jobID = status.GetValue("JobID").ToString();
                            Console.WriteLine(Reapply(deviceID, jobID, rowID)); // initiate reapplying
                        }
                    }
                    return "Success";
                }
            }
            return "Failed";
        }

        // Function to re-apply all the pending jobs
        private static string Reapply(string deviceID, string jobID, string rowID)
        {
            RestClient client = Helper.GetRestClient("/jobqueue/" + jobID + "/" + deviceID + "/" + rowID);
            // Set request method
            var request = new RestRequest(Method.PUT);
            // Execute request
            IRestResponse response = client.Execute(request);
            return response.Content.ToString();
        }

    }
}
