using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using YangHandler.Nodes.Property;
using YangHandler.Nodes;

namespace YangHandlerTool
{
    public class YangHandler
    {
        public string YangAsRawText { get; private set; }
        private static List<SearchScheme> InterpreterSearchSchemeList;
        private static List<TokenTypes> MultilineTokens;
        private static List<TokenTypes> ItemsThatRequireParentFallback;

        private TokenTypes InterpreterStatus = TokenTypes.Start;
        Stack<TokenTypes> InterpreterStatusStack = new Stack<TokenTypes>();
        private List<string> Metadata = new List<string>();
        private YangNode InterpreterTracer;
        private YangNode TracerCurrentNode;

        private Token PreviousToken;

        int LineNumber = 0;

        private static void Init()
        {
            InterpreterSearchSchemeList = new List<SearchScheme>
            {
                //new SearchScheme(new Regex(@"^\s*$"),TokenTypes,-1,-1),
                new SearchScheme(new Regex(@"^\s*module (([a-z0-9A-Z]|-)*) {$",RegexOptions.IgnoreCase),TokenTypes.Module,1,-1),
                new SearchScheme(new Regex(@"^\s*grouping (([a-z0-9A-Z]|-)*) {$"),TokenTypes.Grouping,1,-1),
                new SearchScheme(new Regex(@"^\s*uses (([a-z0-9A-Z]|-)*);$"),TokenTypes.Uses,1,-1),
                new SearchScheme(new Regex(@"^\s*choice (([a-z0-9A-Z]|-)*) {$"),TokenTypes.Choice,1,-1),
                new SearchScheme(new Regex(@"^\s*case (([a-z0-9A-Z]|-)*) {$"),TokenTypes.ChoiceCase,1,-1),
                new SearchScheme(new Regex("^\\s*namespace \"([^\"]*)\";$",RegexOptions.IgnoreCase),TokenTypes.Namespace,-1,1),
                new SearchScheme(new Regex("^\\s*prefix \"([^\"]*)\";$",RegexOptions.IgnoreCase),TokenTypes.Prefix,-1,1),
                new SearchScheme(new Regex("^\\s*organization \"([^\"]*)\";$",RegexOptions.IgnoreCase),TokenTypes.Organization,-1,1),
                new SearchScheme(new Regex("^\\s*contact \"([^\"]*)\";$",RegexOptions.IgnoreCase),TokenTypes.Contact,-1,1),
                new SearchScheme(new Regex(@"^\s*container (([a-z0-9A-Z]|-)*) {$",RegexOptions.IgnoreCase),TokenTypes.Container,1,-1),
                new SearchScheme(new Regex(@"^\s*leaf (([a-z0-9A-Z]|-)*) {$",RegexOptions.IgnoreCase),TokenTypes.Leaf,1,-1),
                new SearchScheme(new Regex(@"^\s*leaf-list (([a-z0-9A-Z]|-)*) {$",RegexOptions.IgnoreCase),TokenTypes.LeafList,1,-1),
                new SearchScheme(new Regex(@"^\s*list (([a-z0-9A-Z]|-)*) {$",RegexOptions.IgnoreCase),TokenTypes.List,1,-1),
                new SearchScheme(new Regex("^\\s*key \"([^\"]*)\";$",RegexOptions.IgnoreCase),TokenTypes.Key,-1,1),
                new SearchScheme(new Regex(@"^\s*type (([a-z0-9A-Z]|-)*);$",RegexOptions.IgnoreCase),TokenTypes.SimpleType,1,-1),
                new SearchScheme(new Regex(@"^\s*type (([a-z0-9A-Z]|-)*) {$",RegexOptions.IgnoreCase),TokenTypes.Type,1,-1),
                new SearchScheme(new Regex(@"^\s*enum (([a-z0-9A-Z]|-)*);$",RegexOptions.IgnoreCase),TokenTypes.SimpleEnum,1,-1),
                new SearchScheme(new Regex(@"^\s*enum (([a-z0-9A-Z]|-)*) {$",RegexOptions.IgnoreCase),TokenTypes.Enum,1,-1),
                new SearchScheme(new Regex(@"^\s*revision (([a-z0-9A-Z]|-)*) {$",RegexOptions.IgnoreCase),TokenTypes.Revision,-1,1),
                new SearchScheme(new Regex("^\\s*description \"([^\"]*)\";$",RegexOptions.IgnoreCase),TokenTypes.Description,-1,1),
                new SearchScheme(new Regex("^\\s*description\\s*$",RegexOptions.IgnoreCase),TokenTypes.DescriptionWithValueNextLine,-1,-1,TokenTypes.Description),
                new SearchScheme(new Regex("^\\s*\"([^\"]*)\";$"),TokenTypes.ValueForPreviousLine,-1,1),
                new SearchScheme(new Regex(@"^\s*config (true|false);$"),TokenTypes.ConfigStatement,-1, 1),
                new SearchScheme(new Regex(@"^\s*typedef (([a-z0-9A-Z]|-)*) {$"),TokenTypes.Typedef,1,-1),
                new SearchScheme(new Regex("^\\s*range \"([0-9]* ?.. ?[0-9]*)\";$"),TokenTypes.Range,-1,1),
                new SearchScheme(new Regex(@"^\s* *$"),TokenTypes.Skip,-1,-1),
                new SearchScheme(new Regex(@"^\s*}$"),TokenTypes.NodeEndingBracket,-1,-1)
            };


            ///Multiline tokens can only be simple tokens where token name is presented in the first line, token value is presented in the upcoming line(s)
            ///For complex Nodes use "List metadata"
            MultilineTokens = new List<TokenTypes>
            {
                TokenTypes.DescriptionWithValueNextLine,
            };

            ItemsThatRequireParentFallback = new List<TokenTypes>
            {
                TokenTypes.Leaf,
                TokenTypes.Container,
                TokenTypes.LeafList,
                TokenTypes.List,
                TokenTypes.Choice,
                TokenTypes.ChoiceCase,
                TokenTypes.Typedef,
                TokenTypes.Type,
                TokenTypes.Grouping,
                TokenTypes.Enum,
            };
        }
        private YangHandler(string InputStr, bool IsPath)
        {
            Init();

            InterpreterStatusStack.Push(TokenTypes.Start);

            if (!IsPath)
            {
                YangAsRawText = InputStr;
            }
            else
            {
                YangAsRawText = File.ReadAllText(InputStr);
            }

            ConvertText(YangAsRawText);

        }

