using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace YangHandlerTool
{
    /// <summary>
    /// This class contains the regex for the interpreter of how to read the file and other informations about the regex.
    /// </summary>
    public class SearchScheme
    {
        /// <summary>
        /// The regex that will match the selected TokenType.
        /// </summary>
        public Regex Reg { get; set; }
        /// <summary>
        /// The TokenType gives information of what row the regex will match.
        /// </summary>
        public TokenTypes TokenType { get; set; }
        /// <summary>
        /// Contains the index of the regex match grouping which will return the desired value. IMPORTANT -1 if the regex doesn't contain a grouping for the desired value.
        /// </summary>
        public int IndexOfTokenName { get; set; }
        /// <summary>
        /// Contains the index of the regex match grouping which will return the desired value. IMPORTANT -1 if the regex doesn't contain a grouping for the desired value.
        /// </summary>
        public int IndexOfTokenValue { get; set; }

        /// <summary>
        /// This links to the single line version of this token which is used after solving multiline tokens.
        /// </summary>
        public TokenTypes TokenAsSingleLine { get; set; }
        public SearchScheme(Regex _Reg, TokenTypes _TokenType,int _IndexOfTokenName, int _IndexOfTokenValue)
        {
            Reg = _Reg;
            TokenType = _TokenType;
            IndexOfTokenName = _IndexOfTokenName;
            IndexOfTokenValue = _IndexOfTokenValue;
        }
        public SearchScheme(Regex _Reg, TokenTypes _TokenType, int _IndexOfTokenName, int _IndexOfTokenValue, TokenTypes _TokenAsSingleLine) : this(_Reg, _TokenType, _IndexOfTokenName, _IndexOfTokenValue)
        {
            TokenAsSingleLine = _TokenAsSingleLine;
        }
    }
}
