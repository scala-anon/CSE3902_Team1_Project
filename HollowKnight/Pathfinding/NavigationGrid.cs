using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HollowKnight.Pathfinding
{
  public class NavigationGrid
  {
    private bool[,] _blocked; //true = blocked, false = walkable
    public int cellSize { get; }
    public int cols { get; }
    public int rows { get; }
    public static bool GridEnabled { get; set; } = true;

    private Texture2D _pixel; 

    public NavigationGrid(int screenWidth, int screenHeight, GraphicsDevice graphicsDevice, int cellSize = 32)
    {
      this.cellSize = cellSize;
      cols = screenWidth / cellSize;
      rows = screenHeight / cellSize;
      _blocked = new bool[cols, rows];

      _pixel = new Texture2D(graphicsDevice, 1, 1); // Create a 1x1 white texture for drawing
      _pixel.SetData(new[] { Color.White }); 
    }

    
    public void AddObstacle(Rectangle bounds)
    {
      int startCol = Math.Max(0, bounds.Left / cellSize);
      int endCol = Math.Min(cols - 1, bounds.Right / cellSize);
      int startRow = Math.Max(0, bounds.Top / cellSize);
      int endRow = Math.Min(rows - 1, bounds.Bottom / cellSize);

      for(int x = startCol; x <= endCol; x++)
      {
        for(int y = startRow; y <= endRow; y++)
        {
          _blocked[x, y] = true;
        }
      }
    }

    public bool IsWalkable(int x, int y)
    {
       if (x < 0 || x >= cols || y < 0 || y >= rows) return false;
       return !_blocked[x, y];
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      Color lineColor = Color.White * 0.2f; // transparency

      //Vertical lines
      for(int x=0; x<=cols; x++)
      {
        spriteBatch.Draw(
          _pixel, 
          new Rectangle(x * cellSize, 0, 1, rows * cellSize),
          lineColor);
      }
      //Horizontal lines
      for(int y=0; y<=rows; y++)
      {
        spriteBatch.Draw(
          _pixel, 
          new Rectangle(0, y * cellSize, cols * cellSize, 1),
          lineColor);
      }
    }

  }
}