        public static YangHandler Parse(string YangAsRawText)
        {

            return new YangHandler(YangAsRawText, false);
        }

        public static YangHandler Load(string Path)
        {
            return new YangHandler(Path, true);
        }

        public YangNode Root()
        {
            return InterpreterTracer;
        }

        /// <summary>
        /// Converts the raw text into objects that the YangHandlerTool can use easily;
        /// </summary>
        private void ConvertText(string YangAsRawText)
        {
            var YangAsRawTextRowByRow = Regex.Split(YangAsRawText, "\r\n|\r|\n");

            foreach (var RowOfYangText in YangAsRawTextRowByRow)
            {
                LineNumber++;
                var TokenForCurrentRow = GetTokenForRow(RowOfYangText);
                if (TokenForCurrentRow != null)
                {
                    if (MultiLineToken(TokenForCurrentRow))
                    {
                        SetPreviousToken(TokenForCurrentRow);
                        continue;
                    }
                    if (PreviousToken != null)
                    {
                        var prevtoken = GetpreviousToken();
                        prevtoken.TokenValue = TokenForCurrentRow.TokenValue;
                        prevtoken.TokenType = prevtoken.TokenAsSingleLine;
                        TokenForCurrentRow = prevtoken;
                    }
                    ProcessToken(TokenForCurrentRow, Metadata);
                }
            }
        }

