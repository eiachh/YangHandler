using System;
using System.Collections.Generic;
using System.Text;

namespace YangHandlerTool
{
    public enum TokenTypes
    {
        Start,
        Skip,
        Leaf,
        LeafList,
        Container,
        List,
        Grouping,
        Uses,
        Choice,
        ChoiceCase,
        SimpleType,
        Type,
        SimpleEnum,
        Enum,
        TypeMultiline,
        TypeWithRange,
        Key,
        Description,
        DescriptionWithValueNextLine,
        ValueForPreviousLine,
        ConfigStatement,
        Typedef,
        Range,
        NodeEndingBracket,
        Module,
        Namespace,
        Prefix,
        Organization,
        Contact,
        Revision,
    }
    public class Token
    {
        public TokenTypes TokenType { get; set; }
        public string TokenName { get; set; }
        public string TokenValue { get; set; }
        public TokenTypes TokenAsSingleLine { get; set; }
        public Token(TokenTypes _TokenType, string _TokenName,string _TokenValue)
        {
            TokenType = _TokenType;
            TokenName = _TokenName;
            TokenValue = _TokenValue;
        }
        public Token(TokenTypes _TokenType, string _TokenName, string _TokenValue,TokenTypes _TokenAsSingleLine) :  this(_TokenType,_TokenName,_TokenValue)
        { 
            TokenAsSingleLine = _TokenAsSingleLine;
        }
    }
}
