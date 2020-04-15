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
            Engine.Init(20,20,4,ConsoleColor.Red);
            Snake snake = new Snake(new Position(1, 1), ConsoleColor.Blue);
            Snake test = new Snake(new Position(4, 4), ConsoleColor.Green);
            Console.ReadKey();
            snake.MoveTo(3, 1);
            test.MoveTo(4, 1);
            Console.ReadKey();
            snake.MoveTo(5, 1);
            test.MoveTo(8, 1);
            Console.ReadKey();
            
        }


    }
}
