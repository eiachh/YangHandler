using System;
using System.Collections.Generic;
using System.Text;
using YangHandlerTool;
using System.Xml.Linq;

namespace YangHandler.Nodes
{
    public class Grouping : ContainerCapability
    {
        public static List<Grouping> GruopingList = new List<Grouping>();
        public static List<Uses> UsesWaitingForSpecifiedGrouping = new List<Uses>();

        public Grouping(string name) : base(name)
        {
            GruopingList.Add(this);
            foreach (var uses in UsesWaitingForSpecifiedGrouping)
            {
                if (uses.Name == name)
                {
                    uses.ContainedGrouping = this;
                    UsesWaitingForSpecifiedGrouping.Remove(uses);
                }
            }
        }

        public static Grouping GetGroupingByName(string groupingname, Uses caller)
        {
            foreach (var grouping in GruopingList)
            {
                if (grouping.Name == groupingname)
                {
                    return grouping;
                }
            }
            RegisterUsesForWaitingList(caller);
            return null;
        }
        private static void RegisterUsesForWaitingList(Uses uses)
        {
            UsesWaitingForSpecifiedGrouping.Add(uses);
        }

        public override string NodeAsYangString(int identationlevel)
        {
            throw new NotImplementedException();
        }

        public override string NodeAsYangString()
        {
            throw new NotImplementedException();
        }

        public XElement[] NodeAsXmlForUses()
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
