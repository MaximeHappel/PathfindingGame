using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgentMap2
{
    /// <summary>
    /// Represents a magical cross obstacle.
    /// </summary>
    public class MagicCross : Obstacle
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MagicCross"/> class with the specified center point.
        /// </summary>
        /// <param name="center">The center point of the magical cross.</param>
        public MagicCross(Point center) : base(center)
        {
            Type = ObstacleType.MagicCross;
        }

        /// <summary>
        /// Builds the points that form the magical cross.
        /// </summary>
        /// <param name="center">The center point of the magical cross.</param>
        /// <param name="IsNight">A boolean indicating if it is nighttime.</param>
        /// <returns>A list of points that form the magical cross obstacle.</returns>
        private List<Point> BuildCross(Point center, bool IsNight)
        {
            List<Point> cross = new();
            // Adding the coordinates to the List based on the center
            for (int i = -2; i <= 4; i++)
            {
                cross.Add(new Point(center.X, center.Y + i));
            }
            for (int i = -3; i <= 3; i++)
            {   
                // At night time the cross gets reversed
                cross.Add(new Point(center.X + i, !IsNight ? center.Y : center.Y + 2));
            }
            return cross;
        }

        /// <summary>
        /// Determines if it is currently nighttime.
        /// </summary>
        /// <returns>True if it is nighttime; otherwise, false.</returns>
        private bool IsNight()
        {
            const int STARTNIGHT = 20;
            const int ENDNIGHT = 6;
            int currentHour = DateTime.Now.Hour;

            if (currentHour >= STARTNIGHT && currentHour <= ENDNIGHT)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a specified point is within the magical cross obstacle.
        /// </summary>
        /// <param name="point">The point to check for containment.</param>
        /// <returns>True if the point is within the magical cross; otherwise, false.</returns>
        public override bool Contains(Point point)
        {
            return BuildCross(Center, IsNight()).Contains(point);
        }
    }
}
