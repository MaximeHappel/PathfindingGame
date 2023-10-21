using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentMap2
{
    /// <summary>
    /// Represents a guard obstacle on the map.
    /// </summary>
    public class Guard : Obstacle
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Guard"/> class with the specified center point.
        /// </summary>
        /// <param name="center">The center point of the guard.</param>
        public Guard(Point center) : base(center)
        {
            Type = ObstacleType.Guard;
        }

        /// <summary>
        /// Checks if a specified point is within the guard obstacle.
        /// </summary>
        /// <param name="point">The point to check for containment.</param>
        /// <returns>True if the point is equal to the guard's center; otherwise, false.</returns>
        public override bool Contains(Point point)
        {
            return point.Equals(Center);

        }
    }
}
