using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using YangHandler.Nodes.Property;
using YangHandler.Nodes;

namespace YangHandlerTool
{
    public class Leaf :YangNode
    {
        public bool Config { get; set; }

        public Leaf(string leafname) : base(leafname)
        {
            Description = new Description();
            Config = false;
        }

        public Leaf(string leafname, SimpleTypeNode type)                                       : this(leafname)              {Type = type;}
        public Leaf(string leafname, SimpleTypeNode type, bool config)                          : this(leafname)              {Type = type;Config = config;}
        public Leaf(string leafname, SimpleTypeNode type, Description description)              : this(leafname,type)         { Description = description; }
        public Leaf(string leafname, SimpleTypeNode type, bool config, Description description) : this(leafname, type, config){Description = description;}

        public override XElement[] NodeAsXML()
        {
            return new XElement[] { new XElement(Name, "Example Content") };
        }

        public override string NodeAsYangString()
        {
            throw new NotImplementedException();
        }

        public override string NodeAsYangString(int identationlevel)
        {
            throw new NotImplementedException();
        }
    }
}
