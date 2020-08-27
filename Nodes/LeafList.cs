using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using YangHandler.Nodes.Property;
using YangHandler.Nodes;

namespace YangHandlerTool
{
    public class LeafList : Leaf
    {
        public LeafList(string leafname) : base(leafname) { }
        public LeafList(string leafname, SimpleTypeNode type) : base(leafname, type) { }
        public LeafList(string leafname, SimpleTypeNode type, Description description) : base(leafname, type, description) { }

        public override XElement[] NodeAsXML()
        {
            return new XElement[] { new XElement(Name, "Example Content1"), new XElement(Name, "Example Content2") };
        }
    }
}
