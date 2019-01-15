using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4_FileParser.Logick
{
    interface IResourceProvider : IDisposable
    {
        bool AddResource(string resource);
        bool ReplacementPreparation();
        bool ReadingPreparation();
        void Seek(long offset, SeekOrigin origin);
        bool EndValidation();
        char Read();
        void Write(char sign);
    }
}
