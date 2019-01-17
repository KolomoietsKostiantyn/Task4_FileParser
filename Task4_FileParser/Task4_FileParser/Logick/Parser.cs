using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4_FileParser.Logick
{
    class Parser
    {
        #region Variables
        private readonly int _minInnerParan = 2;
        private readonly int _maxInnerParam = 3;
        private IResourceProvider _provider;
        private bool _isNededOverwrite = false; 
        private Queue<char> _buffer = new Queue<char>(); 
        private int _readerPozition = 0;
        private int _writerPozition = 0;
        private bool isAllFileReaded = false;
        private string _search = null; 
        private string _replacement = null; 
        private readonly int _stepLimit = 10000000; 
        #endregion

        public Parser(string[] arr)
        {
            _provider = new WorkerWithFileSystem();
            ValidationArr(arr);
            ProcessingInput(arr);
            _provider.AddResource(arr[(int)Dates.Path]);
        }

        private void ValidationArr(string[] arr)
        {
            if (arr == null)
            {
                throw new NullReferenceException();
            }
            if (arr.Length < _minInnerParan || arr.Length > _maxInnerParam)
            {
                throw new ArgumentException();
            }
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == null)
                {
                    throw new NullReferenceException();
                }
            }
        }

        private void ProcessingInput(string[] arr)
        {
            _search = arr[(int)Dates.Serch];
            if (arr.Length == _maxInnerParam)
            {
                _replacement = arr[(int)Dates.Replacement];
            }
        }

        public int Start()
        {
            int result = -1;
            if (_replacement != null) 
            {
                _provider.ReplacementPreparation();
                StartReplesment();
            }
            else 
            {
                _provider.ReadingPreparation();
                result = CountOccurrences();
            }
            _provider.Dispose();

            return result;
        }

        private void StartReplesment()
        {
            bool result;
            do
            {
                result = Read();
                Write();
            } while (!result);
        }

        private void Write()
        {
            if (_buffer.Count != 0)
            {
                if (isAllFileReaded) 
                {
                    _provider.Seek(_writerPozition, SeekOrigin.Begin);
                    while (_buffer.Count != 0) 
                    {
                        _provider.Write(_buffer.Dequeue());
                    }
                }
                else 
                {
                    _provider.Seek(_writerPozition, SeekOrigin.Begin);
                    while (_writerPozition != _readerPozition)
                    {
                        _writerPozition++;
                        _provider.Write(_buffer.Dequeue());
                    }
                }
            }
        }

        private int CountOccurrences()
        {
            int result = 0;
            while ( !_provider.EndValidation())
            {
                char currentReadingSign = _provider.Read();
                if (currentReadingSign == _search[0])
                {
                    bool isCoincidence = true;
                    for (int i = 1; i < _search.Length; i++)
                    {                   
                        if (_provider.EndValidation())
                        {
                            isCoincidence = false;
                            break;
                        }
                        char nextSymbol = _provider.Read();
                        if (nextSymbol != _search[i])
                        {
                            _provider.Seek(-1, SeekOrigin.Current);
                            isCoincidence = false;
                            break;
                        }
                    }
                    if (isCoincidence)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        private bool Read()
        {
            int curentStepValue = 0;
            while (!_provider.EndValidation() || !(curentStepValue >= _stepLimit)) 
            {
                if (_isNededOverwrite)
                { 
                    curentStepValue++;
                    _readerPozition++;
                    char currentReadingSign = _provider.Read(); 
                    if (currentReadingSign == _search[0])
                    {
                        int steps = 0;                       
                        if (IsRestWordMatchesRepeated(_search, out steps, ref curentStepValue, ref _readerPozition))
                        {
                            AddStringToBuffer(_replacement);
                        }
                        else 
                        {
                            
                            _buffer.Enqueue(currentReadingSign);
                            _provider.Seek(-steps, SeekOrigin.Current);
                            for (int i = 0; i < steps; i++)
                            {
                                _buffer.Enqueue(_provider.Read());
                            }
                        }
                    }
                    else
                    {
                        _buffer.Enqueue(currentReadingSign);
                    }
                }
                else
                {
                    curentStepValue++;
                    _readerPozition++;
                    _writerPozition++;
                    char currentReadingSign = _provider.Read();
                    if (currentReadingSign == _search[0])
                    {
                        if (IsRestWordMatchesFirst(_search, ref _writerPozition, ref curentStepValue, ref _readerPozition)) 
                        {
                            _provider.Seek(-1, SeekOrigin.Current); 
                            AddStringToBuffer(_replacement);
                            _isNededOverwrite = true;
                            _writerPozition--; 
                        }
                    }
                }            
            }
            bool readingFinished = false;
            if (_provider.EndValidation())
            {
                readingFinished = true;
                isAllFileReaded = true;
            }

            return readingFinished;
        }

        private void AddStringToBuffer(string inner)
        {
            if (inner == null)
            {
                throw new NullReferenceException();
            }
            for (int i = 0; i < inner.Length; i++)
            {
                _buffer.Enqueue(inner[i]);
            }
        }

        private bool IsRestWordMatchesFirst(string _search, ref int _writerPozition, ref int curentStepValue, ref int _readerPozition)
        {
            bool isCoincidence = true;
            for (int i = 1; i < _search.Length; i++)
            {
                if (_provider.EndValidation())
                {
                    isCoincidence = false;
                    break;
                }
                _readerPozition++;
                curentStepValue++;
                char nextSymbol = _provider.Read();
                if (nextSymbol != _search[i])
                {
                    _readerPozition--;
                    _writerPozition--;
                    _writerPozition = _readerPozition;
                    isCoincidence = false;
                    break;
                }
            }

            return isCoincidence;
        }

        private bool IsRestWordMatchesRepeated(string _search, out int steps, ref int curentStepValue, ref int _readerPozition)
        {
            bool isCoincidence = true;
            steps = 0;
            for (int i = 1; i < _search.Length; i++)
            {
                if (_provider.EndValidation())
                {
                    isCoincidence = false;
                    break;
                }
                _readerPozition++;
                curentStepValue++;
                steps++;
                char nextSymbol = _provider.Read();
                if (nextSymbol != _search[i])
                {
                    _readerPozition--;
                    isCoincidence = false;
                    break;
                }
            }
            return isCoincidence;
        }
    }
}
