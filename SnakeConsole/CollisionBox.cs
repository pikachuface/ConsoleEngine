namespace ConsoleEngine
{
    /// <summary>
    /// Class for Collision Box, used in Collision detection.
    /// </summary>
    internal class CollisionBox
    {
        internal Position position;
        internal int Height;
        internal int Width;
        internal int posX => this.position.X;
        internal int posY => this.position.Y;
        
            
        

        internal int X
        {
            get { return this.position.X; }
        }

        internal CollisionBox(Position position, int Height, int Width)
        {
            this.position = position;
            this.Width = Width;
            this.Height = Height;
        }
        internal CollisionBox(int posX, int posY, int Height, int Width)
        {
            this.position = new Position(posX, posY);
            this.Width = Width;
            this.Height = Height;
        }

        #region Update

        internal void UpdateAll(int newPosX, int newPosY, int newHeight, int newWidth)
        {
            this.position = new Position(newPosX, newPosY);
            this.Width = newWidth;
            this.Height = newHeight;
        }
        internal void UpdateAll(Position newPosition, int newWidth, int newHeight)
        {
            this.position = newPosition;
            this.Width = newWidth;
            this.Height = newHeight;
        }

        internal void UpdateSize(int newWidth, int newHeight)
        {
            this.Width = newWidth;
            this.Height = newHeight;
        }

        internal void UpdatePos(int newPosX, int newPosY) => this.position = new Position(newPosX, newPosY);

        internal void UpdatePos(Position newPosition) => this.position = newPosition;

        #endregion
    }


}

