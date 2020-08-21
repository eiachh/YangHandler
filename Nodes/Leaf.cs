using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace YangHandlerTool
{
    public class Leaf :YangNode
    {
        public string Type { get; set; }
        public bool Config { get; set; }

        public Leaf(string leafname) : base(leafname)
        {
            Description = string.Empty;
            Config = false;
        }
        public Leaf(string leafname, string type)                                  : this(leafname)              {Type = type;}
        public Leaf(string leafname, string type, bool config)                     : this(leafname)              {Type = type;Config = config;}
        public Leaf(string leafname, string type, string description)              : this(leafname,type)         { Description = description; }
        public Leaf(string leafname, string type, bool config, string description) : this(leafname, type, config){Description = description;}

        override public XElement NodeAsXML()
        {
            return new XElement(Name, "Example Content");
        }

    }
}
