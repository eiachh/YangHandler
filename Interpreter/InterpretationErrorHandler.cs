using System;
using System.Collections.Generic;
using System.Text;

namespace YangHandlerTool
{
    class InterpretationErrorHandler
    {
        private string ErrorType;
        private string FullErrorText;
        private ContainerCapability Tracer;
        private int Line;


        public InterpretationErrorHandler(string _ErrorType,string _FullErrorText, int _Line, YangNode _Tracer)
        {
            ErrorType = _ErrorType;
            FullErrorText = _FullErrorText;
            Tracer = (ContainerCapability)_Tracer;


            PrintError();
        }

        private void PrintError()
        {
            Console.WriteLine("--------------------------------------------------------------------\r\n");
            Console.WriteLine("Error Type: {0}. In line: {1}",ErrorType,Line);
            Console.WriteLine("\r\n--------------------------------------------------------------------\r\n");
            Console.WriteLine(FullErrorText);
            Console.WriteLine("\r\n--------------------------------------------------------------------\r\n");

            string TracerAsText = GetTracerAsText();
            Console.WriteLine("Interpreter tracer: ");
            Console.WriteLine(TracerAsText);
        }

        private string GetTracerAsText()
        {
            string TracebackString = string.Empty;

            YangNode currentnode = Tracer;
            bool TracebackFinished = false;

            TracebackString += GetYangNodeAsString(currentnode);
            while (!TracebackFinished)
            {
                if (typeof(ContainerCapability).IsInstanceOfType(currentnode))
                {
                    if (((ContainerCapability)currentnode).LastChild() != null)
                    {
                        currentnode = ((ContainerCapability)currentnode).LastChild();
                        TracebackString += GetYangNodeAsString(currentnode);
                    }
                }
                else
                {
                    TracebackFinished = true;
                }
            }

            return TracebackString+"\r\nLast read node: \r\n"+currentnode.NodeAsYangString();
        }

        private string GetYangNodeAsString(YangNode node)
        {
            return "["+ node.GetType().Name +"]"+node.Name+" //";
        }
    }
}
