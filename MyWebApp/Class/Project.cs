using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.SignalR;
using MyWebApp.Enum;
using NPOI.HPSF;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MyWebApp.Class
{
    public class Project
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string UserID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool ifDeleted { get; set; }
        public MyWebAppEnum.ProjectType ProjectType { get; set; }
    }
}
