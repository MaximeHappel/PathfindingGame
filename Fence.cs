using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentMap2
{
    /// <summary>
    /// Represents a fence obstacle on the map, which can be either horizontal or vertical.
    /// </summary>
    public class Fence : Obstacle
    {     
        private Point FenceStart { get; set; }
        private Point FenceEnd { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fence"/> class with the specified starting and ending points.
        /// </summary>
        /// <param name="fenceStart">The starting point of the fence.</param>
        /// <param name="fenceEnd">The ending point of the fence.</param>
        public Fence(Point fenceStart, Point fenceEnd) : base(fenceStart)
        {
            FenceStart = fenceStart;
            FenceEnd = fenceEnd;
            Type = ObstacleType.Fence;


            if (fenceStart.Y != fenceEnd.Y && fenceStart.X != fenceEnd.X)
            {
                throw new ArgumentException("Only horizontal or vertical fences allowed");
            }
        }

        /// <summary>
        /// Checks if a specified point is within the fence obstacle.
        /// </summary>
        /// <param name="point">The point to check for containment.</param>
        /// <returns>True if the point is within the fence; otherwise, false.</returns>
        public override bool Contains(Point point)
        {
            bool check = ((point.X >= FenceStart.X && point.X <= FenceEnd.X) && (point.Y >= FenceStart.Y && point.Y <= FenceEnd.Y)) || ((point.X <= FenceStart.X && point.X >= FenceEnd.X) && (point.Y <= FenceStart.Y && point.Y >= FenceEnd.Y));

            return check;

        }
    }
}
