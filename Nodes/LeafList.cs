using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace YangHandlerTool
{
    public class LeafList : Leaf
    {
        public LeafList(string leafname) : base(leafname)
        {

        }
        public LeafList(string leafname, string type) : base(leafname,type)
        {

        }
        public LeafList(string leafname, string type, string description) : base(leafname, type, description)
        {

        }
    }
}
