using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentMap2
{
    /// <summary>
    /// Represents a sensor obstacle with a specified range.
    /// </summary>
    public class Sensor : Obstacle
    {
        private double Range { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Sensor"/> class with the specified center and range.
        /// </summary>
        /// <param name="center">The center point of the sensor.</param>
        /// <param name="range">The range of the sensor, in distance units.</param>
        public Sensor(Point center, double range) : base(center)
        {
            Range = range;
            Type = ObstacleType.Sensor;

        }

        /// <summary>
        /// Checks if a specified point is within the sensor's range.
        /// </summary>
        /// <param name="point">The point to check for containment.</param>
        /// <returns>True if the point is within the sensor's range; otherwise, false.</returns>
        public override bool Contains(Point point)
        {
            double distance = Math.Sqrt(Math.Pow((point.X - Center.X), 2) + Math.Pow((point.Y - Center.Y), 2));
            return distance <= Range;
        }

    }
}
