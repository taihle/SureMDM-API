// Description: Program for applying job on the device using SureMDM apis.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SureMdm;

namespace ApplyJob
{
    class JobApply
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Apply job on the device.");

            string DeviceName = "Device 1";     // Name of the device on which job will be applied
            string JobName = "Test Job";        // Name of the job which will be applied
            string FolderName = "";             // Name of the jobfolder where the job is stored
            // Note: Keep FolderName empty if job is stored in root folder

            // Retrieve ID of the device using name of device
            string DeviceID = Helper.GetDeviceID(DeviceName);
            if (!string.IsNullOrEmpty(DeviceID))
            {
                // Retrieve ID of the job using name of the job
                string JobID = GetJobID(JobName, FolderName);
                if (JobID != null)
                {
                    // Apply job on the device
                    ApplyJob(DeviceID, JobID);
                }
                else
                {
                    Console.WriteLine("\nJob not found!");
                }
            }
            else
            {
                Console.WriteLine("\nDevice not found!");
            }
            Console.Write("\nPress any key to end...");
            Console.ReadKey();
        }

        // Function to apply job using 'Apply job on device' api
        private static void ApplyJob(string deviceID, string jobID)
        {
            RestClient client = Helper.GetRestClient("/jobassignment");
            // RequestBody for apply job
            var RequestBody = new
            {
                DeviceIds = new List<string>() { deviceID },
                JobId = jobID
            };

            // Set request method
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(RequestBody);

            // Execute request
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (Convert.ToBoolean(response.Content.ToString()))
                {
                    Console.WriteLine("\nJob applied successfully!");
                }
                else
                {
                    Console.WriteLine("\nFailed to apply job!");
                }
            }
            else
            {
                Console.WriteLine(response.Content.ToString());
            }
        }

        // Function to retrieve ID of the job using 'Get all jobs' api
        private static string GetJobID(string jobName, string folderName)
        {
            // FolderID for root folder is "null"
            string FolderID = string.IsNullOrWhiteSpace(folderName) ? "null" : Helper.GetFolderID(folderName);

            RestClient client = Helper.GetRestClient("/job");

            // Set request method
            var request = new RestRequest(Method.GET);
            request.AddParameter("FolderID", FolderID, ParameterType.QueryString);
            // Execute request
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var OutPut = JsonConvert.DeserializeObject(response.Content);
                var jsonResponse = JsonConvert.DeserializeObject<List<JObject>>(response.Content);
                if ((jsonResponse != null) && (jsonResponse.Any()))
                {
                    foreach (var job in jsonResponse)
                    {
                        if (job.GetValue("JobName").ToString() == jobName)
                        {
                            return job.GetValue("JobID").ToString();
                        }
                    }
                }
            }
            return null;
        }
    }
}
