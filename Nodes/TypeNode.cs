using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using YangHandler.Nodes.Property;

namespace YangHandlerTool
{
    public class TypeNode : YangNode
    {
        public Range Range { get; set; }
        public TypeNode(string name) : base(name)
        {
            if(YangTypes.IsValidType(name))
            {
                throw new System.ArgumentException("Not a valid type", "The given type for TypeNode is not a valid type: "+name);
            }
        }
        public override XElement NodeAsXML()
        {
            throw new NotImplementedException();
        }

        public override string NodeAsYangString()
        {
            if (Range == null)
            {
                return string.Format("type {0};",Name);
            }
            return "type {0} {\r\n\t" + Range.PropertyAsYangText() + "}";
        }
    }
}
