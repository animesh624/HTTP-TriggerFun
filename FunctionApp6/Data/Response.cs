﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FunctionApp6.Data
{
    public class Response<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }

        public Response(string message, T data)
        {

            Message = message;
            Data = data;
        }
    }
}
