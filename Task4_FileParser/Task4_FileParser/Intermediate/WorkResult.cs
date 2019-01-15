using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4_FileParser.Intermediate
{
    public enum WorkResult
    {
        OK,
        NullReferenceException,
        ArgumentException,
        PathTooLongException,
        DirectoryNotFoundException,
        UnauthorizedAccessException,
        FileNotFoundException,
        NeedInstruction
    }
}
