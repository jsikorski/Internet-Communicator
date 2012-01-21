using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Hash;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Out.WriteLine("Sikorski & Stoiński instant messaging");
            Console.Out.WriteLine("v0.1 alpha");
            Console.Out.WriteLine("");
            var server = new Server();
        }
    }
}
