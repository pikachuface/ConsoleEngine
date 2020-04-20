using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ConsoleEngine
{

    abstract class GameObject : IDisposable
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
        private bool needsToBeRendered = true;
        public bool Invisible { get; private set; }



        #region ctor
        /// <summary>
        /// Creates new GameObject;
        /// </summary>
        /// <param name="color">Determines the color of the GameObject.</param>
        /// <param name="position">Determines the postion of the GameObject.</param>
        /// <param name="checkPosOverflow">If the gameOject will move beyond the map borders 
        /// <br>it will be teleported back on the other side of the map.</br></param>
        public GameObject(Position position, ConsoleColor color, bool checkPosOverflow = false)
        {
            this.CheckPosOverflow = checkPosOverflow;
            this.color = color;
            this.Pos = position;
            Engine.AddRenderMethod(this.Render);
        }


        /// <summary>
        /// Creates new GameObject.
        /// </summary>
        /// <param name="position">Determines the postion of the GameObject.</param>
        /// /// <param name="checkPosOverflow">If the gameOject will move beyond the map borders 
        /// <br>it will be teleported back on the other side of the map.</br></param>
        public GameObject(Position position, bool checkPosOverflow = false)
        {
            this.CheckPosOverflow = checkPosOverflow;
            this.color = Engine.BorderColor;
            this.Pos = position;
            Engine.AddRenderMethod(this.Render);
        }


        /// <summary>
        /// Creates new GameObject.
        /// </summary>
        /// <param name="checkPosOverflow">If the gameOject will move beyond the map borders 
        /// <br>it will be teleported back on the other side of the map.</br></param>
        public GameObject(bool checkPosOverflow = false)
        {
            this.CheckPosOverflow = checkPosOverflow;
            this.color = Engine.BorderColor;
            this.Pos = new Position(0, 0);
            Engine.AddRenderMethod(this.Render);
        }
        #endregion


        public void Dispose()
        {
            Engine.DeleteRenderMethod(this.Render);
            DeleteOldRender();
        }


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
            needsToBeRendered = true;
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
            needsToBeRendered = true;
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
            needsToBeRendered = true;
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
                if (Pos.X < 1) Pos.X = Engine.mapWidth;
                if (Pos.X > Engine.mapWidth) Pos.X = 1;
                if (Pos.Y < 1) Pos.Y = Engine.mapWidth;
                if (Pos.Y > Engine.mapWidth) Pos.Y = 1;
            }
        }
        #endregion

        #region Rendering

        /// <summary>
        /// Renders the GameObject.
        /// </summary>
        /// <param name="deleteOldOne">Decides if the old position will be deleted.</param>
        private void Render()
        {
            if (Engine.isRendering && needsToBeRendered && !Invisible)
            {
                if (lastPos != null)
                {
                    DeleteOldRender();
                }
                PrintNewRender();
            }
            needsToBeRendered = false;
        }

        /// <summary>
        /// Deltes the old render on the old position.
        /// </summary>
        private void DeleteOldRender()
        {
            if (lastPos.Y >= 1 && lastPos.Y <= Engine.mapHeight + Engine.BorderThicknes && lastPos.X >= 1 && lastPos.Y <= Engine.mapHeight + Engine.BorderThicknes)
            {
                Console.SetCursorPosition(2 * (Engine.BorderThicknes + lastPos.X - 1), Engine.BorderThicknes + lastPos.Y - 1);

                Console.BackgroundColor = Engine.defaultColor;
                Console.ForegroundColor = Engine.defaultColor;

                Console.Write("  ");
                Console.SetCursorPosition(2 * (2 * Engine.BorderThicknes + Engine.mapHeight + 1), 2 * Engine.BorderThicknes + Engine.mapHeight + 1);
            }
        }

        /// <summary>
        /// Prints render on the current position.
        /// </summary>
        private void PrintNewRender()
        {
            if (Pos.Y >= 1 && Pos.Y <= Engine.mapHeight + Engine.BorderThicknes && Pos.X >= 1 && Pos.Y <= Engine.mapHeight + Engine.BorderThicknes)
            {

                Console.SetCursorPosition(2 * (Engine.BorderThicknes + Pos.X - 1), Engine.BorderThicknes + Pos.Y - 1);
                Console.BackgroundColor = color;
                Console.ForegroundColor = color;

                Console.Write("##");
                Console.SetCursorPosition(2 * (2 * Engine.BorderThicknes + Engine.mapHeight + 1), 2 * Engine.BorderThicknes + Engine.mapHeight + 1);
            }

        }

        #endregion


        public void SetVisibility(bool invisible)
        {
            if (invisible && !this.Invisible)
            {
                this.Invisible = true;
                DeleteOldRender();
            }
            else if (!invisible && this.Invisible)
            {
                this.Invisible = false;
                needsToBeRendered = true;
            }
        }

        /// <summary>
        /// Changes the <c>color</c> of the GameObject.
        /// </summary>
        /// <param name="newColor">Decides the new color.</param>
        public void ChangeColor(ConsoleColor newColor)
        {
            this.color = newColor;
            needsToBeRendered = true;
            this.Render();
        }

        #region Collision

        /// <summary>
        /// Checks if the gameObject is colliding with something.
        /// </summary>
        /// <param name="gameObject">GameObject checks if it is colliding with this parametr.</param>
        /// <returns></returns>
        public bool IsColliding(GameObject gameObject)
        {
            return this.Pos == gameObject.Pos;
        }

        /// <summary>
        /// Checks if the gameObject is colliding with something.
        /// </summary>
        /// <param name="position">GameObject checks if it is colliding with this parametr.</param>
        /// <returns></returns>
        public bool IsColliding(Position position)
        {
            return this.Pos == position;
        }

        /// <summary>
        /// Checks if the gameObject is colliding with something.
        /// </summary>
        /// <param name="gameObjects">GameObject checks if it is colliding with this parametr.</param>
        /// <returns></returns>
        public bool IsColliding(GameObjects gameObjects)
        {
            if (this.Pos.X >= gameObjects.CollisionBox.posX && this.Pos.Y >= gameObjects.CollisionBox.posY)
            {
                if (this.Pos.X <= gameObjects.CollisionBox.posX+gameObjects.CollisionBox.Width && this.Pos.Y <= gameObjects.CollisionBox.posY + gameObjects.CollisionBox.Height)
                {
                    foreach (var bodyPartPosition in gameObjects.Positions)
                    {
                        if (this.Pos == bodyPartPosition)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

        }


        #endregion


    }


    /// <summary>
    /// This abstarct class can contain multiple GameObjects.
    /// <br>Good for multi cell bodies, like one of an snake.</br>
    /// </summary>
    abstract class GameObjects : IDisposable
    {

        public List<Position> Positions { get; private set; }
        public List<ConsoleColor> BodyColors { get; private set; }

        public CollisionBox CollisionBox { get; private set; }

        private List<Position> LastPositions;
        private bool checkPosOverflow;


        #region ctors
        /// <summary>
        /// Crates new colection of GameObjects
        /// </summary>
        /// <param name="color">Determines the color of first the GameObject.</param>
        /// <param name="position">Determines the first postion of the GameObject.</param>
        /// <param name="checkPosOverflow">If the any GameObjects will move beyond the map borders 
        /// <br>it will be teleported back on the other side of the map.</br></param>
        GameObjects(ConsoleColor color, Position position, bool checkPosOverflow = false)
        {
            this.checkPosOverflow = checkPosOverflow;

            Positions = new List<Position>();
            BodyColors = new List<ConsoleColor>();

            Positions.Add(position);
            BodyColors.Add(color);
        }

        /// <summary>
        /// Crates new colection of GameObjects
        /// </summary>
        /// <param name="position">Determines the first postion of the GameObject.</param>
        /// <param name="checkPosOverflow">If the any GameObjects will move beyond the map borders 
        /// <br>it will be teleported back on the other side of the map.</br></param>
        GameObjects(Position position, bool checkPosOverflow = false)
        {
            this.checkPosOverflow = checkPosOverflow;

            Positions = new List<Position>();
            BodyColors = new List<ConsoleColor>();

            Positions.Add(position);
            BodyColors.Add(Engine.BorderColor);
        }
        /// <summary>
        /// Crates new colection of GameObjects
        /// </summary>
        /// <param name="colors">Determines the color of every GameObject.</param>
        /// <param name="positions">Determines the postion of every GameObject.</param>
        /// <param name="checkPosOverflow">If the any GameObjects will move beyond the map borders 
        /// <br>it will be teleported back on the other side of the map.</br></param>
        GameObjects(List<ConsoleColor> colors, List<Position> positions, bool checkPosOverflow = false)
        {
            this.checkPosOverflow = checkPosOverflow;

            if (colors.Count == positions.Count)
            {
                this.BodyColors = colors;
                this.Positions = positions;
            }
            else
            {
                throw new ArgumentException("Colors and positions must have the same length.", "positions & colors");
            }
            Engine.AddRenderMethod(this.Render);
        }
        #endregion

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #region addBodypart
        /// <summary>
        /// Adds body part.
        /// </summary>
        /// <param name="color">Decides the color.</param>
        /// <param name="position">Decides the position.</param>
        public void AddBodyPart(Position position, ConsoleColor color)
        {
            BodyColors.Add(color);
            Positions.Add(position);
        }


        /// <summary>
        /// Adds body part.
        /// </summary>
        /// <param name="color">Decides the color.</param>
        /// <param name="position">Decides the position.</param>
        /// <param name="insertIndex">Decides the position in the list.</param>
        public void AddBodyPart(Position position, ConsoleColor color, int insertIndex)
        {
            BodyColors.Insert(insertIndex, color);
            Positions.Insert(insertIndex, position);
        }
        #endregion

        //Must complete
        private void Render()
        {

        }

        //Needs to be automatic
        private void SetCollisionBox()
        {
            
        }

        
    }
}
