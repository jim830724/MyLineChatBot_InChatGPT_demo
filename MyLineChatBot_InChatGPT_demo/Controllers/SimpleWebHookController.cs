using isRock.LineBot;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using MyLineChatBot_InChatGPT_demo.Controllers;
using MyLineChatBot_InChatGPT_demo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.Http;

namespace SimpleLineWebHook.Controllers
{
    public class SimpleWebHookController : isRock.LineBot.LineWebHookControllerBase
    {

        public static LineStickerDefinitions lineStickerDefinitions = new LineStickerDefinitions();
        public string AdminUserId = ConfigurationManager.AppSettings["Line_MyUserId"];

        [Route("api/SimpleWebHook")]
        [HttpPost]
        public IHttpActionResult POST()
        {
            //var AdminUserId = ConfigurationManager.AppSettings["Line_MyUserId"];
            
            //Bot初始化
            Bot bot = new isRock.LineBot.Bot(ConfigurationManager.AppSettings["Line_ChannelAccessToken"]);
            
            //MessageBase初始化
            List<isRock.LineBot.MessageBase> MessageBaseList = new List<isRock.LineBot.MessageBase>();
            
            //官方貼圖初始化(詳見class中的Json字串)
            lineStickerDefinitions.Initial();

            try
            {
                //初始化回應字串
                var responseMsg = "";

                //配合Line Verify
                if (ReceivedMessage.events == null || ReceivedMessage.events.Count() <= 0 ||
                    ReceivedMessage.events.FirstOrDefault().replyToken == "00000000000000000000000000000000") return Ok();
                //取得Line Event
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                

                //整合使用
                //         ***   重點!!! ReplyToken只能使用一次!!!   ***
                //如果只做一件事(ex.回一個貼圖、回一則訊息)，請使用 bot.ReplyMessage(token, --根據方式夾帶參數--)
                //如果要做多件事(ex.同時回一個貼圖和訊息)，請先根據回應的 MessageBase基底，Add至 MessageBaseList
                //最後再call bot.ReplyMessage(token, MessageBaseList);
                switch (LineEvent.type.ToLower())
                {
                    case "message":
                        {
                            switch (LineEvent.message.type)
                            {
                                case "text":
                                    {
                                        if (LineEvent.message.text[0] == '!')
                                        {
                                            var GptResult = ChatGPT.CallChatGPT(LineEvent.message.text.Substring(1, LineEvent.message.text.Length - 1)).choices[0].message.content;
                                            responseMsg = ChineseConverter.Convert($"{GptResult}", ChineseConversionDirection.SimplifiedToTraditional); ;
                                            MessageBaseList.Add(new TextMessage(responseMsg));
                                        }
                                        else 
                                            break;                                        
                                        break;//responseMsg = $"收到 event : {LineEvent.type} type: {LineEvent.message.type} ";
                                    }                                    
                                case "image":
                                    break;
                                case "video":
                                    break;
                                case "audio":
                                    break;
                                case "location":
                                    break;
                                case "sticker":
                                    {
                                        //LineEvent.message.packageId, LineEvent.message.stickerId
                                        var result = lineStickerDefinitions.Item.stickers.FindAll(p => p.packageId == LineEvent.message.packageId);
                                        if (result.Count > 0)
                                        {
                                            var result2 = result[0].stickerId.FindAll(s => s == LineEvent.message.stickerId);
                                            if (result2.Count > 0)
                                            {
                                                MessageBaseList.Add(new TextMessage("我可以傳跟你一樣的貼圖!!!"));
                                                MessageBaseList.Add(new StickerMessage(LineEvent.message.packageId, LineEvent.message.stickerId));
                                            }
                                            else
                                                StickerReply_Sorry_RJ(ref MessageBaseList);
                                        }
                                        else
                                            StickerReply_Sorry_RJ(ref MessageBaseList);
                                    }
                                    break;
                                default:
                                    {
                                        responseMsg = $"收到 event : {LineEvent.type} ";
                                        //bot.ReplyMessage(LineEvent.replyToken, responseMsg);
                                        MessageBaseList.Add(new TextMessage(responseMsg));
                                        break;
                                    }
                                    
                            }
                            break;
                        }
                    default:
                        break;
                }


                //準備回覆訊息
                //if (LineEvent.type.ToLower() == "message" && LineEvent.message.type == "text" && LineEvent.message.text[0] == '!')
                //{
                //    var GptResult = ChatGPT.CallChatGPT(LineEvent.message.text.Substring(1, LineEvent.message.text.Length-1)).choices[0].message.content;
                //    responseMsg = ChineseConverter.Convert($"{GptResult}", ChineseConversionDirection.SimplifiedToTraditional); ;
                //}
                //else if (LineEvent.type.ToLower() == "message")
                //    responseMsg = $"收到 event : {LineEvent.type} type: {LineEvent.message.type} ";
                //else
                //    responseMsg = $"收到 event : {LineEvent.type} ";
                ////回覆訊息
                ////this.ReplyMessage(LineEvent.replyToken, responseMsg);
                //bot.ReplyMessage(LineEvent.replyToken, responseMsg);

                if (MessageBaseList.Count > 6)
                    throw new Exception("MessageBaseList內含回應超過5個。");
                else if (MessageBaseList.Count <= 5 && MessageBaseList.Count > 0)
                    bot.ReplyMessage(LineEvent.replyToken, MessageBaseList);
                else { }
                    
                
                return Ok();


            }
            catch (Exception ex)
            {
                //回覆訊息
                //bot.ReplyMessage(LineEvent.replyToken, responseMsg);
                bot.PushMessage(ConfigurationManager.AppSettings["Line_MyUserId"], "發生錯誤:\n" + ex.Message);
                bot.PushMessage(ConfigurationManager.AppSettings["Line_MyUserId"], "發生錯誤:\n" + ex.StackTrace);

                //response OK
                return Ok();
            }
        }


        public void StickerReply_Sorry_RJ(ref List<MessageBase> mbList, [Optional]string msg)
        { 
            if(string.IsNullOrEmpty(msg))
                mbList.Add(new TextMessage("不好意思捏~我沒有跟你一樣的貼圖~"));
            else
                mbList.Add(new TextMessage(msg));

            mbList.Add(new StickerMessage(11539, 52114115)); //RJ-不失禮貌的微笑


        }

    }
}