        private Token GetTokenForRow(string row)
        {
            foreach (var scheme in InterpreterSearchSchemeList)
            {
                Match match = scheme.Reg.Match(row);
                if (match.Success)
                {
                    Token MatchResultToken = new Token(scheme.TokenType, "", "", scheme.TokenAsSingleLine);
                    if (scheme.IndexOfTokenName != -1)
                    {
                        MatchResultToken.TokenName = match.Groups[scheme.IndexOfTokenName].Value;
                    }
                    if (scheme.IndexOfTokenValue != -1)
                    {
                        MatchResultToken.TokenValue = match.Groups[scheme.IndexOfTokenValue].Value;
                    }

                    return MatchResultToken;
                }
            }
            InterpretationErrorHandler interpreterr = new InterpretationErrorHandler("unrecognisable line.",
                                                                                         "The interpreter could not convert the following line into any valid token:\r\n"+row.Trim(),
                                                                                         LineNumber,
                                                                                         InterpreterTracer);
            return null;
        }

        private void CreateMetadata(List<string> input)
        {
            Metadata.Clear();
            Metadata.AddRange(input);
        }
        private Module CreateMainModule(string name)
        {
            return new Module(name);
        }

        private bool MultiLineToken(Token token)
        {
            return MultilineTokens.Contains(token.TokenType);
        }

