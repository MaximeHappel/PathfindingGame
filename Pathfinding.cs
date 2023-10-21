using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AgentMap2
{
    /// <summary>
    /// Represents a class for pathfinding operations on a map.
    /// </summary>
    public class Pathfinding
    {

        private Map Map;
        //public List<Node> path { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Pathfinding class with the given map.
        /// </summary>
        /// <param name="map">The map on which pathfinding will be performed.</param>
        public Pathfinding(Map map)
        {
            Map = map;
        }

        /// <summary>
        /// Finds a path from the starting point to the target point using the A* pathfinding algorithm.
        /// </summary>
        /// <param name="start">The starting point for the path.</param>
        /// <param name="target">The target point for the path.</param>
        /// <returns>A list of nodes representing the path from the starting point to the target point.</returns>
        public List<Node> FindPath(Point start, Point target)
        {
            if (start == target)
            {
                throw new ArgumentException("Input Invalid: same start and target values");
            };


            // When the program stops searching for a path
            const int TIMOUT_IN_MILLISECONDS = 30000;
            Node startNode = new(start);
            Node targetNode = new(target);
            Node currentNode;
            List<Node> path = new();
            // Stores Nodes that has been visited or that are blocked
            HashSet<Node> closedSet = new();
            // Stores open Nodes to check by the algorithm
            List<Node> openSet = new();
            // Value that represents the distance between starting Node and the current Node 
            startNode.gCost = 0;
            // Value that represents the heuristic distance between start and target
            startNode.hCost = GetDistance(startNode, targetNode);
            // Stopwatch to track how long the method needs to be executed
            Stopwatch stopwatch = new();
            stopwatch.Start();

            // Add the initial Node to openSet
            openSet.Add(startNode);

            // Loop until there are no open Set is empty
            while (openSet.Count > 0)
            {
                if (stopwatch.ElapsedMilliseconds > TIMOUT_IN_MILLISECONDS)
                {
                    Console.WriteLine(openSet.Count);
                    throw new PathfindingTimeoutException("Pathfinding took too long: there is no path");
                }
                // Set current Node as the starting Node
                currentNode = openSet[0];

                for (int i = 1; i < openSet.Count; i++)
                {
                    /* Find the Node with a lower fCost in the open set.
                       If there are two Nodes with the same fCost, choose the one with lower hCost and update the currentNode
                    */
                    if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                // Path has been found if the current Node equals the target Node
                if (currentNode.Equals(targetNode))
                {
                    // Retrace the path and return
                    path = RetracePath(startNode, currentNode);
                    stopwatch.Stop();
                    stopwatch.Reset();
                    return path;
                }

                // Explore neighboring Node from the current Node
                foreach (Node neighbour in GetNeighbours(currentNode.Position))
                {
                    if (closedSet.Contains(neighbour))
                    {
                        // Skip this neighbor if it's already in the closed set.
                        continue;
                    }
                    else if (Map.obstacles.Any(obj => obj.Contains(new Point(neighbour.Position.X, neighbour.Position.Y))))
                    {
                        // Skip this neighbor if it overlaps with map obstacles.
                        closedSet.Add(neighbour);
                        continue;
                    }

                    // Update new gCost
                    int new_gCost = currentNode.gCost + GetDistance(currentNode, neighbour);

                    // If the new gCost is lower or the neighbor is not in the open set, update the neighbor's costs and set Parent Node
                    if (new_gCost < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = new_gCost;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;


                        if (!openSet.Contains(neighbour))
                        {
                            // Add the neighbor to the open set for further exploration.
                            openSet.Add(neighbour);
                        }
                    }

                }
            }
            if (openSet.Count <= 0)
            {
                throw new Exception("No Path found");
            }
            return path;
        }

        /// <summary>
        /// Retrieves neighboring nodes for a given position.
        /// </summary>
        /// <param name="position">The position for which neighboring nodes are needed.</param>
        /// <returns>An enumerable collection of neighboring nodes.</returns>
        private IEnumerable<Node> GetNeighbours(Point position)
        {
            var neighbours = new List<Node>
            {
                new Node(new Point(position.X + 1, position.Y)),
                new Node(new Point(position.X - 1, position.Y)),
                new Node(new Point(position.X, position.Y + 1)),
                new Node(new Point(position.X, position.Y - 1))
            };
            return neighbours;
        }

        /// <summary>
        /// Calculates the heuristic distance (H-cost) between two nodes using the Manhattan distance.
        /// </summary>
        /// <param name="nodeA">The first node.</param>
        /// <param name="nodeB">The second node.</param>
        /// <returns>The estimated distance between the two nodes.</returns>
        private int GetDistance(Node nodeA, Node nodeB)
        {
            int distX = Math.Abs(nodeA.Position.X - nodeB.Position.X);
            int distY = Math.Abs(nodeA.Position.Y - nodeB.Position.Y);

            if (distX > distY)
            {
                return 14 * distY + 10 * (distX - distY);
            }
            else
            {
                return 14 * distX + 10 * (distY - distX);
            }
        }

        /// <summary>
        /// Retraces the path from the target node back to the start node.
        /// </summary>
        /// <param name="startNode">The starting node of the path.</param>
        /// <param name="targetNode">The target node of the path.</param>
        /// <returns>A list of nodes representing the path from the start node to the target node in reverse order.</returns>
        public List <Node> RetracePath(Node startNode, Node targetNode)
        {
            List<Node> path = new();
            Node currentNode = targetNode;

            while (currentNode != startNode && currentNode != null)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Reverse();
            return path;
        }

        /// <summary>
        /// Converts a list of nodes into a string representation of the path with directional instructions (E, W, S, N).
        /// </summary>
        /// <param name="path">The list of nodes representing the path to be converted.</param>
        /// <returns>A string representation of the path with directional instructions, along with a message indicating it leads to the objective.</returns>
        public string PathToString(List<Node> path)
        {
            string pathAsString = "";
            foreach (Node node in path)
            {
                if (node.Parent.Position.X < node.Position.X)
                {
                    pathAsString += (char)Directions.East;
                }
                if (node.Parent.Position.X > node.Position.X)
                {
                    pathAsString += (char)Directions.West;
                }
                if (node.Parent.Position.Y < node.Position.Y)
                {
                    pathAsString += (char)Directions.South;
                }
                if (node.Parent.Position.Y > node.Position.Y)
                {
                    pathAsString += (char)Directions.North;
                }
            }
            return "The following path will take you to the objective:\n" + pathAsString;
        }


    }
}
