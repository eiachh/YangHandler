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


        public override XElement NodeAsXML()
        {
            return new XElement("Typedef" + Name + "Example", "value");
        }

        public override string NodeAsYangString()
        {
            return "typedef "+Name+" {\r\n\t"+ContainedType.NodeAsYangString()+"\r\n}";
        }
    }
}
