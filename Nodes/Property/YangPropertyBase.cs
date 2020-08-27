using System;
using System.Collections.Generic;
using System.Text;

namespace YangHandler.Nodes.Property
{
    public abstract class YangPropertyBase
    {
        protected string Name;
        protected string Value;

        public YangPropertyBase() { }
        protected YangPropertyBase(string _Name) { Name = _Name; }

        public abstract string GetValue();
        public abstract void SetValue(string _Value);
        public string PropertyAsYangText()
        {
            return Name + "\"" + Value + "\"" + ";";
        }
        public string PropertyAsYangText(int identationlevel)
        {
            return GetIdentation(identationlevel) + Name + "\"" + Value + "\"" + ";";
        }

        protected string GetIdentation(int n)
        {
            return new String('\t', n);
        }
    }
}
