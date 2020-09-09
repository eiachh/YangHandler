using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using YangHandler.Nodes.Property;
using YangHandler.Nodes;

namespace YangHandlerTool
{
    public enum YangAddingOption
    {
        ChildIncapable
    }
    public abstract class YangNode 
    {
        public string Name { get; set; }
        public SimpleTypeNode Type { get; set; }
        public Description Description { get; set; }
        public virtual YangNode Parent { get; set; }
        public abstract XElement[] NodeAsXML();
        public abstract string NodeAsYangString(int identationlevel);
        public abstract string NodeAsYangString();

        /// <summary>
        /// This is here to force YangNode constructor with Name parameter.
        /// </summary>
        private YangNode() { }
        public YangNode(string name) { Name = name; }



        protected string GetIdentation(int n)
        {
            return new String('\t', n);
        }
    }
}
