using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SureMdm
{
    // TODO: read from .evn/.cfg file
    static class Constants
    {
        public const string BaseURL = "https://suremdm.42gears.com/api";  // Your SureMDM domain
        public const string Username = "Username"; // Your SureMDM username 
        public const string Password = "Password"; // Your SureMDM password
        public const string ApiKey = "Your ApiKey"; // Your SureMDM apikey
    }

    public class Helper
    {
        public static RestClient GetRestClient(string path)
        {
            string URL = Constants.BaseURL + path;
            var client = new RestClient(URL);
            // Basic authentication header
            client.Authenticator = new HttpBasicAuthenticator(Constants.Username, Constants.Password);
            // ApiKey Header
            client.AddDefaultHeader("ApiKey", Constants.ApiKey);
            // Set content type
            client.AddDefaultHeader("Content-Type", "application/json");

            return client;
        }

        // Retrieve Device ID
        public static string GetDeviceID(string deviceName)
        {
            RestClient client = GetRestClient("/device");

            //  Set request method
            var request = new RestRequest(Method.POST);

            //  Request payload
            var RequestPayLoad = new
            {
                ID = "AllDevices",
                IsSearch = true,
                Limit = 10,
                SearchColumns = new string[] { "DeviceName" },
                SearchValue = deviceName,
                SortColumn = "LastTimeStamp",
                SortOrder = "asc"
            };
            request.AddJsonBody(RequestPayLoad);

            //  Execute request
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var OutPut = JsonConvert.DeserializeObject<JArray>(response.Content);
                foreach (var device in OutPut["rows"])
                {
                    if ((string)device["DeviceName"] == deviceName)
                    {
                        return device["DeviceID"].ToString();
                    }
                }
            }
            return null;
        }

        // Retrieve Group ID
        public static string GetGroupID(string groupName)
        {
            // For home group no need to get groupID
            if (string.Equals(groupName, "Home", StringComparison.InvariantCultureIgnoreCase))
            {
                return groupName;
            }

            RestClient client = GetRestClient("/group/1/getall");

            // Set request method
            var request = new RestRequest(Method.GET);
            // Getting response
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var ret = JsonConvert.DeserializeObject<JObject>(response.Content);
                foreach (var group in ret["Groups"])
                {
                    if ((string)group["GroupName"] == groupName)
                    {
                        Console.WriteLine("hello " + group["GroupID"].ToString());
                        return group["GroupID"].ToString();
                    }
                }
            }
            return null;
        }

        // Function to retrieve ID of the jobfolder using 'Get all folders' api
        public static string GetFolderID(string folderName)
        {
            RestClient client = GetRestClient("jobfolder/all");

            // Set request method
            var request = new RestRequest(Method.GET);

            // Execute request
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // var OutPut = JsonConvert.DeserializeObject(response.Content);
                List<JObject> jsonResponse = JsonConvert.DeserializeObject<List<JObject>>(response.Content);
                if ((jsonResponse != null) && (jsonResponse.Any()))
                {
                    foreach (var folder in jsonResponse)
                    {
                        if (folder.GetValue("FolderName").ToString() == folderName)
                        {
                            return folder.GetValue("FolderID").ToString();
                        }
                    }
                }
            }
            return null;
        }

        // Method to get application ID
        public static string GetAppID(string deviceID, string appName)
        {
            RestClient client = GetRestClient("/installedapp/android/" + deviceID + "/device");

            // Set request method
            var request = new RestRequest(Method.GET);

            // Execute request
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // var OutPut = JsonConvert.DeserializeObject(response.Content);
                List<JObject> jsonResponse = JsonConvert.DeserializeObject<List<JObject>>(response.Content);
                // var a = jsonResponse.Any();
                if ((jsonResponse != null) && (jsonResponse.Any()))
                {
                    foreach (var app in jsonResponse)
                    {
                        if (app.GetValue("Name").ToString().ToUpper() == appName.ToUpper())
                        {
                            return app.GetValue("Id").ToString();
                        }
                    }
                }
            }
            return null;
        }

        // Function to execute dynamicjob on a device
        public static string ExecuteDynamicJob(string deviceID, string jobType)
        {
            RestClient client = GetRestClient("/dynamicjob");

            // Set request method
            var request = new RestRequest(Method.POST);

            // Request payload for reboot device dynamic job
            var RequestPayLoad = new
            {
                JobType = jobType,
                DeviceID = deviceID
            };
            request.AddJsonBody(RequestPayLoad);

            // Execute request
            IRestResponse response = client.Execute(request);

            return response.Content.ToString();
        }

        public static string ExecuteDynamicJob(string deviceID, string jobType, string payLoadBase64)
        {
            RestClient client = GetRestClient("/dynamicjob");

            // Set request method
            var request = new RestRequest(Method.POST);

            // Request payload for reboot device dynamic job
            var RequestPayLoad = new
            {
                JobType = jobType,
                DeviceID = deviceID,
                PayLoad = payLoadBase64
            };

            request.AddJsonBody(RequestPayLoad);

            // Execute request
            IRestResponse response = client.Execute(request);

            return response.Content.ToString();
        }

    }
}
