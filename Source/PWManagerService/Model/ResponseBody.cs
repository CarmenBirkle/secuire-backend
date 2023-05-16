using Microsoft.Identity.Client;
using PWManagerServiceModelEF;
using System.Net;

namespace PWManagerService
{
    public abstract class ResponseBody<T>
    {
        public string ResponseMessage { get; set; } = string.Empty;
        //public string? ResponseType { get { return typeof(T).Name; } }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess => (int)StatusCode >= 200 && (int)StatusCode <= 299;
        public T? Data { get; set; }

    }

    public class GetResponseBody<T> : ResponseBody<T>
    {
    }

    public class PostResponseBody<T> : ResponseBody<T>
    {
        public string RessourceLocation { get; set; } = string.Empty;
    }

}
