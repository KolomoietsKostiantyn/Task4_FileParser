using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task4_FileParser.Intermediate;

namespace Task4_FileParser.Logick
{
    class Controller
    {
        private bool isValidated = true;
        private string[] _arr;
        private Parser _txtFileParser;
        private IVisualizer _visualizator;



        public Controller(string[] arr, IVisualizer visualizator)
        {
            _visualizator = visualizator;
            _arr = arr;
            isValidated = Validation(arr);
        }

        private bool Validation(string[] arr)
        {
            bool result = true;
            if (arr.Length == 0) 
            {
                result = false;
                if (_visualizator != null)
                {
                    _visualizator.AnswerCatcher(WorkResult.NeedInstruction);
                }
            }

            return result;
        }

        public void Start()
        {
            if (isValidated)
            {
                WorkResult result = WorkResult.OK;
                bool isError = false;

                try
                {
                    _txtFileParser = new Parser(_arr);
                }
                catch (NullReferenceException)
                {
                    isError = true;
                    result = WorkResult.NullReferenceException;
                }
                catch (ArgumentException)
                {
                    isError = true;
                    result = WorkResult.ArgumentException;
                }
                catch (PathTooLongException)
                {
                    isError = true;
                    result = WorkResult.PathTooLongException;
                }
                catch (DirectoryNotFoundException)
                {
                    isError = true;
                    result = WorkResult.DirectoryNotFoundException;
                }
                catch (UnauthorizedAccessException)
                {
                    isError = true;
                    result = WorkResult.UnauthorizedAccessException;
                }
                catch (FileNotFoundException)
                {
                    isError = true;
                    result = WorkResult.FileNotFoundException;
                }

                int count = -1;
                if (!isError)
                {
                    count = _txtFileParser.Start();
                }
                if (_visualizator != null)
                {
                    _visualizator.AnswerCatcher(result, count);
                }
            }
        }
    }
}
