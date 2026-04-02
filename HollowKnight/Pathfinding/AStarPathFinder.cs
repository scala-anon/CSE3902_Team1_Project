using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace HollowKnight.Pathfinding
{
    public class AStarPathFinder
    {
        private class Node
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int G { get; set; } // Cost from start
            public int H { get; set; } // Heuristic cost to end
            public int F => G + H;     // Total cost
            public Node Parent { get; set; }
        }

        public static List<Vector2> FindPath(NavigationGrid grid, Vector2 startPixel, Vector2 targetPixel)
        {
            List<Vector2> path = new List<Vector2>();

            int startX = (int)(startPixel.X / grid.cellSize);
            int startY = (int)(startPixel.Y / grid.cellSize);
            int targetX = (int)(targetPixel.X / grid.cellSize);
            int targetY = (int)(targetPixel.Y / grid.cellSize);

            // Clamp out of bounds
            startX = Math.Clamp(startX, 0, grid.cols - 1);
            startY = Math.Clamp(startY, 0, grid.rows - 1);
            targetX = Math.Clamp(targetX, 0, grid.cols - 1);
            targetY = Math.Clamp(targetY, 0, grid.rows - 1);

            // If start or target is identical cell, return current target pixel
            if (startX == targetX && startY == targetY)
            {
                path.Add(targetPixel);
                return path;
            }

            List<Node> openList = new List<Node>();
            HashSet<string> closedList = new HashSet<string>();

            Node startNode = new Node { X = startX, Y = startY, G = 0, H = GetOctileDistance(startX, startY, targetX, targetY) };
            openList.Add(startNode);

            int maxIterations = 1000; // Failsafe to prevent infinite loops / lag
            int currentIteration = 0;

            Node current = null;

            while (openList.Count > 0 && currentIteration < maxIterations)
            {
                currentIteration++;

                // Get node with lowest F cost
                openList.Sort((a, b) => a.F.CompareTo(b.F));
                current = openList[0];
                openList.RemoveAt(0);

                string currentKey = $"{current.X},{current.Y}";
                closedList.Add(currentKey);

                // Found target
                if (current.X == targetX && current.Y == targetY)
                {
                    break;
                }

                // Check neighbors
                int[] dx = { 0, 0, -1, 1, -1, 1, -1, 1 };
                int[] dy = { -1, 1, 0, 0, -1, -1, 1, 1 };

                for (int i = 0; i < 8; i++)
                {
                    int neighborX = current.X + dx[i];
                    int neighborY = current.Y + dy[i];

                    if (!grid.IsWalkable(neighborX, neighborY)) continue;

                    string neighborKey = $"{neighborX},{neighborY}";
                    if (closedList.Contains(neighborKey)) continue;

                    // 1.4 for diagonal, 1.0 for straight using pythagorean theorem.
                    //using 10 and 14 since it is easier for integer math
                    int moveCost = (dx[i] != 0 && dy[i] != 0) ? 14 : 10; 
                    int newCostToNeighbor = current.G + moveCost;

                    Node neighborNode = openList.Find(n => n.X == neighborX && n.Y == neighborY);
                    if (neighborNode == null || newCostToNeighbor < neighborNode.G)
                    {
                        if (neighborNode == null)
                        {
                            neighborNode = new Node { X = neighborX, Y = neighborY };
                            openList.Add(neighborNode);
                        }
                        
                        neighborNode.G = newCostToNeighbor;
                        neighborNode.H = GetOctileDistance(neighborX, neighborY, targetX, targetY);
                        neighborNode.Parent = current;
                    }
                }
            }

            // Retrace path and convert to Vector2 pixels centered on cells
            if (current != null && current.X == targetX && current.Y == targetY)
            {
                while (current.Parent != null) // Don't include the start block
                {
                    path.Add(new Vector2(current.X * grid.cellSize + grid.cellSize / 2f, current.Y * grid.cellSize + grid.cellSize / 2f));
                    current = current.Parent;
                }
                path.Reverse(); // Reverse to get start to end
            }

            return path;
        }

        /*
        private static int GetManhattanDistance(int x1, int y1, int x2, int y2)
        {
            // Multiplying by 10 to make integer math easier
            return 10 * (Math.Abs(x1 - x2) + Math.Abs(y1 - y2));
        }
        */
        
        private static int GetOctileDistance(int x1, int y1, int x2, int y2)
        {
            int dx = Math.Abs(x1 - x2);
            int dy = Math.Abs(y1 - y2);

            // Uses integer approximation of diagonal cost (14) vs straight cost (10).
            // Formula: 10 * (dx + dy) + (14 - 20) * min(dx, dy)
            // Simplified: 10 * max + 4 * min
            return 10 * Math.Max(dx, dy) + 4 * Math.Min(dx, dy);
        }
        
    }
}
