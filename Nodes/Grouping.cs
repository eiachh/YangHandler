using System;
using System.Collections.Generic;
using System.Text;
using YangHandlerTool;
using System.Xml.Linq;

namespace YangHandler.Nodes
{
    class Grouping : ContainerCapability
    {
        public Grouping(string name) : base(name) { }
        public override string NodeAsYangString(int identationlevel)
        {
            throw new NotImplementedException();
        }

        public override string NodeAsYangString()
        {
            throw new NotImplementedException();
        }

        public XElement[] GetXElemForUsing()
        {
            List<XElement> retchildlist = new List<XElement>();
            foreach (var child in Children)
            {
                retchildlist.AddRange(child.NodeAsXML());
            }
            return retchildlist.ToArray();
        }
    }
}
