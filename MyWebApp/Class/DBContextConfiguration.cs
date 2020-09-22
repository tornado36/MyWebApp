using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Class
{
    public interface IDBContextConfiguration
    {
        string DatabaseName { get; }
        string ConnectionString { get; }
    }
}
