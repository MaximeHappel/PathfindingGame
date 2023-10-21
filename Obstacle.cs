using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentMap2
{

    /// <summary>
    /// Enum that represents different types of obstacles.
    /// </summary>
    public enum ObstacleType
    {
        Fence = 'f',
        Guard = 'g',
        Sensor = 's',
        Camera = 'c',
        MagicCross = 't'
    }
    /// <summary>
    /// Represents an abstract obstacle on the map with a center point.
    /// </summary>
    public abstract class Obstacle
    {

        public Point Center { get; private set; }
        public ObstacleType Type { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class with the specified center point.
        /// </summary>
        /// <param name="center">The center point of the obstacle.</param>
        public Obstacle(Point center)
        {
            this.Center = center;
        }
        /// <summary>
        /// Checks if a specified point is contained within the obstacle.
        /// </summary>
        /// <param name="point">The point to check for containment.</param>
        /// <returns>True if the point is contained within the obstacle; otherwise, false.</returns>
        public abstract bool Contains(Point point);

    }
}
