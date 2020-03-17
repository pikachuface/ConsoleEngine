using System;
using System.Threading;
using ConsoleEngine;

namespace SnakeConsole
{
    class Program
    {
        bool gameOver = false;
        



        static void Main(string[] args)
        {
            Engine.Init(20, 20,2);
            Console.ReadKey();
            Snake snake = new Snake();
        }
    }
}
