using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentMap2
{
    /// <summary>
    /// Represents a map that can contain obstacles and provides methods for map-related operations.
    /// </summary>
    public class Map
    {
        // List that stores all obstacles on a map
        public List<Obstacle> obstacles = new();

        /// <summary>
        /// Draws a map based on the specified range of cells and the location of obstacles.
        /// </summary>
        /// <param name="topLeftCell">The top-left cell of the map.</param>
        /// <param name="bottomRightCell">The bottom-right cell of the map.</param>
        public void DrawMap(Point topLeftCell, Point bottomRightCell)
        {
            if (topLeftCell.X > bottomRightCell.X || topLeftCell.Y > bottomRightCell.Y)
            {
               throw new Exception("Invalid map specification.");
            }
            for (int y = topLeftCell.Y; y <= bottomRightCell.Y; y++)
            {
                for (int x = topLeftCell.X; x <= bottomRightCell.X; x++)
                {
                    // Flag if a position contains an obstacle or not
                    bool contains = false;

                    // Write the letter of the obstacle if it is on a specific location
                    foreach (var obj in obstacles)
                    {
                        if (obj.Contains(new Point(x, y)))
                        {
                            Console.Write(((char)obj.Type));
                            contains = true;
                            break;
                        }
                    }

                    // Otherwise print a dot
                    if (contains == false)
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Prints safe directions based on the agent's current position and map obstacles.
        /// </summary>
        /// <param name="position">The agent's current position.</param>
        public void PrintSafeDirections(Point position)
        {
            //todo: besser den Stuff in getsafedirections ballern und hier nur printen, dann kann parameter weggelassen werden
            if (obstacles.Any(obj => obj.Contains(position)))
            {
                Console.Write("Agent, your location is compromised. Abort mission.");
                Console.WriteLine();

            }
            else
            {
                List<char> safeDirections = GetSafeDirections(position);
                if (safeDirections.Count == 0)
                {
                    Console.WriteLine("You cannot safely move in any direction. Abort mission.");
                    Console.WriteLine();
                }
                else
                {
                    Console.Write("You can safely take any of the following directions: ");
                    foreach (var dir in safeDirections)
                    {
                        Console.Write(dir);

                    }
                    Console.WriteLine();
                }

            }
        }

        /// <summary>
        /// Determines the safe directions the agent can move to from its current position.
        /// </summary>
        /// <param name="position">The agent's current position.</param>
        /// <returns>A list of safe directions represented as characters ('N', 'S', 'E', 'W').</returns>
        private List<char> GetSafeDirections(Point position)
        {
            List<char> safeDirections = new();

            Dictionary<Directions, Point> directions = new()
            {
                { Directions.North, new Point(position.X, position.Y - 1) },
                { Directions.South, new Point(position.X, position.Y + 1) },
                { Directions.East, new Point(position.X + 1, position.Y) },
                { Directions.West, new Point(position.X - 1, position.Y) }
            };

            // Check for every direction if it is obstructed by an obstacle. If not then add it to the safeDirections list
            foreach (var dirs in directions)
            {
                if (obstacles.Any(obj => obj.Contains(dirs.Value)))
                {
                    continue;
                }
                else
                {
                    safeDirections.Add((char)dirs.Key);
                }
            }
            if (safeDirections.Count == 4)
            {

            }
            return safeDirections;

        }


    }
}
