using UnityEngine;

namespace CustomUnityLibrary
{
    /// <summary>
    /// Contains x and y coordinates.
    /// Similar to Vector2, but stores ints instead of floats.
    /// </summary>
    [System.Serializable]
    public struct IntRect
    {
        /// <summary>
        /// The X coordinate of the rectangle.
        /// </summary>
        public int x;

        /// <summary>
        /// The Y coordinate of the rectangle
        /// </summary>
        public int y;

        /// <summary>
        /// The width of the rectangle, mesasured from the X position.
        /// </summary>
        public int width;

        /// <summary>
        /// The height of the rectangle, measured from the Y position.
        /// </summary>
        public int height;

        /// <summary>
        /// The position of the center of the rectangle.
        /// </summary>
        /// <value>The center of the rectangle</value>
        public Point2 center
        {
            get
            {
                int x = Mathf.RoundToInt(this.x + width / 2.0f);
                int y = Mathf.RoundToInt(this.y + height / 2.0f);
                return new Point2(x, y);
            }
            set
            {
                x = Mathf.RoundToInt(value.x - width / 2.0f);
                y = Mathf.RoundToInt(value.y - height / 2.0f);
            }
        }

        /// <summary>
        /// The position of the maximum corner of the rectangle.
        /// </summary>
        /// <value>The Point with the maximum x and maximum y of the rectangle</value>
        public Point2 max
        {
            get
            {
                int maxX = x + width;
                int maxY = y + height;
                return new Point2(maxX, maxY);
            }
            set
            {
                width = value.x - x;
                height = value.y - x;
            }
        }

        /// <summary>
        /// The position of the minimum corner of the rectangle.
        /// </summary>
        /// <value>The Point with the minimum x and minimum y of the rectangle</value>
        public Point2 min
        {
            get
            {
                return new Point2(x, y);
            }
            set
            {
                width = max.x - value.x;
                height = max.y - value.y;
                x = value.x;
                y = value.y;
            }
        }

        /// <summary>
        /// The X and Y position of the rectangle.
        /// </summary>
        /// <value>The position of the rectangle</value>
        public Point2 position
        {
            get
            {
                return new Point2(x, y);
            }
            set
            {
                x = value.x;
                y = value.y;
            }
        }

        /// <summary>
        /// The width and height of the rectangle.
        /// </summary>
        /// <value>The size of the rectangle</value>
        public Point2 size
        {
            get
            {
                return new Point2(width, height);
            }
            set
            {
                width = value.x;
                height = value.y;
            }
        }

        /// <summary>
        /// The maximum X coordinate of the rectangle.
        /// </summary>
        /// <value>The maximum X coordinate</value>
        public int xMax
        {
            get
            {
                return x + width;
            }
            set
            {
                width = value - x;
            }
        }

        /// <summary>
        /// The minimum X coordinate of the rectangle.
        /// </summary>
        /// <value>The minimum X coordinate</value>
        public int xMin
        {
            get
            {
                return x;
            }
            set
            {
                width = xMax - value;
                x = value;
            }
        }

        /// <summary>
        /// The maximum Y coordinate of the rectangle.
        /// </summary>
        /// <value>The maximum Y coordinate</value>
        public int yMax
        {
            get
            {
                return y + height;
            }
            set
            {
                height = value - y;
            }
        }

        /// <summary>
        /// The minumum Y coordinate of the rectangle.
        /// </summary>
        /// <value>The minimum Y coordinate</value>
        public int yMin
        {
            get
            {
                return y;
            }
            set
            {
                height = yMax - value;
                y = value;
            }
        }

