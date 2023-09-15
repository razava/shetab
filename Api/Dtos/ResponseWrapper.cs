using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Dtos
{
    public class ResponseWrapper<T>
    {
        public T Payload { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
