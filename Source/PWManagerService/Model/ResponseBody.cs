﻿using PWManagerServiceModelEF;
using System.Net;

namespace PWManagerService
{
    public class ResponseBody<T>
    {
        public string ResponseMessage { get; set; } = string.Empty;
        public string? ResponseType { get { return typeof(T).Name; } }
        //public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
    }
}