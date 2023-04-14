using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;

namespace MyLineChatBot_InChatGPT_demo.Controllers
{
    public class ChatGPT
    {
        public static Result CallChatGPT(string msg)
        {
            HttpClient client = new HttpClient();
            string uri = "https://api.openai.com/v1/chat/completions";

            // Request headers.
            client.DefaultRequestHeaders.Add(
                "Authorization", ConfigurationManager.AppSettings["OpenAI_SecretKey"]);

            var JsonString = @"
            {
  ""model"": ""gpt-3.5-turbo"",
  ""messages"": [{""role"": ""user"", ""content"":""question"" }]
}
            ".Replace("question", msg);
            var content = new StringContent(JsonString, Encoding.UTF8, "application/json");
            var response = client.PostAsync(uri, content).Result;
            var JSON = response.Content.ReadAsStringAsync().Result;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Result>(JSON);
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Choice
    {
        public int index { get; set; }
        public Message message { get; set; }
        public string finish_reason { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class Result
    {
        public string id { get; set; }
        public string @object { get; set; }
        public int created { get; set; }
        public List<Choice> choices { get; set; }
        public Usage usage { get; set; }
    }

    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }
}