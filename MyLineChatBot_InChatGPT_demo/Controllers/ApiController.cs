using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using RouteAttribute = System.Web.Mvc.RouteAttribute;

namespace MyLineChatBot_InChatGPT_demo.Controllers
{
    public class ApiController :  isRock.LineBot.LineWebHookControllerBase
    {

        [Route("api/Return")]
        [System.Web.Mvc.HttpPost]
        public IHttpActionResult POST()
        {
            try
            {
                //設定ChannelAccessToken(或不設定直接抓取Web.Config中key為ChannelAccessToken的AppSetting)
                //this.ChannelAccessToken = "!!!!! 改成自己的ChannelAccessToken !!!!!";
                this.ChannelAccessToken = ConfigurationManager.AppSettings["ChannelAccessToken"];
                //↑↑↑↑↑↑↑↑↑↑↑ 此範例採用 appsetting 設定 ChannelAccessToken

                //取得Line Event
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                var responseMsg = "這是預設訊息啦";

                //判斷是否為文字訊息
                if (LineEvent.type.ToLower() == "message" && LineEvent.message.type == "text")
                {
                    var UserSay = LineEvent.message.text.Trim();
                    //判斷是否為教學訊息
                    if (UserSay.Contains("看到") && (UserSay.Contains("回覆") || UserSay.Contains("回復")))
                    {
                        //處理教學動作
                        //responseMsg = Learn(UserSay, LineEvent);
                    }
                    else
                    {
                        //如果沒有教學指令，則從資料庫中找回覆訊息
                        //responseMsg = GetNormalResponse(UserSay, LineEvent);
                    }

                    //回覆訊息
                    this.ReplyMessage(LineEvent.replyToken, responseMsg);
                }
                else
                {
                    //如果是其他型態的訊息

                }

                //response OK
                return Ok();
            }
            catch (Exception ex)
            {
                //回覆訊息
                this.PushMessage("請改成你自己的Admin User Id", "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }
    }
}