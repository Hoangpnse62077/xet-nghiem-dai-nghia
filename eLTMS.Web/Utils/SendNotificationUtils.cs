using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using eLTMS.DataAccess.Models;

namespace eLTMS.Web.Utils
{
    public class SendNotificationUtils
    {
        public static void SendNotification(object data, List<Token> tokens)
        {
            foreach (var token in tokens)
            {
                var dataToSend = new
                {
                    to = token.TokenString,
                    data
                };
                try
                {
                    SendNotificationUtils.SendNotification(dataToSend);
                }
                catch (Exception ex)
                {
                    //
                }
            }
        }

        public static void SendNotification(object data)
        {
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(data);
            Byte[] byteArray = Encoding.UTF8.GetBytes(json);

            SendNotification(byteArray);
        }

        public static void SendNotification(byte[] byteArray)
        {
            string server_api_key = ConfigurationManager.AppSettings["SERVER_API_KEY"]; 
            string sender_id = ConfigurationManager.AppSettings["SENDER_ID"]; 

            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "POST";
            tRequest.ContentType = "application/json";
            tRequest.Headers.Add($"Authorization: key={server_api_key}");
            tRequest.Headers.Add($"Sender: id={sender_id}");

            tRequest.ContentLength = byteArray.Length;
            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();
            dataStream = tResponse.GetResponseStream();
            StreamReader tReader = new StreamReader(dataStream);

            string sResponseFromServer = tReader.ReadToEnd();

            tReader.Close();
            dataStream.Close();
        }

    }
}
