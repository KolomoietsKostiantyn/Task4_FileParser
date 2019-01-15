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
        public Controller(string[] arr, IUI visualizator)
        {

            _resultInformer += visualizator.AnswerCatcher;
            _arr = arr;
            isValidated = Validation(arr);
        }

        private bool Validation(string[] arr)
        {
            bool result = true;
            if (arr.Length == 0)
            {
                result = false;
                if (_resultInformer != null)
                {

                    _resultInformer(WorkResult.NeedInstruction);
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
                if (_resultInformer != null)
                {
                    _resultInformer(result, count);
                }
            }
        }




        private bool isValidated = true;
        private ResultInformer _resultInformer;
        private string[] _arr;
        Parser _txtFileParser;


        



    }
}
