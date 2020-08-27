using System;
using System.Collections.Generic;
using System.Text;

namespace YangHandler.Nodes.Property
{
    public class Description : YangPropertyBase
    {
        public Description() : base("Description") { }
        public Description(string _Value) : base("Description")
        {
            Value = _Value;
        }
        public override string GetValue()
        {
            return Value;
        }

        public override void SetValue(string _Value)
        {
            Value = _Value;
        }
    }
}
