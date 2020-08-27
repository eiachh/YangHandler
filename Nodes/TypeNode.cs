﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using YangHandler.Nodes.Property;

namespace YangHandlerTool
{
    public class TypeNode : YangNode
    {
        private YangNode parent;
        public override YangNode Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                YangTypes.BindType(parent.Name, this);
            }
        }

        public Range Range { get; set; }
        public TypeNode(string name) : base(name)
        {
            if(!YangTypes.IsValidType(name))
            {
                throw new ArgumentException("The given type for TypeNode is not a valid type: "+name);
            }
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
