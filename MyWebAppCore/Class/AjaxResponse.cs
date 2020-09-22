using System;
using System.Collections.Generic;
using System.Text;

namespace MyWebAppCore.Class
{
    public class AjaxResponse
    {
        public bool Succeed { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
