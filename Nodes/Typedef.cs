using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace YangHandlerTool
{
    class Typedef : YangNode
    {
        public string ContainedType { get; set; }
        public string Range { get; set; }
        public Typedef(string name) : base(name) { }

        public override XElement NodeAsXML()
        {
            return new XElement("Typedef" + Name + "Example", "value");
        }
    }
}
