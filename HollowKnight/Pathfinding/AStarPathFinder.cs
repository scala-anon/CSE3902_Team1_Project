using System;
using System.Collections.Generic;
using HollowKnight.Shared;
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

        public static List<Vector2> FindPath(NavigationGrid grid, Vector2 startPixel, Vector2 targetPixel, Vector2? entityBounds = null)
        {
            List<Vector2> path = new List<Vector2>();
            DebugLogger.LogAStar($"Search start=({startPixel.X:F0},{startPixel.Y:F0}) goal=({targetPixel.X:F0},{targetPixel.Y:F0})");

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
            HashSet<int> closedList = new HashSet<int>();
            Dictionary<int, Node> nodeMap = new Dictionary<int, Node>();

            Node startNode = new Node { X = startX, Y = startY, G = 0, H = GetOctileDistance(startX, startY, targetX, targetY) };
            openList.Add(startNode);
            nodeMap[startX + startY * grid.cols] = startNode;

            int maxIterations = GameConstants.AStarMaxIterations; // Failsafe to prevent infinite loops / lag
            int currentIteration = 0;

            Node current = null;

            while (openList.Count > 0 && currentIteration < maxIterations)
            {
                currentIteration++;

                // Get node with lowest F cost efficiently (O(N) instead of O(N log N))
                int lowestIndex = 0;
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].F < openList[lowestIndex].F)
                    {
                        lowestIndex = i;
                    }
                }
                current = openList[lowestIndex];
                openList[lowestIndex] = openList[openList.Count - 1]; // Swap with last element
                openList.RemoveAt(openList.Count - 1); // O(1) removal

                int currentKey = current.X + current.Y * grid.cols;
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

                    if (!IsAreaWalkable(grid, neighborX, neighborY, entityBounds)) continue;
                    
                    // Prevent corner-cutting for diagonal movements
                    if (dx[i] != 0 && dy[i] != 0)
                    {
                        if (!IsAreaWalkable(grid, current.X, neighborY, entityBounds) || !IsAreaWalkable(grid, neighborX, current.Y, entityBounds))
                        {
                            continue;
                        }
                    }
                    
                    int neighborKey = neighborX + neighborY * grid.cols;
                    if (closedList.Contains(neighborKey)) continue;

                    // 1.4 for diagonal, 1.0 for straight using pythagorean theorem.
                    // Using integers (14/10) since it is easier for integer math
                    int moveCost = (dx[i] != 0 && dy[i] != 0) ? GameConstants.AStarDiagonalMoveCost : GameConstants.AStarStraightMoveCost;
                    int newCostToNeighbor = current.G + moveCost;

                    nodeMap.TryGetValue(neighborKey, out Node neighborNode);
                    if (neighborNode == null || newCostToNeighbor < neighborNode.G)
                    {
                        if (neighborNode == null)
                        {
                            neighborNode = new Node { X = neighborX, Y = neighborY };
                            openList.Add(neighborNode);
                            nodeMap[neighborKey] = neighborNode;
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
                DebugLogger.LogAStar($"Path found: length={path.Count} nodes expanded={currentIteration}");
            }
            else
            {
                DebugLogger.LogAStar($"Path failed: start=({startPixel.X:F0},{startPixel.Y:F0}) goal=({targetPixel.X:F0},{targetPixel.Y:F0}) iterations={currentIteration}");
            }

            return path;
        }

        private static bool IsAreaWalkable(NavigationGrid grid, int cx, int cy, Vector2? entityBounds)
        {
            if (!grid.IsWalkable(cx, cy)) return false;
            if (!entityBounds.HasValue) return true;

            int cellsX = (int)Math.Ceiling(entityBounds.Value.X / grid.cellSize) - 1; // subtract 1 to ensure some leniency
            int cellsY = (int)Math.Ceiling(entityBounds.Value.Y / grid.cellSize) - 1;
            
            if (cellsX <= 0 && cellsY <= 0) return true;

            int halfX = cellsX / 2;
            int halfY = cellsY / 2;

            for (int x = cx - halfX; x <= cx + halfX; x++)
            {
                for (int y = cy - halfY; y <= cy + halfY; y++)
                {
                    if (!grid.IsWalkable(x, y)) return false;
                }
            }
            return true;
        }

        private static int GetOctileDistance(int x1, int y1, int x2, int y2)
        {
            int dx = Math.Abs(x1 - x2);
            int dy = Math.Abs(y1 - y2);

            // Uses integer approximation of diagonal cost vs straight cost.
            // Formula: StraightCost * max + (DiagonalCost - StraightCost) * min
            int diagCorrection = GameConstants.AStarDiagonalMoveCost - GameConstants.AStarStraightMoveCost;
            return GameConstants.AStarStraightMoveCost * Math.Max(dx, dy) + diagCorrection * Math.Min(dx, dy);
        }
        
    }
}
