using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using hanabi.Controller;

namespace hanabi
{
   
    class Program
    {
        static void Main(string[] args)
        {
            int level = 2;
            Command com = new Command(level);
            string result;

            string scommand = Console.ReadLine();
            while (scommand != null)
            {
                com.DoFunc(scommand);
                result = com.GetResultGame();
                if (result != null)
                {
                    Console.WriteLine(result);
                }
                scommand = Console.ReadLine();
            }
        }
    }
}
