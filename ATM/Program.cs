using System;
using ATM_PresentationLayer;
namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            ATM_PL PresentationLayer = new ATM_PL();
            PresentationLayer.getLogin();
        }
    }
}
