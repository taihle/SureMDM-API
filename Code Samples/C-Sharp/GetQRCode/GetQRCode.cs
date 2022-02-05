using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.IO;
using System.Net;
using SureMdm;

namespace GetQRCode
{
    class GetQRCode
    {
        static void Main(string[] args)
        {
            string groupName = "testing devices"; // name of the device where you want enroll device

            //Get base64 QRcode
            string base64String = GetQRCodeImage(groupName);
            base64String = base64String.Replace("\"", "");
            Console.WriteLine(base64String);
            if (base64String != null)
            {
                Byte[] QRCode = Convert.FromBase64String(base64String);
                using (var imageFile = new FileStream("QRCode.png", FileMode.Create))
                {
                    imageFile.Write(QRCode, 0, QRCode.Length);
                    imageFile.Flush();
                }
            }
            else
            {
                Console.WriteLine("Invalid request!");
            }
            Console.ReadKey();
        }

        // methos to get QRCode
        private static string GetQRCodeImage(string groupName)
        {
            RestClient client = Helper.GetRestClient("/QRCode/" + Helper.GetGroupID(groupName) + "/default/true/UseSystemGenerated");
 
            // Set request method
            var request = new RestRequest(Method.GET);
            // Getting response
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK && !string.IsNullOrWhiteSpace(response.Content))
            {
                return response.Content;
            }
            return null;
        }

    }
}
