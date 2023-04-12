using System.Web;
using System.Web.Mvc;

namespace MyLineChatBot_InChatGPT_demo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
