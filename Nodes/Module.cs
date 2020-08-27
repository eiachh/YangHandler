using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace YangHandlerTool
{
    public class Module : ContainerCapability
    {
        public string Organization { get; set; }
        public string Contact { get; set; }

        public Module(string name) : base(name) { }

        /// <summary>
        /// Dictionary for namespaces. Keys are the prefixes, values are the full namespace.
        /// </summary>
        private Dictionary<string, string> namespacedictionary = new Dictionary<string, string>();
        public bool AddNamespace(string _prefix, string _namespace)
        {
            return namespacedictionary.TryAdd(_prefix, _namespace);
        } 

        /// <summary>
        /// Returns full namespace based on prefix. If not found "Namespace not found" returned.
        /// </summary>
        /// <param name="_prefix"></param>
        /// <returns></returns>
        public string GetNamespace(string _prefix)
        {
            string outvalue = "Namespace not found";
            namespacedictionary.TryGetValue(_prefix, out outvalue);
            return outvalue;
        }

        public override string NodeAsYangString()
        {
            throw new NotImplementedException();
        }

        public override string NodeAsYangString(int identationlevel)
        {
            throw new NotImplementedException();
        }
    }
}