        /// <summary>
        /// Creates a new rectangle.
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="width">The width of the rectangle</param>
        /// <param name="height">The height of the rectangle</param>
        public IntRect(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Creates a new rectangle.
        /// </summary>
        /// <param name="position">The position of the rectangle</param>
        /// <param name="size">The size of the rectangle</param>
        public IntRect(Point2 position, Point2 size)
        {
            x = position.x;
            y = position.y;
            width = size.x;
            height = size.y;
        }

        /// <summary>
        /// Creates a new rectangle.
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="size">The size of the rectangle</param>
        public IntRect(int x, int y, Point2 size)
        {
            this.x = x;
            this.y = y;
            width = size.x;
            height = size.y;
        }

        /// <summary>
        /// Creates a new rectangle.
        /// </summary>
        /// <param name="position">The position of the rectangle</param>
        /// <param name="width">The width of the rectangle</param>
        /// <param name="height">The height of the rectangle</param>
        public IntRect(Point2 position, int width, int height)
        {
            x = position.x;
            y = position.y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Checks to see if the two objects are equal.
        /// </summary>
        /// <param name="obj">The object to compare to the IntRect</param>
        /// <returns>Whether or not the two Objects are both IntRects and are equal</returns>
        public override bool Equals(object obj)
        {
            return obj is IntRect && this == (IntRect)obj;
        }

        /// <summary>
        /// Gets the HashCode for this IntRect.
        /// </summary>
        /// <returns>The IntRect's hashcode</returns>
        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ width.GetHashCode() ^ height.GetHashCode();
        }

        /// <summary>
        /// Checks if the two instances of IntRects are equivalent.
        /// </summary>
        /// <param name="lhs">First instance of IntRect</param>
        /// <param name="rhs">Second instance of IntRect</param>
        /// <returns>Whether or not the two IntRects have the same values</returns>
        public static bool operator ==(IntRect lhs, IntRect rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
        }

        /// <summary>
        /// Checks if the two instances of IntRects are inequivalent.
        /// </summary>
        /// <param name="lhs">First instance of IntRect</param>
        /// <param name="rhs">Second instance of IntRect</param>
        /// <returns>Whether or not the two IntRects have different values</returns>
        public static bool operator !=(IntRect lhs, IntRect rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Creates a IntRect from the min/max coordinate values.
        /// </summary>
        /// <param name="minX">The minimum x for the new IntRect</param>
        /// <param name="minY">The minimum y for the new IntRect</param>
        /// <param name="maxX">The maximum x for the new IntRect</param>
        /// <param name="maxY">The maximum y for the new IntRect</param>
        /// <returns>IntRect.</returns>
        public static IntRect MinMaxIntRect(int minX, int minY, int maxX, int maxY)
        {
            int x = minX;
            int y = minY;
            int width = maxX - minX;
            int height = maxY - minY;
            return new IntRect(x, y, width, height);
        }

        /// <summary>
        /// Returns a point inside a rectangle, given normalized coordinates.
        /// </summary>
        /// <param name="intRectangle">The IntRect to get the Point from</param>
        /// <param name="normalizedRectCoordinates">The normalized coordinates used to find the point inside the int rect</param>
        /// <returns>The point within the IntRect</returns>
        public static Point2 NormalizedToPoint(IntRect intRectangle, Vector2 normalizedRectCoordinates)
        {
            normalizedRectCoordinates = Vector2.Min(normalizedRectCoordinates, Vector2.one);
            normalizedRectCoordinates = Vector2.Max(normalizedRectCoordinates, Vector2.zero);
            int x = Mathf.RoundToInt(intRectangle.x + normalizedRectCoordinates.x * intRectangle.width);
            int y = Mathf.RoundToInt(intRectangle.y + normalizedRectCoordinates.y * intRectangle.height);
            return new Point2(x, y);
        }

        /// <summary>
        /// Returns the normalized coordinates of the coresponding point.
        /// </summary>
        /// <param name="intRectangle">The IntRect to get the point from</param>
        /// <param name="point">The point within the rectangle to get the normalized point from</param>
        /// <returns>The normalized point within the rectangle</returns>
        public static Vector2 PointToNormalized(IntRect intRectangle, Point2 point)
        {
            float x = intRectangle.width == 0 ? 0.0f : (float)(point.x - intRectangle.x) / intRectangle.width;
            float y = intRectangle.height == 0 ? 0.0f : (float)(point.y - intRectangle.y) / intRectangle.height;
            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns true if the x and y components of point is a point inside this rectangle.
        /// </summary>
        /// <param name="point">The point to check for</param>
        /// <returns>Whether or not the point is within the rectangle</returns>
        public bool Contains(Point2 point)
        {
            return point.x >= xMin && point.x <= xMax && point.y >= yMin && point.y <= yMax;
        }

        /// <summary>
        /// Returns true if the other rectangle overlaps this one.
        /// </summary>
        /// <param name="other">The other rectangle to check for overlaps with</param>
        /// <returns>Whether or not the two rectangles overlap eachother</returns>
        public bool Overlaps(IntRect other)
        {
            return xMin <= other.xMax && xMax >= other.xMax && yMin <= other.yMax && yMax >= other.yMin;
        }

        /// <summary>
        /// Sets components of an existing IntRect.
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        public void Set(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// The string interpretation of this IntRect
        /// </summary>
        /// <returns>The IntRect's string</returns>
        public override string ToString()
        {
            return "(" + x + "," + y + "," + width + "," + height + ")";
        }
    }
}