using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentMap2
{
    /// <summary>
    /// Represents a node in a pathfinding algorithm, defined by a position on the map.
    /// </summary>
    public class Node
    {
        public Point Position { get; }
        // Cost from start node to current node
        public int gCost;

        // Cost from current node to target node
        public int hCost;
        public Node? Parent { get; set; }
        // Sum of hCost and gCost
        public int FCost { get { return hCost + gCost; } }

        /// <summary>
        /// Constructs a new Node with the specified position.
        /// </summary>
        /// <param name="position">The position (coordinates) of the node.</param>
        public Node(Point position)
        {
            Position = position;
        }

        /// <summary>
        /// Determines whether this Node is equal to another object based on its position.
        /// </summary>
        /// <param name="obj">The object to compare to this Node.</param>
        /// <returns>
        ///  True, if the specified object is equal to this Node; otherwise, false.
        /// </returns>
        public override bool Equals(object? obj)
        {
            // If the object is null or not of the same type, they can't be equal.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // Convert the object to a Node for comparison.
            Node otherNode = (Node)obj;

            // Two nodes are considered equal if their positions match.
            return Position.X == otherNode.Position.X && Position.Y == otherNode.Position.Y;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current Node.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Position.X, Position.Y);
        }    
    }
}
