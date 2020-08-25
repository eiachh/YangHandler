using System;
using System.Collections.Generic;
using System.Text;

namespace YangHandler.Interpreter
{
    [Serializable()]
    public class OverflownContainer : System.Exception
    {
        public OverflownContainer() : base() { }
        public OverflownContainer(string message) : base(message) { }
        public OverflownContainer(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected OverflownContainer(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
