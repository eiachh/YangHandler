using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using YangHandler.Nodes;

namespace YangHandlerTool
{
    public class Typedef : SingleItemContainer
    {
        public TypeNode ContainedType;
        public Typedef(string name) : base(name) { }


        public override XElement[] NodeAsXML()
        {
            return new XElement[] { new XElement(Name, "Typedef value") };
        }

        public override string NodeAsYangString()
        {
            if (ContainedType == null)
            {
                return "typedef " + Name + " {\r\n\t" + "Empty" + "\r\n}";
            }
            return "typedef "+Name+" {\r\n\t"+ContainedType.NodeAsYangString()+"\r\n}";
        }
        public override string NodeAsYangString(int identationlevel)
        {
            return GetIdentation(identationlevel) + "typedef " + Name + " {\r\n" +
                   GetIdentation(identationlevel) + "\t" + ContainedType.NodeAsYangString(identationlevel+1) + "\r\n" +
                   GetIdentation(identationlevel) + "}";
        }
    }
}
