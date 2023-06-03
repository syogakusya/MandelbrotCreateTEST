using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Numerics;

namespace MandelbrotTEST02
{
    
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MandelbrotForm());
        }
       
    }
}