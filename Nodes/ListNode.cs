using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace YangHandlerTool
{
    public class ListNode : ContainerCapability
    {
        /// <summary>
        /// The key property tells which Leaf node(s) of this list is(are) the key(s).
        /// </summary>
        public string Key { get; set; }
        public ListNode(string name) : base(name) { }

        public override string NodeAsYangString()
        {
            throw new NotImplementedException();
        }
    }
}
