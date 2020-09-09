using System;
using System.Collections.Generic;
using System.Text;
using YangHandlerTool;
using System.Xml.Linq;
using YangHandler.Interpreter;

namespace YangHandler.Nodes
{
    public class Choices : ContainerCapability
    { 
        public Choices(string name) : base(name){ }
        public override void AddChild(YangNode cases)
        {
            if (cases.GetType() != typeof(ChoiceCase))
            {
                throw new TypeMissmatch("You are trying to set a YangNode as child for Choices, which is not a Choicecase. Make sure that choices only contain case node!");
            }
            else
            {
                base.AddChild(cases);
            }
        }

        public override string NodeAsYangString(int identationlevel)
        {
            throw new NotImplementedException();
        }

        public override string NodeAsYangString()
        {
            throw new NotImplementedException();
        }

        public XElement[] NodeAsXmlForUses()
        {
            List<XElement> retchildlist = new List<XElement>();
            foreach (var child in Children)
            {
                retchildlist.AddRange(child.NodeAsXML());
            }
            return retchildlist.ToArray();
        }
    }
}
