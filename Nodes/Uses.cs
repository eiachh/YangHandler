using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using YangHandlerTool;

namespace YangHandler.Nodes
{
    public class Uses : YangNode
    {
        public Uses(string name) : base(name) { ContainedGrouping = Grouping.GetGroupingByName(name,this); }
        public Grouping ContainedGrouping { get; set; }
        public override XElement[] NodeAsXML()
        {
            return ContainedGrouping.NodeAsXmlForUses();
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
