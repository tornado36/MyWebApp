using Castle.MicroKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MyWebApp.Enum.MyWebAppEnum;

namespace MyWebApp.Class
{
    public interface IUser
    {
        public string ID { get; set; }
        public string Username { get; set; }
        public Gender Gender { get; set; }
    }
    public class User : IUser
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string Username { get; set; }
        public double Height { get; set; }
        public Gender Gender { get; set; }

    }
}
