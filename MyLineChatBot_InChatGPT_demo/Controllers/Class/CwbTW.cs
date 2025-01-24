using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace MyLineChatBot_InChatGPT_demo.Controllers.Class
{taskkill
    public class CwbTW
    {
        static string url = "https://opendata.cwb.gov.tw/api";// /v1/rest/datastore/F-C0032-001";
        public static string GetCwbData(string input)
        {
            HttpClient client = new HttpClient();

            string getQuery = "?";
            SetGetQueryStr(input, ref getQuery);
            string func = "/v1/rest/datastore/F-C0032-001";
            string total_uri = url + func + SetGetQueryStr(input, ref getQuery);
            // Request headers.
            //client.DefaultRequestHeaders.Add(
            //    "Authorization", ConfigurationManager.AppSettings["CwbTW_AuthKey"]);
            //client.DefaultRequestHeaders.Authorization
            //             = new AuthenticationHeaderValue("Bearer", ConfigurationManager.AppSettings["OpenAI_SecretKey"]);




            //            client.Default


            //            var JsonString = @"
            //            {
            //  ""model"": ""gpt-3.5-turbo"",
            //  ""messages"": [{""role"": ""user"", ""content"":""question"" }]
            //}
            //            ".Replace("question", msg);
            //            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");
            //            var response = client.PostAsync(uri, content).Result;
            //            var JSON = response.Content.ReadAsStringAsync().Result;
            //            return Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(JSON);
            return "";
        }

        public void SetGetQueryStr(string loation,ref string query) 
        {
            switch (loation)
            {
                case "F-C0032-001": {
                        var str = HttpUtility.ParseQueryString(string.Empty);
                        str["Authorization"] = ConfigurationManager.AppSettings["CwbTW_AuthKey"];
                        //str["limit"];
                        //str["offset"] =;
                        //str["format"] ="JSON";
                        str["locationName"] = SetLocationName();
                        //str["elementName"] =;
                        //str["sort"] =;
                        //str["startTime"] =;
                        //str["timeFrom"] =;
                        //str["timeTo"] =;
                        query += str.ToString();


                        break; }
                default: break;
            
            
            }
        }

        private string SetLocationName()
        {
            throw new NotImplementedException();
        }
    }
}