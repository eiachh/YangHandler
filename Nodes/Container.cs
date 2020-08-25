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
            throw new NotImplementedException();
        }
    }
}
