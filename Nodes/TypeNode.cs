using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using YangHandler.Nodes.Property;
using YangHandler.Nodes;
using YangHandler.Interpreter;

namespace YangHandlerTool
{
    public class TypeNode : ContainerCapability
    {
        private YangNode parent;
        public Range Range { get; set; }

        public override YangNode Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                var tmpTypedefAsParent = parent;
                while (tmpTypedefAsParent != null && tmpTypedefAsParent.GetType() != typeof(Typedef))
                {
                    tmpTypedefAsParent = tmpTypedefAsParent.Parent;
                }
                YangTypes.BindType(tmpTypedefAsParent.Name, this);
            }
        }

        public TypeNode(string name) : base(name)
        {
            if(!YangTypes.IsValidType(name))
            {
                throw new ArgumentException("The given type for TypeNode is not a valid type: "+name);
            }
        }

        public override void AddChild(YangNode Node)
        {
            //Types can strictly contain other types only as children. 
            if (Node.GetType() == typeof(TypeNode) || Node.GetType() == typeof(YangEnum))
            {
                throw new TypeMissmatch("TypeNode can strictly contain other (TypeNodes or Enums) only as children.");
            }
            base.AddChild(Node);
        }

        public override XElement[] NodeAsXML()
        {
            throw new NotImplementedException();
        }

        public override string NodeAsYangString()
        {
            if (Range == null)
            {
                return string.Format("type {0};",Name);
            }
            return "type {0} {\r\n" + Range.PropertyAsYangText(1) + "\r\n}";
        }
        public override string NodeAsYangString(int identationlevel)
        {
            if (Range == null)
            {
                return string.Format(GetIdentation(identationlevel) + "type {0};", Name);
            }
            return GetIdentation(identationlevel) + "type {0} {\r\n" +
                   GetIdentation(identationlevel) + Range.PropertyAsYangText(identationlevel+1) + "\r\n" +
                   GetIdentation(identationlevel) + "}";
        }
    }
}
