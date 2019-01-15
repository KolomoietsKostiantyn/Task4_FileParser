using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4_FileParser.Logick
{
    class WorkerWithFileSystem : IResourceProvider
    {
        #region Variables
        private string _encoding = "Windows-1251";
        private Stream _stream;
        private BinaryReader _reader;
        private BinaryWriter _writer;
        #endregion

        public WorkerWithFileSystem()
        {

        }
                     
        public bool AddResource(string resource)
        {
            bool result = true;
            try
            {
                _stream = File.Open(resource, FileMode.Open);
            }
            catch (Exception)
            {
                throw;
            }
            _reader = null;
            _writer = null;

            return result;
        }

        public bool ReplacementPreparation()
        {
            bool result = true;
            if (_stream == null)
            {
                result = false;
            }
            if (result && _reader == null)
            {
                _reader = new BinaryReader(_stream, Encoding.GetEncoding(_encoding));
            }
            if (result && _writer == null)
            {
                _writer = new BinaryWriter(_stream, Encoding.GetEncoding(_encoding));
            }

            return result;
        }

        public bool ReadingPreparation()
        {
            bool result = true;
            if (_stream == null)
            {
                result = false;
            }
            if (result && _reader == null)
            {
                _reader = new BinaryReader(_stream, Encoding.GetEncoding(_encoding));
            }
            return result;
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            _stream.Seek(offset, origin);
        }

        public bool EndValidation() // true если там конец
        {
            bool result = false;
            if (_reader.PeekChar() == -1)
            {
                result = true;
            }

            return result;
        }

        public char Read()
        {
            return _reader.ReadChar();
        }

        public void Write(char sign)
        {
            _writer.Write(sign);
        }

        public void Dispose()
        {
            _stream.Close();
            _stream.Dispose();
        }
    }
}
