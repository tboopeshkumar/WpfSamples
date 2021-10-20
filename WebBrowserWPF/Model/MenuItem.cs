using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace WebBrowser.Model
{
    [Serializable]
    internal class MenuItem
    {        
        public string Name { get; set; }
        public string Url { get; set; }
        public List<MenuItem> Items { get; set; }
        public MenuItem Parent { get; set; }          
        public OperationType OperationType { get; set; }

    }

    internal enum OperationType
    {
        None,
        Open, 
        Modify,
        Delete,
    }
}
