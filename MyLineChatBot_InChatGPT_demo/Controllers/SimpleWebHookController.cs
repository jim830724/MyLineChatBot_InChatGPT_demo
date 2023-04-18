using isRock.LineBot;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using MyLineChatBot_InChatGPT_demo.Controllers;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace SimpleLineWebHook.Controllers
{
    public class SimpleWebHookController : isRock.LineBot.LineWebHookControllerBase
    {
        [Route("api/SimpleWebHook")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            var AdminUserId = ConfigurationManager.AppSettings["Line_MyUserId"];
            Bot bot = new isRock.LineBot.Bot(ConfigurationManager.AppSettings["Line_ChannelAccessToken"]);
            try
            {
                
                //配合Line Verify
                if (ReceivedMessage.events == null || ReceivedMessage.events.Count() <= 0 ||
                    ReceivedMessage.events.FirstOrDefault().replyToken == "00000000000000000000000000000000") return Ok();
                //取得Line Event
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                var responseMsg = "這是預設訊息";

                //等帶下波整合
                //switch (LineEvent.type.ToLower())
                //{
                //    case "message": {
                //            switch (LineEvent.message.type){
                //                case "text":
                //                    break;//responseMsg = $"收到 event : {LineEvent.type} type: {LineEvent.message.type} ";
                //                case "image":
                //                    break;
                //                case "video":
                //                    break;
                //                case "audio":
                //                    break;
                //                case "location":
                //                    break;
                //                case "sticker":
                //                    break;
                //                default:
                //                    break;
                //            }
                //            break;
                //    }
                //    default: 
                //        break;
                //}


                //準備回覆訊息
                if (LineEvent.type.ToLower() == "message" && LineEvent.message.type == "text" && LineEvent.message.text[0] == '!')
                {
                    var GptResult = ChatGPT.CallChatGPT(LineEvent.message.text.Substring(1, LineEvent.message.text.Length-1)).choices[0].message.content;
                    responseMsg = ChineseConverter.Convert($"{GptResult}", ChineseConversionDirection.SimplifiedToTraditional); ;
                }
                else if (LineEvent.type.ToLower() == "message")
                    responseMsg = $"收到 event : {LineEvent.type} type: {LineEvent.message.type} ";
                else
                    responseMsg = $"收到 event : {LineEvent.type} ";
                //回覆訊息
                //this.ReplyMessage(LineEvent.replyToken, responseMsg);
                bot.ReplyMessage(LineEvent.replyToken, responseMsg);

                return Ok();


            }
            catch (Exception ex)
            {
                //回覆訊息
                bot.PushMessage(ConfigurationManager.AppSettings["Line_MyUserId"], "發生錯誤:\n" + ex.Message);

                //response OK
                return Ok();
            }
        }
    }
}
