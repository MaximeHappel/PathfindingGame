using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AgentMap2
{
    /// <summary>
    /// A utility class for vector calculations.
    /// </summary>
    public static class VectorCalc
    {
        /// <summary>
        /// Calculates the vector between two specified points.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        /// <returns>The vector from p1 to p2.</returns>
        public static Vector2 GetVectorBetweenPoints(Point p1, Point p2)
        {
            return new Vector2(p1.X - p2.X, p2.Y - p1.Y);
        }




    }
}
