using System;
using System.Collections.Generic;
using System.Text;
using YangHandlerTool;
using YangHandler.Interpreter;

namespace YangHandler.Nodes
{
    public abstract class SingleItemContainer : ContainerCapability
    {
        protected SingleItemContainer(string name) : base(name) { }
        public override void AddChild(YangNode Node)
        {
            if (Children.Count == 0)
            {
                Children.Add(Node);
                Node.Parent = this;
            }
            else
            {
                throw new OverflownContainer("Container cannot contain more than 1 child. Container overflown.");
            }
        }
        public void SetChild(YangNode Node)
        {
            Children.Clear();
            Children.Add(Node);
        }
        public YangNode GetChild()
        {
            return FirstChild();
        }
    }
}
