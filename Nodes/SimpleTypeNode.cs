using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using YangHandlerTool;

namespace YangHandler.Nodes
{
    public class SimpleTypeNode : YangNode
    {
        public SimpleTypeNode(string name) : base(name)
        {
            if (!YangTypes.IsValidType(name))
            {
                throw new ArgumentException("The given type for TypeNode is not a valid type: " + name);
            }
        }
        public override XElement[] NodeAsXML()
        {
            throw new NotImplementedException();
            //return new XElement(Name, "TypeValue");
        }

        public override string NodeAsYangString(int identationlevel)
        {
            return string.Format(GetIdentation(identationlevel)+"type {0};", Name);
        }

        public override string NodeAsYangString()
        {
            return string.Format("type {0};", Name);
        }
    }
}
