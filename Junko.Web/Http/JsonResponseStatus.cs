using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.Http
{
    public static class JsonResponseStatus
    {
        public static JsonResult SendStatus(JsonResponseStatusType type, string message, object data)
        {
            return new JsonResult(new { status = type.ToString(), message = message, data = data });
        }
    }


    public enum JsonResponseStatusType
    {
        Success,
        Warning,
        Danger,
        Info
    }

}
