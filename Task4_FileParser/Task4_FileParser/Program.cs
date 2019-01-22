using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task4_FileParser.Intermediate;
using Task4_FileParser.Logick;
using Task4_FileParser.UI;

namespace Task4_FileParser
{
    class Program
    {
        static void Main(string[] args)
        {

            IVisualizer visualizator = new ConsoleInformer();
            Controller cntrl = new Controller(args, visualizator);
            cntrl.Start();//.
        }
    }
}
