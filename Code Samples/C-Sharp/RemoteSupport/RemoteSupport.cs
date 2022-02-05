using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SureMdm;

namespace RemoteSupport
{
    class RemoteSupport
    {
        static void Main(string[] args)
        {
            string deviceName = "device 1"; // name of the device
            // Generate remote support URL for device
            string URL = GetRemoteSupportURL(deviceName);
            Console.WriteLine(URL);
            Console.ReadKey();
        }

        // get URL for remote support of the device
        private static string GetRemoteSupportURL(string deviceName)
        {
            // Retrieving ID of the device
            string deviceID = Helper.GetDeviceID(deviceName);

            RestClient client = Helper.GetRestClient("/device/" + deviceID);

            // Set request method
            var request = new RestRequest(Method.GET);
            // Getting response
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // var OutPut = JsonConvert.DeserializeObject(response.Content);
                List<JObject> jsonResponse = JsonConvert.DeserializeObject<List<JObject>>(response.Content);
                // var a = jsonResponse.Any();
                if ((jsonResponse != null) && (jsonResponse.Any()))
                {
                    foreach (var device in jsonResponse)
                    {
                        if (device.GetValue("DeviceName").ToString() == deviceName)
                        {
                            string remoteSupporturl = "https://suremdm.42gears.com" + "/RemoteSupport.aspx?" +
                                "id=" + device.GetValue("DeviceID").ToString() +
                                "&name=" + device.GetValue("DeviceName").ToString() +
                                "&userid=" + device.GetValue("UserID").ToString() +
                                "&pltFrmType=" + device.GetValue("PlatformType").ToString() +
                                "&agentversion=" + device.GetValue("AgentVersion").ToString() +
                                "&perm=126,127,128,129";

                            return remoteSupporturl;
                        }
                    }
                }
            }
            return null;
        }
    }
}
