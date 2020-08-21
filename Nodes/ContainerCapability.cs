using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace YangHandlerTool
{
    public class ContainerCapability : YangNode
    {
        protected List<YangNode> Children = new List<YangNode>();
        public ContainerCapability(string name) : base(name) { }
        public void AddChild(YangNode Node)
        {
            Children.Add(Node);
            Node.Parent = this;
        }

        /// <summary>
        /// Returns the first child element.
        /// </summary>
        public YangNode Element()
        {
            if (Children.Count > 0)
            {
                return Children[0];
            }
            else
            {
                return null;
            }
        }

         public override XElement NodeAsXML()
        {
            XElement containerasroot = new XElement(this.Name);

            foreach (var child in Children)
            {
                if (child.NodeAsXML() != null)
                {
                    containerasroot.Add(child.NodeAsXML());
                }
            }

            return containerasroot;
        }
    }
}