        private void SetPreviousToken(Token previoustoken)
        {
            PreviousToken = previoustoken;
        }
        private Token GetpreviousToken()
        {
            Token rettoken = PreviousToken;
            PreviousToken = null;
            return rettoken;
        }
        private void ProcessToken(Token InputToken, List<string> metadata)
        {
            ///Reachable Statuses from Start status
            if (InterpreterStatus == TokenTypes.Start && InputToken.TokenType == TokenTypes.Module)
            {
                InterpreterTracer = CreateMainModule(InputToken.TokenName);
                TracerCurrentNode = InterpreterTracer;
                NewInterpreterStatus(TokenTypes.Module);
            }

            ///NODE END 
            if (InputToken.TokenType == TokenTypes.NodeEndingBracket)
            {
                FallbackToPreviousInterpreterStatus();
            }

            ///MODULE
            else if (InterpreterStatus == TokenTypes.Module)
            {
                if (InputToken.TokenType == TokenTypes.Namespace)
                {
                    ///Metadata[0] contains fullnamespace
                    CreateMetadata(new List<string> { InputToken.TokenValue });
                    NewInterpreterStatus(TokenTypes.Namespace);
                }
                else if (InputToken.TokenType == TokenTypes.Revision)
                {
                    NewInterpreterStatus(TokenTypes.Revision);
                    CreateMetadata(new List<string> { InputToken.TokenValue });
                }
                else if (InputToken.TokenType == TokenTypes.Organization)
                {
                    ((Module)InterpreterTracer).Organization = InputToken.TokenValue;
                }
                else if (InputToken.TokenType == TokenTypes.Contact)
                {
                    ((Module)InterpreterTracer).Contact = InputToken.TokenValue;
                }
                else if (InputToken.TokenType == TokenTypes.Organization)
                {
                    ((Module)InterpreterTracer).Organization = InputToken.TokenValue;
                }
                else if (InputToken.TokenType == TokenTypes.Description)
                {
                    ((Module)InterpreterTracer).Description = new Description(InputToken.TokenValue);
                }
                else if (InputToken.TokenType == TokenTypes.Container)
                {
                    AddNewYangNode(typeof(Container), InputToken);
                }
                else if (InputToken.TokenType == TokenTypes.Typedef)
                {
                    AddNewYangNode(typeof(Typedef), InputToken);
                    YangTypes.AddNewYangType(InputToken.TokenName, InputToken.TokenName, LineNumber, InterpreterTracer);
                }
                else if (InputToken.TokenType == TokenTypes.Grouping)
                {
                    AddNewYangNode(typeof(Grouping), InputToken);
                }
                else if (InputToken.TokenType == TokenTypes.Uses)
                {
                    AddNewYangNode(typeof(Uses), InputToken);
                }
            }

            ///CONTAINER
            else if (InterpreterStatus == TokenTypes.Container)
            {
                if (InputToken.TokenType == TokenTypes.Leaf)
                {
                    AddNewYangNode(typeof(Leaf), InputToken);
                }
                else if (InputToken.TokenType == TokenTypes.LeafList)
                {
                    AddNewYangNode(typeof(LeafList), InputToken);
                }
                else if (InputToken.TokenType == TokenTypes.Container)
                {
                    AddNewYangNode(typeof(Container), InputToken);
                }
                else if (InputToken.TokenType == TokenTypes.List)
                {
                    AddNewYangNode(typeof(ListNode), InputToken);
                }
                else if (InputToken.TokenType == TokenTypes.Uses)
                {
                    AddNewYangNode(typeof(Uses), InputToken, YangAddingOption.ChildIncapable);
                }
                else if (InputToken.TokenType == TokenTypes.Choice)
                {
                    AddNewYangNode(typeof(Choices), InputToken);
                }
            }

            ///LEAF
            else if (InterpreterStatus == TokenTypes.Leaf)
            {
                if (InputToken.TokenType == TokenTypes.SimpleType)
                {
                    TracerCurrentNode.Type = new SimpleTypeNode(InputToken.TokenName);
                }
                else if (InputToken.TokenType == TokenTypes.Description)
                {
                    TracerCurrentNode.Description = new Description(InputToken.TokenValue);
                }
            }

            ///LEAF-LIST
            else if (InterpreterStatus == TokenTypes.LeafList)
            {
                if (InputToken.TokenType == TokenTypes.SimpleType)
                {
                    TracerCurrentNode.Type = new SimpleTypeNode(InputToken.TokenName);
                }
                else if (InputToken.TokenType == TokenTypes.Description)
                {
                    TracerCurrentNode.Description = new Description(InputToken.TokenValue);
                }
            }

            ///LIST
            else if (InterpreterStatus == TokenTypes.List)
            {
                if (InputToken.TokenType == TokenTypes.Key)
                {
                    ((ListNode)TracerCurrentNode).Key = InputToken.TokenValue;
                }
                else if (InputToken.TokenType == TokenTypes.Leaf)
                {
                    AddNewYangNode(typeof(Leaf), InputToken);
                }
            }

            ///Typedef
            else if (InterpreterStatus == TokenTypes.Typedef)
            {
                if (InputToken.TokenType == TokenTypes.Type)
                {
                    AddNewYangNode(typeof(TypeNode), InputToken);
                    CreateMetadata(new List<string> { InputToken.TokenName });
                }
                else if (InputToken.TokenType == TokenTypes.Description)
                {
                    Description desc = new Description(InputToken.TokenValue);
                }
            }
            ///TYPE
            else if (InterpreterStatus == TokenTypes.Type)
            {
                if (InputToken.TokenType == TokenTypes.Range)
                {
                    Range rangeprop = new Range();
                    rangeprop.SetValue(InputToken.TokenValue);
                    ((TypeNode)TracerCurrentNode).Range = rangeprop;
                }
                else if (InputToken.TokenType == TokenTypes.Description)
                {
                    Description desc = new Description(InputToken.TokenValue);
                }
                else if (InputToken.TokenType == TokenTypes.SimpleEnum)
                {
                    AddNewYangNode(typeof(YangEnum), InputToken,YangAddingOption.ChildIncapable);

                    //((ContainerCapability)TracerCurrentNode).AddChild(new YangEnum(InputToken.TokenName));
                }
                else if (InputToken.TokenType == TokenTypes.Enum)
                {
                    AddNewYangNode(typeof(YangEnum), InputToken);
                }
            }

            ///GROUPING
            else if (InterpreterStatus == TokenTypes.Grouping)
            {
                if (InputToken.TokenType == TokenTypes.Leaf)
                {
                    AddNewYangNode(typeof(Leaf), InputToken);
                }
                else if (InputToken.TokenType == TokenTypes.LeafList)
                {
                    AddNewYangNode(typeof(LeafList), InputToken);
                }
                else if (InputToken.TokenType == TokenTypes.Container)
                {
                    AddNewYangNode(typeof(Container), InputToken);
                }
                else if (InputToken.TokenType == TokenTypes.List)
                {
                    AddNewYangNode(typeof(ListNode), InputToken);
                }
            }


            ///CHOICES
            else if (InterpreterStatus == TokenTypes.Choice)
            {
                if (InputToken.TokenType == TokenTypes.ChoiceCase)
                {
                    AddNewYangNode(typeof(ChoiceCase), InputToken);
                }
            }

            ///CHOICE-CASE
            else if (InterpreterStatus == TokenTypes.ChoiceCase)
            {
                if (InputToken.TokenType == TokenTypes.Leaf)
                {
                    AddNewYangNode(typeof(Leaf), InputToken);
                }
                else if (InputToken.TokenType == TokenTypes.LeafList)
                {
                    AddNewYangNode(typeof(LeafList), InputToken);
                }
                else if (InputToken.TokenType == TokenTypes.Container)
                {
                    AddNewYangNode(typeof(Container), InputToken);
                }
                else if (InputToken.TokenType == TokenTypes.List)
                {
                    AddNewYangNode(typeof(ListNode), InputToken);
                }
                else if (InputToken.TokenType == TokenTypes.Uses)
                {
                    AddNewYangNode(typeof(Uses), InputToken, YangAddingOption.ChildIncapable);
                }
                else if (InputToken.TokenType == TokenTypes.Choice)
                {
                    AddNewYangNode(typeof(Choices), InputToken);
                }
            }

            ///NAMESPACE
            else if (InterpreterStatus == TokenTypes.Namespace && InputToken.TokenType == TokenTypes.Prefix)
            {
                ///Metadata[0] contains fullnamespace
                ((Module)InterpreterTracer).AddNamespace(InputToken.TokenValue, Metadata[0]);
                FallbackToPreviousInterpreterStatus();
            }

            ///REVISION
            else if (InterpreterStatus == TokenTypes.Revision && InputToken.TokenType == TokenTypes.Description)
            {
                Revision rev = new Revision(Metadata[0]);
                rev.Description = new Description(InputToken.TokenValue);
                ((ContainerCapability)TracerCurrentNode).AddChild(rev);
            }

            ///No search scheme match => Incorrect inputline;
            else
            {
                InterpretationErrorHandler interpreterr = new InterpretationErrorHandler("Invalid value in node",
                                                                                        "Interpreter read: " + InputToken.TokenType + " which is not possible in current interpreter status: " + InterpreterStatus,
                                                                                        LineNumber,
                                                                                        InterpreterTracer);
            }
        }

