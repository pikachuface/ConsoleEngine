using System;
using System.Collections.Generic;


namespace ConsoleEngine
{

    abstract class GameObject
    {
        /// <summary>
        /// Current position of the GameObject.
        /// <br>If you want to change the position use <c>GameObject.MoveTo()</c> or <c>GameObject.MoveBy()</c> methods.</br>
        /// </summary>
        public Position Pos { get; private set; }
        private Position lastPos;
        /// <summary>
        /// Current color of the gameObject.
        /// <br>If you want to change the color use the <c>GameObject.ChangeColor()</c> method.</br>
        /// </summary>
        public ConsoleColor color { get; private set; }
        private bool CheckPosOverflow = false;



        #region ctor
        /// <summary>
        /// Creates new GameObject;
        /// </summary>
        /// <param name="color">Determines the color of the GameObject.</param>
        /// <param name="position">Determines the postion of the GameObject.</param>
        /// <param name="checkPosOverflow">If the gameOject will move beyond the map borders 
        /// <br>it will be teleported back on the other side of the map</br></param>
        public GameObject(ConsoleColor color, Position position, bool checkPosOverflow = false)
        {
            this.CheckPosOverflow = checkPosOverflow;
            this.color = color;
            this.Pos = position;
        }


        /// <summary>
        /// Creates new GameObject.
        /// </summary>
        /// <param name="position">Determines the postion of the GameObject.</param>
        /// /// <param name="checkPosOverflow">If the gameOject will move beyond the map borders 
        /// <br>it will be teleported back on the other side of the map</br></param>
        public GameObject(Position position, bool checkPosOverflow = false)
        {
            this.CheckPosOverflow = checkPosOverflow;
            this.color = Engine.BorderColor;
            this.Pos = position;
        }


        /// <summary>
        /// Creates new GameObject
        /// </summary>
        /// <param name="checkPosOverflow">If the gameOject will move beyond the map borders 
        /// <br>it will be teleported back on the other side of the map</br></param>
        public GameObject(bool checkPosOverflow = false)
        {
            this.CheckPosOverflow = checkPosOverflow;
            this.color = Engine.BorderColor;
            this.Pos = new Position(0, 0);
        }
        #endregion



        #region Movement
        /// <summary>
        /// Moves GameObject to specific position.
        /// </summary>
        /// <param name="X">New position on the X axis</param>
        /// <param name="Y">New position on the Y axis</param>
        public void MoveTo(int X, int Y)
        {
            saveLastPos();
            Pos = new Position(X, Y);
            CorrectPositionOverflow();
        }


        /// <summary>
        /// Moves GameObject to specific position.
        /// </summary>
        /// <param name="newPosition">Position to which will be the GameObject moved.</param>
        public void MoveTo(Position newPosition)
        {
            saveLastPos();
            Pos = newPosition;
            CorrectPositionOverflow();
        }


        /// <summary>
        /// Moves gameObject by specific offset.
        /// </summary>
        /// <param name="offsetX">Offset by which will be teh GameObject moved on the X axis</param>
        /// <param name="offsetY">Offset by which will be teh GameObject moved on the Y axis</param>
        public void MoveBy(int offsetX, int offsetY)
        {
            saveLastPos();
            Pos.X += offsetX;
            Pos.Y += offsetY;
            CorrectPositionOverflow();
        }


        /// <summary>
        /// Saves the last position of the GameObject
        /// </summary>
        private void saveLastPos()
        {
            lastPos = Pos;
        }

        /// <summary>
        /// If GameObject is out of the map it will teleport the GameObject to the other side.
        /// </summary>
        private void CorrectPositionOverflow()
        {
            if (CheckPosOverflow)
            {
                if (Pos.X < Engine.mapWidth) Pos.X = Engine.mapWidth;
                if (Pos.X > Engine.mapWidth) Pos.X = 0;
                if (Pos.Y < Engine.mapWidth) Pos.Y = Engine.mapWidth;
                if (Pos.Y > Engine.mapWidth) Pos.Y = 0;
            }
        }
        #endregion



        /// <summary>
        /// Renders the GameObject.
        /// </summary>
        /// <param name="deleteOldOne">Decides if the old position will be deleted.</param>
        public void Render(bool deleteOldOne = true)
        {
            if (lastPos.Y >= Engine.BorderThicknes && lastPos.Y <= Engine.mapHeight + Engine.BorderThicknes && lastPos.X >= Engine.BorderThicknes && lastPos.Y <= Engine.mapHeight + Engine.BorderThicknes)
            {
                //Deletes old position
                if (deleteOldOne)
                {
                    Console.SetCursorPosition(2 * (Engine.BorderThicknes + lastPos.X), Engine.BorderThicknes + lastPos.Y);

                    Console.BackgroundColor = Engine.defaultColor;
                    Console.ForegroundColor = Engine.defaultColor;

                    Console.Write("  ");
                }

                //Prints new position
                Console.SetCursorPosition(2 * (Engine.BorderThicknes + Pos.X), Engine.BorderThicknes + Pos.Y);
                Console.BackgroundColor = color;
                Console.ForegroundColor = color;

                Console.Write("##");
            }


        }
        /// <summary>
        /// Changes the <c>color</c> of the GameObject.
        /// </summary>
        /// <param name="newColor">Decides the new color.</param>
        public void ChangeColor(ConsoleColor newColor)
        {
            this.color = newColor;
            this.Render(false);
        }

    }


    /// <summary>
    /// This abstarct class can contain multiple GameObjects.
    /// <br>Good for multi cell bodies, like one of an snake.</br>
    /// </summary>
    abstract class GameObjects
    {
        private List<ConsoleColor> BodyColor;
        public List<Position> BodyPosition;


        GameObjects(ConsoleColor color, Position position)
        {


        }

        #region addBodypart
        /// <summary>
        /// Adds body part.
        /// </summary>
        /// <param name="color">Decides the color.</param>
        /// <param name="position">Decides the position.</param>
        public void AddBodyPart(ConsoleColor color, Position position)
        {
            BodyColor.Add(color);
            BodyPosition.Add(position);
        }


        /// <summary>
        /// Adds body part.
        /// </summary>
        /// <param name="color">Decides the color.</param>
        /// <param name="position">Decides the position.</param>
        /// <param name="insertIndex">Decides the position in the list.</param>
        public void AddBodyPart(ConsoleColor color, Position position, int insertIndex)
        {
            BodyColor.Insert(insertIndex, color);
            BodyPosition.Insert(insertIndex, position);
        }
        #endregion

    }
}
