using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task4_FileParser.Intermediate;

namespace Task4_FileParser.UI
{
    class ConsoleInformer : IUI
    {
        public ConsoleInformer()
        {

        }


        public void AnswerCatcher(WorkResult masage, int count)
        {
            string result = "";
            switch (masage)
            {
                case WorkResult.OK:
                    if (count >= 0)
                    {
                        result = string.Format("Found {0} occurrences of a given line", count);
                    }
                    else
                    {
                        result = "Done!!!";
                    }
                    break;
                case WorkResult.NullReferenceException:
                    result = "Transferred array or its elements have null references.";
                    break;
                case WorkResult.ArgumentException:
                    result = "Too many or few values passed.";
                    break;
                case WorkResult.PathTooLongException:
                    result = "The specified path, file name, or both exceed the maximum length.";
                    break;
                case WorkResult.DirectoryNotFoundException:
                    result = "The specified path is invalid";
                    break;
                case WorkResult.UnauthorizedAccessException:
                    result = "The path parameter specifies a read-only file.";
                    break;
                case WorkResult.FileNotFoundException:
                    result = "The file specified by the path parameter was not found.";
                    break;
                case WorkResult.NeedInstruction:
                    result = "Run the program with parameters:\n1.<file path> <line to count>" 
                        + "- to count the number of occurrences\n2. < file path > < search "
                        + "string> < replacement string> - to replace all occurrences";
                    break;
            }
            Console.WriteLine(result);
            Console.Read();
        }
    }
}
