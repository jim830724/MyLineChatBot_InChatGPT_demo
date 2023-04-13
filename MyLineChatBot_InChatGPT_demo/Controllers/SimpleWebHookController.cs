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
            var MyUserId = ConfigurationManager.AppSettings["MyUserId"];
            try
            {
                
                //設定ChannelAccessToken(或抓取Web.Config)
                this.ChannelAccessToken = ConfigurationManager.AppSettings["ChannelAccessToken"] ;
                //取得Line Event
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                //回覆訊息
                this.ReplyMessage(
                    LineEvent.replyToken, "你說了:" + LineEvent.message.text);
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