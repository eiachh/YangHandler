using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace YangHandlerTool
{
    public class Container : ContainerCapability
    {
        public Container(string name) : base(name) { }

        public override string NodeAsYangString()
        {
            string retval = string.Format("container {0} {{\r\n", Name);
            foreach (var child in Children)
            {
                retval += child.NodeAsYangString(1)+"\r\n";
            }
            retval += "}";
            return retval;
        }

        public override string NodeAsYangString(int identationlevel)
        {
            throw new NotImplementedException();
        }
    }
}
