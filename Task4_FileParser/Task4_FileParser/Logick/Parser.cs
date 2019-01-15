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
        private readonly int _maxInnerParan = 3;
        private IResourceProvider _provider;
        private bool _isNededOverwrite = false; // флаг показывающий встречали мы хоть одно вхождение искомой строки
        private Queue<char> _buffer = new Queue<char>(); 
        private int _readerPozition = 0;
        private int _writerPozition = 0;
        private bool isAllFileReaded = false;
        private string _search = null; // искомый текст
        private string _replacement = null; // текст замены
        private readonly int _stepLimit = 1000000; //количество считуемых символов за 1ну проходку
        #endregion

        public Parser(string[] arr)
        {
            _provider = new WorkerWithFileSystem();
            Validation(arr);
            ProcessingInput(arr);
            _provider.AddResource(arr[(int)Dates.Path]);
        }

        private void Validation(string[] arr)
        {
            if (arr == null)
            {
                throw new NullReferenceException();
            }
            if (arr.Length < _minInnerParan || arr.Length > _maxInnerParan)
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

        private void ProcessingInput(string[] arr) //обработка входных данных
        {
            _search = arr[(int)Dates.Serch];
            if (arr.Length == _maxInnerParan)
            {
                _replacement = arr[(int)Dates.Replacement];
            }
        }

        public int Start()
        {
            int result = -1;
            if (_replacement != null) // если поле замены не пустое, то заменяем 
            {
                _provider.ReplacementPreparation();
                StartReplesment();
            }
            else // иначе просто подсчет
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
                if (isAllFileReaded) // весь файл вычитан просто записываем до конца
                {
                    _provider.Seek(_writerPozition, SeekOrigin.Begin);
                    while (_buffer.Count != 0)
                    {
                        _provider.Write(_buffer.Dequeue());
                    }
                }
                else // пишим пока не упремся в _readerPozition
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
                    if (isCoincidence)// есть полное совпадения, просто вместо вхождения заносим нужный
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
            while (!_provider.EndValidation()) 
            {
                if (curentStepValue >= _stepLimit)
                {
                    break;
                }
                if (_isNededOverwrite)
                {
                    curentStepValue++;
                    _readerPozition++;
                    char currentReadingSign = _provider.Read(); //проверка совпадения с искомым
                    if (currentReadingSign == _search[0])
                    {
                        bool isCoincidence = true;
                        int steps = 0;
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
                        if (isCoincidence)// есть полное совпадения, просто вместо вхождения заносим нужный
                        {
                            for (int i = 0; i < _replacement.Length; i++)
                            {
                                _buffer.Enqueue(_replacement[i]);
                            }
                        }
                        else // совпадения не найденно переводим каретку к началу и записываем до n-го символа 
                        {
                            
                            _buffer.Enqueue(currentReadingSign);
                            _provider.Seek(-steps, SeekOrigin.Current);
                            for (int i = 0; i < steps; i++)
                            {
                                char nextSymbol =  _provider.Read();
                                _buffer.Enqueue(nextSymbol);
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
                                _provider.Seek(-1, SeekOrigin.Current);
                                _writerPozition = _readerPozition;
                                isCoincidence = false;
                                break;
                            }
                        }
                        if (isCoincidence) // записуем в буфер
                        {
                            for (int i = 0; i < _replacement.Length; i++)
                            {
                                _buffer.Enqueue(_replacement[i]);
                            }
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
    }
}
