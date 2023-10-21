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
    /// Represents a camera obstacle that has a viewing angle in certain directions.
    /// </summary>
    public class Camera : Obstacle
    {
        private Vector2 direction = new(0, 0);
        private Vector2 camToPoint;

        /// <summary>
        /// Initializes a new instance of the Camera class with a specified center and direction.
        /// </summary>
        /// <param name="center">The center point of the camera.</param>
        /// <param name="direction">The direction of the camera (n, s, e, or w).</param>
        public Camera(Point center, char direction) : base(center)
        {
            SetDirection(direction);
            Type = ObstacleType.Camera;
        }

        /// <summary>
        /// Sets the direction of the camera based on a specified character.
        /// </summary>
        /// <param name="direction">A character representing the camera direction (n, s, e, or w).</param>
        private void SetDirection(char direction)
        {
            switch (direction)
            {
                case 'n':
                    this.direction.Y += 1;
                    break;
                case 's':
                    this.direction.Y += -1;
                    break;
                case 'e':
                    this.direction.X += 1;
                    break;
                case 'w':
                    this.direction.X += -1;
                    break;
                default:
                    throw new ArgumentException("Invalid Direction");
            }
        }

        /// <summary>
        /// Determines whether the camera contains a specific point within its view.
        /// </summary>
        /// <param name="point">The point to be checked.</param>
        /// <returns> True if the point is within the camera's view; otherwise, false.
        /// </returns>
        public override bool Contains(Point point)
        {
            // Calculate Vector from cam to point
            camToPoint = VectorCalc.GetVectorBetweenPoints(point, Center);
            // Calculate angle between vector from Cam to Point and Cam Direction
            double angleToPoint = Math.Atan2(camToPoint.Y, camToPoint.X) - Math.Atan2(direction.Y, direction.X);
            // Normalize if neg degree
            if (angleToPoint < 0)
            {
                angleToPoint += 2 * Math.PI;
            }
            double viewingAngle = Math.PI / 2;
            //if angle in range of camera angle return true
            if (angleToPoint <= viewingAngle / 2 || angleToPoint >= 2 * Math.PI - viewingAngle / 2 || Center == point)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
