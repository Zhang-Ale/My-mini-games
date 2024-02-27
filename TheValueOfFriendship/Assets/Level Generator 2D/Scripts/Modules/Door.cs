namespace LevelGenerator2D
{
    using CustomUnityLibrary;
    using System;
    using UnityEngine;

    /// <summary>
    /// An opening where two Rooms can be attached
    /// </summary>
    [Serializable]
    public class Door
    {
#pragma warning disable 0414
        [Tooltip("Name assigned to this door")]
        [SerializeField]
        private string name;
#pragma warning restore 0414

        [Tooltip("Indexed position of this door")]
        [SerializeField]
        private int number;

        private bool open = true;
        private Point2 localPoint;
        private Point2 globalPoint;
        private Direction side;

        /// <summary>
        /// Creates a door
        /// </summary>
        /// <param name="name">Name of door</param>
        /// <param name="number">Index of door</param>
        public Door(string name, int number)
        {
            SetName(name);
            SetIndex(number);
        }

        /// <summary>
        /// Checks if the Door is a valid match with another Door
        /// </summary>
        /// <param name="door">Door to compare to</param>
        /// <returns>Whether or not the two Doors match</returns>
        public bool PairsWith(Door door)
        {
            if (door == null)
            {
                return false;
            }
            return GetNeighborPoint() == door.GetGlobalPoint() && door.GetNeighborPoint() == GetGlobalPoint();
        }

        /// <summary>
        /// Gets the global point to which this door connects
        /// </summary>
        /// <returns>Global point where the neighboring Room connects, or would connect, to this Door</returns>
        public Point2 GetNeighborPoint()
        {
            var neighborPoint = GetGlobalPoint();
            switch (GetSide())
            {
                case Direction.West:
                    neighborPoint.x -= 1;
                    break;
                case Direction.North:
                    neighborPoint.y += 1;
                    break;
                case Direction.East:
                    neighborPoint.x += 1;
                    break;
                case Direction.South:
                    neighborPoint.y -= 1;
                    break;
                default:
                    Debug.LogError("Invalid Door Direction (" + GetSide() + " received!");
                    break;
            }
            return neighborPoint;
        }

        /// <summary>
        /// Gets the indexed number within the Room of this Door
        /// </summary>
        /// <returns>Door number</returns>
        public int GetIndex()
        {
            return number;
        }

        /// <summary>
        /// Sets the indexed number within the Room of this Door
        /// </summary>
        public void SetIndex(int index)
        {
            number = index;
        }

        /// <summary>
        /// Sets the name associated with this Door
        /// </summary>
        /// <param name="name">Name of Door</param>
        public void SetName(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Gets whether or not this door is open and available, opposed to having a Room already connected to it on both sides
        /// </summary>
        /// <returns>Whether or not the door is open</returns>
        public bool IsOpen()
        {
            return open;
        }

        /// <summary>
        /// Sets whether or not this Door is open and available
        /// </summary>
        /// <param name="open">Whether or not to set the Door as open</param>
        public void SetOpen(bool open)
        {
            this.open = open;
        }

        /// <summary>
        /// Gets which side of its Room this door is on
        /// </summary>
        /// <returns>Side of Room this door is on</returns>
        public Direction GetSide()
        {
            return side;
        }

        /// <summary>
        /// Sets which side of its Room this Door is on
        /// </summary>
        /// <param name="side">Side of Room to set this Door on</param>
        public void SetSide(Direction side)
        {
            this.side = side;
        }

        /// <summary>
        /// Gets the Point local to the Room where this Door is located
        /// </summary>
        /// <returns>Local point of the door, relative to its Room</returns>
        public Point2 GetLocalPoint()
        {
            return localPoint;
        }

        /// <summary>
        /// Sets the Point local to the Room where this Door is located
        /// </summary>
        /// <param name="localPoint">Local point of door, relative to its Room</param>
        public void SetLocalPoint(Point2 localPoint)
        {
            this.localPoint = localPoint;
        }

        /// <summary>
        /// Gets the Point where this Door is located globally
        /// </summary>
        /// <returns>Global point of door</returns>
        public Point2 GetGlobalPoint()
        {
            return globalPoint;
        }

        /// <summary>
        /// Sets the Point where this Door is located globally
        /// </summary>
        /// <param name="globalPoint">New global point for door</param>
        public void SetGlobalPoint(Point2 globalPoint)
        {
            this.globalPoint = globalPoint;
        }
    }
}