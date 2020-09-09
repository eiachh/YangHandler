using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using YangHandlerTool;

namespace YangHandler.Nodes
{
    class ChoiceCase : ContainerCapability
    {
        public ChoiceCase(string name) : base(name) { }

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
