namespace ConsoleEngine
{
    /// <summary>
    /// Class that stores 2D position
    /// </summary>
    internal class Position
    {
        public int X;
        public int Y;

        /// <summary>
        /// Cretes new position struct
        /// </summary>
        internal Position(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
        /// <summary>
        /// Cretes new position struct
        /// </summary>
        internal Position(Position position)
        {
            this.X = position.X;
            this.Y = position.Y;
        }
    }
}
