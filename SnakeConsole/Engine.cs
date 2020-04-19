using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ConsoleEngine
{
    static class Engine
    {
        /// <summary>
        /// Gives you the value for the map border thicknes
        /// </summary>
        static public int BorderThicknes { get; private set; }

        /// <summary>
        /// Returns the map width.
        /// </summary>
        static public int mapWidth { get; private set; }
        /// <summary>
        /// Returns the map height.
        /// </summary>
        static public int mapHeight { get; private set; }

        //Colors
        /// <summary>
        /// Color of the border.
        /// </summary>
        static public ConsoleColor BorderColor = ConsoleColor.White;
        /// <summary>
        /// Deafault color which is used in blank spaces.
        /// </summary>
        static public ConsoleColor defaultColor = ConsoleColor.Black;


        static private List<Action> ToBeRendered = new List<Action>();
        static public Task rendering { get; private set; }
        static public Task keyRegistering { get; private set; }
        static public bool isRendering { get; private set; }
        static private bool RenderLock = false;



        #region Init

        /// <summary>
        /// This method will start Engine
        /// </summary>
        /// <param name="_mapWidth">Sets width of the map</param>
        /// <param name="_mapHeight">Sets heigh of the map</param>
        static public void Init(int _mapWidth, int _mapHeight)
        {
            BorderThicknes = 1;
            BaseInit(_mapWidth, _mapHeight);
        }


        /// <summary>
        /// This method will start Engine.
        /// </summary>
        /// <param name="_mapWidth">Sets width of the map.</param>
        /// <param name="_mapHeight">Sets heigh of the map.</param>
        /// <param name="_borderThicknes">Sets thicknes of the border around the map.</param>
        static public void Init(int _mapWidth, int _mapHeight, int _borderThicknes)
        {
            BorderThicknes = _borderThicknes;
            BaseInit(_mapWidth, _mapHeight);
        }


        /// <summary>
        /// This method will start Engine.
        /// </summary>
        /// <param name="_mapWidth">Sets width of the map.</param>
        /// <param name="_mapHeight">Sets heigh of the map.</param>
        /// <param name="title">Sets the label of the console app.</param>
        static public void Init(int _mapWidth, int _mapHeight, string title)
        {
            BorderThicknes = 1;
            Console.Title = title;
            BaseInit(_mapWidth, _mapHeight);
        }


        /// <summary>
        /// This method will start Engine.
        /// </summary>
        /// <param name="_mapWidth">Sets width of the map.</param>
        /// <param name="_mapHeight">Sets heigh of the map.</param>
        /// <param name="_borderThicknes">Sets thicknes of the border around the map.</param>
        /// <param name="title">Sets the label of the console app.</param>
        static public void Init(int _mapWidth, int _mapHeight, int _borderThicknes, string title)
        {
            BorderThicknes = _borderThicknes;
            Console.Title = title;
            BaseInit(_mapWidth, _mapHeight);
        }

        /// <summary>
        /// This method will start Engine.
        /// </summary>
        /// <param name="_mapWidth">Sets width of the map.</param>
        /// <param name="_mapHeight">Sets heigh of the map.</param>
        /// <param name="_borderThicknes">Sets thicknes of the border around the map.</param>
        /// <param name="_borderColor">Sets the color of the map border.</param>
        /// <param name="title">Sets the label of the console app.</param>
        static public void Init(int _mapWidth, int _mapHeight, int _borderThicknes, ConsoleColor _borderColor, string title)
        {
            BorderThicknes = _borderThicknes;
            Console.Title = title;
            BorderColor = _borderColor;
            BaseInit(_mapWidth, _mapHeight);
        }

        /// <summary>
        /// This method will start Engine.
        /// </summary>
        /// <param name="_mapWidth">Sets width of the map.</param>
        /// <param name="_mapHeight">Sets heigh of the map.</param>
        /// <param name="_borderThicknes">Sets thicknes of the border around the map.</param>
        /// <param name="_borderColor">Sets the color of the map border.</param>
        /// <param name="title">Sets the label of the console app.</param>
        static public void Init(int _mapWidth, int _mapHeight, int _borderThicknes, ConsoleColor _borderColor)
        {
            BorderThicknes = _borderThicknes;
            BorderColor = _borderColor;
            BaseInit(_mapWidth, _mapHeight);
        }

        /// <summary>
        /// This method will start Engine.
        /// </summary>
        /// <param name="_mapWidth">Sets width of the map.</param>
        /// <param name="_mapHeight">Sets heigh of the map.</param>
        /// <param name="_borderColor">Sets the color of the map border.</param>
        static public void Init(int _mapWidth, int _mapHeight, ConsoleColor _borderColor)
        {
            BorderThicknes = 1;
            BorderColor = _borderColor;
            BaseInit(_mapWidth, _mapHeight);
        }


        /// <summary>
        /// Base method for the <c>Engine.Init()</c> method.
        /// </summary>
        /// <param name="_mapWidth">Sets width of the map.</param>
        /// <param name="_mapHeight">Sets heigh of the map.</param>
        static private void BaseInit(int _mapWidth, int _mapHeight)
        {
            mapHeight = _mapHeight;
            mapWidth = _mapWidth;
            Console.CursorVisible = false;
            if (Environment.OSVersion.Platform != PlatformID.Unix)
            {
                Console.SetWindowSize(2 * (_mapWidth + (2 * BorderThicknes)) + 2, _mapHeight + (2 * BorderThicknes) + 1);
                Console.SetBufferSize(2 * (_mapWidth + (2 * BorderThicknes)) + 3, _mapHeight + (2 * BorderThicknes) + 2);
            }
            DrawBorder();
            rendering = Task.Run(Render);
            isRendering = true;
        }

        #endregion


        /// <summary>
        /// Draws the border of the map.
        /// <br></br>
        /// <example>
        /// This method is called in the <c>Engine.Init()</c> method.
        /// </example>
        /// </summary>
        static void DrawBorder()
        {
            SetColor(BorderColor);
            for (int y = 0; y < mapHeight + (2 * BorderThicknes); y++)
            {
                for (int x = 0; x < mapWidth + (2 * BorderThicknes); x++)
                {
                    if (y < BorderThicknes || y >= mapHeight + BorderThicknes || x < BorderThicknes || x >= mapWidth + BorderThicknes)
                    {
                        Console.SetCursorPosition(2 * x, y);
                        Console.Write("00");
                    }
                }
            }
            ResetColor();
        }


        /// <summary>
        /// Sets the color for the new output.
        /// </summary>
        /// <param name="color"></param>
        static public void SetColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.BackgroundColor = color;
        }


        /// <summary>
        /// Restart the color of the new output to the default color.
        /// <example>
        /// Which is found in: <c>Engine.defaultColor()</c>
        /// </example>
        /// </summary>
        static public void ResetColor()
        {
            Console.ForegroundColor = defaultColor;
            Console.BackgroundColor = defaultColor;
        }


        /// <summary>
        /// Stops rendering the application.
        /// </summary>
        /// <param name="flushBuffer">If <c>true</c> the Engine will clear the screenbuffer</param>
        static public void StopRendering(bool flushBuffer = false)
        {
            if (isRendering)
            {
                isRendering = false;
                rendering.Wait();
                if (flushBuffer)
                {
                    Console.Clear();
                }
            }
        }



        /// <summary>
        /// Flushes the whole sceen buffer and starts rendering.
        /// </summary>
        static public void StartRendering()
        {
            if (!isRendering)
            {
                isRendering = true;
                Console.Clear();
                DrawBorder();
                rendering = Task.Run(Render);
            }
        }

        static private void Render()
        {
            if (!RenderLock)
            {
                RenderLock = true;
                while (isRendering)
                {
                    foreach (var gameObjectRender in ToBeRendered)
                    {
                        gameObjectRender.Invoke();
                    }
                }
                RenderLock = false;
            }
        }
        static internal void AddRenderMethod(Action _render)
        {
            ToBeRendered.Add(_render);
        }

        static internal void DeleteRenderMethod(Action _render)
        {
            for (int i = 0; i < ToBeRendered.Count; i++)
            {
                if (_render == ToBeRendered[i])
                {
                    ToBeRendered.RemoveAt(i);
                }
            }
        }

    }
}
