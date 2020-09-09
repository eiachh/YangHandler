using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using YangHandlerTool;
using YangHandler.Nodes.Property;

namespace YangHandler.Nodes
{
    public class YangEnum : YangNode
    {
        public YangEnum(string name) : base(name) { }

        public Description Description { get; set; }
        public override XElement[] NodeAsXML()
        {
            throw new NotImplementedException();
        }

        public override string NodeAsYangString(int identationlevel)
        {
            throw new NotImplementedException();
        }

        public override string NodeAsYangString()
        {
            throw new NotImplementedException();
        }
    }
}