        private TokenTypes FallbackToPreviousInterpreterStatus()
        {
            if (ParentFallbackIsNeeded(InterpreterStatus))
            {
                TracerCurrentNode = TracerCurrentNode.Parent;
            }
            //First pop the current TokenType
            var stackpop = InterpreterStatusStack.Pop();
            if (InterpreterStatus == stackpop)
            {
                InterpreterStatus = InterpreterStatusStack.Pop();
                InterpreterStatusStack.Push(InterpreterStatus);
            }

            return InterpreterStatus;
        }

        /// <summary>
        /// Returns true if an object called fallback that requires fallback to parent when exiting the node.
        /// </summary>
        /// <param name="caller"></param>
        /// <returns></returns>
        private bool ParentFallbackIsNeeded(TokenTypes currenttokentype)
        {
            bool rettypeof = false;
            foreach (var tokentype in ItemsThatRequireParentFallback)
            {
                if (currenttokentype == tokentype)
                {
                    rettypeof = true;
                }
            }
            return rettypeof;
        }
        private void NewInterpreterStatus(TokenTypes status)
        {
            InterpreterStatusStack.Push(status);
            InterpreterStatus = status;
        }

        private void AddNewYangNode(Type type, Token InputToken)
        {
            var instantiatedobj = (YangNode)Activator.CreateInstance(type, InputToken.TokenName);
            ((ContainerCapability)TracerCurrentNode).AddChild(instantiatedobj);
            TracerCurrentNode = instantiatedobj;
            NewInterpreterStatus(InputToken.TokenType);
        }
        private void AddNewYangNode(Type type, Token InputToken,YangAddingOption opt)
        {
            var instantiatedobj = (YangNode)Activator.CreateInstance(type, InputToken.TokenName);
            ((ContainerCapability)TracerCurrentNode).AddChild(instantiatedobj);
            if (opt != YangAddingOption.ChildIncapable)
            {
                TracerCurrentNode = instantiatedobj;
                NewInterpreterStatus(InputToken.TokenType);
            }
            
        }
    }

}
