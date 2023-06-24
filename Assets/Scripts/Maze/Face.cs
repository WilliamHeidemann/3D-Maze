namespace Maze
{
    public class Face
    {
        public Square[,] Squares;
        public Face(int width, int height, Orientation orientation)
        {
            CreateSquares(width, height, orientation);
            ConnectDirectNeighbors(width, height);
        }
        private void CreateSquares(int width, int height, Orientation orientation)
        {
            Squares = new Square[width, height];
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    Squares[w, h] = new Square(orientation);
                }
            }
        }
        private void ConnectDirectNeighbors(int width, int height)
        {
            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    if (w < width - 1)
                    {
                        var wall = new Wall();
                        Squares[w, h].SetNeighbor(Orientation.Right, (Squares[w + 1, h], wall));
                        Squares[w + 1, h].SetNeighbor(Orientation.Left, (Squares[w, h], wall));
                    }
                
                    if (h < height - 1)
                    {
                        var wall = new Wall();
                        Squares[w, h].SetNeighbor(Orientation.Up, (Squares[w, h + 1], wall));
                        Squares[w, h + 1].SetNeighbor(Orientation.Down, (Squares[w, h], wall));
                    }
                }
            }
        }
    }
}