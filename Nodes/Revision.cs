using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace YangHandlerTool

{
    public class Revision : YangNode
    {
        public string Value { get; set; }
        public Revision(string value) : base("revision") { Value = value; }

        public override XElement NodeAsXML()
        {
            return null;
        }

        public override string NodeAsYangString()
        {
            throw new NotImplementedException();
        }
    }
}
