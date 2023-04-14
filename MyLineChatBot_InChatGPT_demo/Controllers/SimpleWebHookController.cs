using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace MyLineChatBot_InChatGPT_demo.Controllers
{
    public class SimpleWebHookController : isRock.LineBot.LineWebHookControllerBase
    {
        // GET: SimpleWebHook
        [Route("api/SimpleWebHook")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            var MyUserId = ConfigurationManager.AppSettings["Line_MyUserId"];
            try
            {
                if (ReceivedMessage.events == null || ReceivedMessage.events.Count() <= 0 ||
                    ReceivedMessage.events.FirstOrDefault().replyToken == "00000000000000000000000000000000") 
                    return Ok();

                //設定Line ChannelAccessToken(或抓取Web.Config)
                this.ChannelAccessToken = ConfigurationManager.AppSettings["Line_ChannelAccessToken"] ;
                //取得Line Event
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                //回覆訊息
                var responseMsg = "";
                if (LineEvent.message.text[0] == '!')
                {
                    var GptResult = ChatGPT.CallChatGPT(LineEvent.message.text).choices[0].message.content;
                    responseMsg = $"{GptResult}";
                }
                else
                {
                    responseMsg = "你說了:" + LineEvent.message.text;                    
                }
                this.ReplyMessage(LineEvent.replyToken, responseMsg);
                //response OK
                return Ok();
            }
            catch (Exception ex)
            {
                //回覆訊息
                this.PushMessage(MyUserId , "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }
    }
}