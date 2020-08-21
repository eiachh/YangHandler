using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace YangHandlerTool
{
    
    public abstract class YangNode 
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public YangNode Parent { get; set; }
        public abstract XElement NodeAsXML();

        /// <summary>
        /// This is here to force YangNode constructor with Name parameter.
        /// </summary>
        private YangNode() { }
        public YangNode(string name) { Name = name; }
    }
}
