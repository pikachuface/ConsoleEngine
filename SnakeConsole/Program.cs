using System;
using System.Threading;
using ConsoleEngine;
using System.Collections.Generic;

namespace SnakeConsole
{
    class Program
    {
        static bool gameOver = false;

        static void Main(string[] args)
        {
            Engine.Init(20,20,2,ConsoleColor.Red);
            Console.ReadKey();
            Console.ReadKey();
        }


    }
}
