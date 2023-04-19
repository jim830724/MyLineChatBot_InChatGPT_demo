using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyLineChatBot_InChatGPT_demo.Models
{
    public class LineStickerDefinitions
    {
        
        public List<LineSticker> lineStickers { 
            
        
        }
    }
    public class LineSticker
    {
        public int packageId { get; set; }
        public List<int> stickerId { get; set; }

        public LineSticker(int pid, int sid) {
            //packageId = pid;
            if (!stickerId.Contains(sid))
            {
                packageId = pid;
                stickerId.Add(sid);
            }                        
        }
    }

}