namespace LevelGenerator2D
{
    using UnityEngine;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.Rendering;
    using CustomUnityLibrary;

    /// <summary>
    /// The Level containing Rooms which is being generated via a controller
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class Level : MonoBehaviour
    {
        /// <summary>
        /// Size of a single grid segment in Unity units
        /// </summary>
        public const float GridSize = 10.0f;

        private const string PrimitiveRoomName = "Primitive Room";
        private const string WallShader = "Unlit/Texture";
        private const string WallsName = "Walls";
        private const string WallBlockName = "Wall";
        private static readonly float[] WallThicknesses = { 0.125f, 0.25f, 0.5f, 1.0f, 2.0f };
        private const string CloneSuffix = "(Clone)";
        private const float MinRandomWeight = 0.01f;
        private const float MaxRandomWeight = 1.0f;

        [Tooltip("Whether or not to retain Rooms after they leave the Main Camera.  This could create latency issues in larger levels if enabled.")]
        [SerializeField]
        private bool persistantRooms = true;

        [Tooltip("Texture to be used when building walls in primitive Rooms.  If no texture is provided, no walls will be built.")]
        [SerializeField]
        private Texture2D wallTexture;

        [Tooltip("Width of the walls to be used in primitive Rooms")]
        [SerializeField]
        private WallWidth wallThickness = WallWidth.Medium;

        [Tooltip("Size of the gap between walls where doors are placed in primitive Rooms")]
        [SerializeField]
        [Range(0.0f, GridSize / 2.0f)]
        private float doorSize = GridSize / 4.0f;

        private Dictionary<Point2, Room> grid = new Dictionary<Point2, Room>();
        private bool initialRoomAdded = false;
        private float wallWidth;

        void Awake()
        {
            transform.hideFlags = HideFlags.HideInInspector;
            SetWallThickness(wallThickness);
        }

        void Update()
        {
            if (!persistantRooms)
            {
                RemoveOutOfScopeRooms();
            }
        }

        void Reset()
        {
            transform.hideFlags = HideFlags.HideInInspector;
        }

        /// <summary>
        /// Gets all of the Rooms which are currently visible by the camera
        /// </summary>
        /// <param name="camera">The camera to use when looking at the rooms</param>
        /// <returns>The Rooms visible by the camera</returns>
        public Room[] GetRoomsVisibleByCamera(Camera camera = null)
        {
            if (!camera)
            {
                camera = Camera.main;
            }
            if (!camera)
            {
                Debug.LogError("Either a camera must be passed, or a camera must be tagged with \"Main Camera\"!", gameObject);
                return null;
            }
            var visiblePoints = camera.GetPointsVisibleByCamera();
            int minX = visiblePoints.x;
            int minY = visiblePoints.y;
            int maxX = visiblePoints.x + visiblePoints.width;
            int maxY = visiblePoints.y + visiblePoints.height;
            var visibleRooms = new List<Room>();
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    var gridIndex = new Point2(x, y);
                    if (grid.ContainsKey(gridIndex) && !visibleRooms.Contains(grid[gridIndex]))
                    {
                        visibleRooms.Add(grid[gridIndex]);
                    }
                }
            }
            return visibleRooms.ToArray();
        }

        /// <summary>
        /// Gets the global point of the Main Camera based on the level's grid size
        /// </summary>
        /// <returns>The point of the camera</returns>
        public static Point2 GetCameraPoint(Camera camera = null)
        {
            if (!camera)
            {
                camera = Camera.main;
            }
            var cameraPosition = camera.transform.position;
            var cameraPoint = new Point2(Mathf.RoundToInt(cameraPosition.x / GridSize), Mathf.RoundToInt(cameraPosition.y / GridSize));
            return cameraPoint;
        }

        /// <summary>
        /// Gets whether or not the rooms will be persistant, opposed to being destroyed when out of range
        /// </summary>
        /// <returns>Whether or not the rooms will be persistant</returns>
        public bool IsPersistantRooms()
        {
            return persistantRooms;
        }

        /// <summary>
        /// Sets whether or not the rooms will be persistant, opposed to being destroyed when out of range
        /// </summary>
        /// <param name="roomPersistance">Whether or not the rooms will be persistant</param>
        public void SetPersistantRooms(bool roomPersistance)
        {
            persistantRooms = roomPersistance;
        }

        /// <summary>
        /// Gets the wall texture for the Rooms
        /// </summary>
        /// <returns>The wall's texture</returns>
        public Texture2D GetWallTexture()
        {
            return wallTexture;
        }

        /// <summary>
        /// Sets the wall texture for the rooms
        /// </summary>
        /// <param name="wallTexture">The wall texture to be used</param>
        public void SetWallTexture(Texture2D wallTexture)
        {
            this.wallTexture = wallTexture;
        }

        /// <summary>
        /// Gets the wall thickness for the Rooms
        /// </summary>
        /// <returns>The wall's thickness</returns>
        public float GetWallWidth()
        {
            return wallWidth;
        }

        /// <summary>
        /// Gets the thickness to be used for walls
        /// </summary>
        /// <returns>The wall thickness</returns>
        public WallWidth GetWallThickness()
        {
            return wallThickness;
        }

        /// <summary>
        /// Sets the thickness to be used for walls
        /// </summary>
        /// <param name="wallWidth">The thickness used for walls</param>
        public void SetWallThickness(WallWidth wallThickness)
        {
            this.wallThickness = wallThickness;
            if (EnumUtility.Count<WallWidth>() != WallThicknesses.Count())
            {
                Debug.LogError("There must be an equal number of Wall Thickness options and Wall Thickness values!", gameObject);
            }
            wallWidth = WallThicknesses[(int)this.wallThickness];
        }

        /// <summary>
        /// Gets the door size for the Rooms
        /// </summary>
        /// <returns>The size of the doorways</returns>
        public float GetDoorSize()
        {
            return doorSize;
        }

        /// <summary>
        /// Changes the door size for the Rooms
        /// </summary>
        /// <param name="doorSize">The door size to use</param>
        public void SetDoorSize(float doorSize)
        {
            this.doorSize = doorSize;
        }

        /// <summary>
        /// Fills the grid with a room at its specified place and adds the room to the list of open rooms
        /// </summary>
        /// <param name="room">The Room to add to the level</param>
        public void AddRoom(Room room)
        {
            if (!room)
            {
                return;
            }
            int minX = room.GetGlobalPoint().x;
            int minY = room.GetGlobalPoint().y;
            int maxX = room.GetGlobalPoint().x + room.GetWidth() - 1;
            int maxY = room.GetGlobalPoint().y + room.GetHeight() - 1;
            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    var gridPoint = new Point2(x, y);
                    if (grid.ContainsKey(gridPoint))
                    {
                        Debug.LogError("Attempting to place " + room + " at " + gridPoint + ", but " + grid[gridPoint] + " already exists there!", room.gameObject);
                    }
                    grid.Add(gridPoint, room);
                }
            }
            CloseDoors(room);
            room.transform.parent = transform;
            room.BuildWalls();
            if (initialRoomAdded && !HasValidDoor(room))
            {
                Debug.LogWarning(room.name + " has been added to the Level, but is not connected to any other Rooms.  Although likely, a path to this Room is not guaranteed.", room.gameObject);
            }
            initialRoomAdded = true;
        }

        /// <summary>
        /// Fills the grid with rooms at their specified places and adds the rooms to the list of open rooms
        /// </summary>
        /// <param name="rooms">The Rooms to add to the level</param>
        public void AddRooms(Room[] rooms)
        {
            if (rooms == null)
            {
                return;
            }
            foreach (var room in rooms)
            {
                AddRoom(room);
            }
        }

        /// <summary>
        /// Removes a Room from the level and destroys it
        /// </summary>
        /// <param name="room">The room to destroy</param>
        public void DestroyRoom(Room room)
        {
            if (!room)
            {
                return;
            }
            foreach (var door in room.GetDoors())
            {
                var neighborPoint = door.GetNeighborPoint();
                var neighborDoor = GetDoors(neighborPoint).Where(d => d.PairsWith(door)).FirstOrDefault();
                if (neighborDoor != null)
                {
                    neighborDoor.SetOpen(true);
                }
            }
            for (int x = room.GetGlobalPoint().x; x < room.GetGlobalPoint().x + room.GetWidth(); x++)
            {
                for (int y = room.GetGlobalPoint().y; y < room.GetGlobalPoint().y + room.GetWidth(); y++)
                {
                    grid.Remove(new Point2(x, y));
                }
            }
            Destroy(room.gameObject);
        }

        /// <summary>
        /// Removes Rooms from the level and destroy them
        /// </summary>
        /// <param name="rooms">The Rooms to destroy</param>
        public void DestroyRooms(Room[] rooms)
        {
            if (rooms == null || rooms.Length == 0)
            {
                return;
            }
            foreach (var room in rooms)
            {
                DestroyRoom(room);
            }
        }

        /// <summary>
        /// Adds neighbors at all of the Room's doors, such that each neighbor will not have any open doors
        /// </summary>
        /// <param name="room">The room whose neighbors will be added</param>
        /// <returns>The neighbors that were added to the room</returns>
        public Room[] CloseRoom(Room room)
        {
            var neighborsAdded = new List<Room>();
            foreach (var door in room.GetDoors())
            {
                if (!grid.ContainsKey(door.GetNeighborPoint()))
                {
                    var neighborAdded = AddPrimitiveNeighbor(door);
                    neighborsAdded.Add(neighborAdded);
                }
            }
            return neighborsAdded.ToArray();
        }

        /// <summary>
        /// Adds neighbors at all of the Rooms' doors, such that each neighbor will not have any open doors
        /// </summary>
        /// <param name="rooms">The rooms whose neighbors will be added</param>
        /// <returns>The neighbors that were added to the Rooms</returns>
        public Room[] CloseRooms(Room[] rooms)
        {
            if (rooms == null || rooms.Length == 0)
            {
                return new Room[0];
            }
            var closedRooms = new List<Room>();
            foreach (var room in rooms)
            {
                closedRooms.AddRange(CloseRoom(room));
            }
            return closedRooms.ToArray();
        }

        /// <summary>
        /// Gets a copy of the list of the Level's open rooms, where an open room is a room with a door, but no attached neighbor to that door
        /// </summary>
        /// <returns>Array of all open rooms</returns>
        public Room[] GetOpenRooms()
        {
            return grid.Values.Distinct().Where(c => c.GetDoors().Where(d => d.IsOpen()).Count() > 0).ToArray();
        }

        /// <summary>
        /// Gets a copy of the list of the Level's open rooms within the left, bottom, right, and top bounds
        /// </summary>
        /// <param name="x0">left bound</param>
        /// <param name="y0">top bound</param>
        /// <param name="x1">right bound</param>
        /// <param name="y1">bottom bound</param>
        /// <returns>Open rooms</returns>
        public Room[] GetOpenRooms(int x0, int y0, int x1, int y1)
        {
            if (x0 > x1)
            {
                int tempX = x0;
                x0 = x1;
                x1 = tempX;
            }
            if (y0 > y1)
            {
                int tempY = y0;
                y0 = y1;
                y1 = tempY;
            }
            return GetOpenRooms().Where(c => c.GetGlobalPoint().x + c.GetWidth() > x0 && c.GetGlobalPoint().x <= x1 && c.GetGlobalPoint().y + c.GetHeight() > y0 && c.GetGlobalPoint().y <= y1).ToArray();
        }

        /// <summary>
        /// Gets a copy of all of the Level's Rooms
        /// </summary>
        /// <returns>The level's rooms</returns>
        public Room[] GetRooms()
        {
            return grid.Values.Distinct().ToArray();
        }

        /// <summary>
        /// Gets the room at the global point, or returns null if there is none.
        /// </summary>
        /// <param name="globalPoint">The point to get the room from</param>
        /// <returns>The Room at the specified point</returns>
        public Room GetRoom(Point2 globalPoint)
        {
            if (!grid.ContainsKey(globalPoint))
            {
                return null;
            }
            return grid[globalPoint];
        }

        /// <summary>
        /// Gets a copy of the list of the Level's rooms within the left, bottom, right, and top bounds
        /// </summary>
        /// <param name="x0">left bound</param>
        /// <param name="y0">top bound</param>
        /// <param name="x1">right bound</param>
        /// <param name="y1">bottom bound</param>
        /// <returns>All Rooms within bounds</returns>
        public Room[] GetRooms(int x0, int y0, int x1, int y1)
        {
            if (x0 > x1)
            {
                int tempX = x0;
                x0 = x1;
                x1 = tempX;
            }
            if (y0 > y1)
            {
                int tempY = y0;
                y0 = y1;
                y1 = tempY;
            }

            var roomsInRangeIEnumerable = grid.Where(c => c.Key.x + c.Value.GetWidth() > x0 && c.Key.x <= x1 && c.Key.y + c.Value.GetHeight() > y0 && c.Key.y <= y1);
            var roomsInRange = roomsInRangeIEnumerable.Select(lc => lc.Value).Distinct().ToArray();
            return roomsInRange;
        }

        /// <summary>
        /// Adds random neighbors to all open Rooms within range, where an open Room is a Room with a Door that does not have a connecting Room.
        /// Neighbors must be loaded from the Resources folder.
        /// To set wallBlocks to false without prioritizing file paths, set prioritizedFilePaths to null and wallBlocks to true.
        /// When passing larger sections, wallBlocks can lead towards more linear paths.
        /// </summary>
        /// <param name="x0">Minimum x to where a neighbor can be</param>
        /// <param name="y0">Minimum y to where a neighbor can be</param>
        /// <param name="x1">Maximum x to where a neighbor can be</param>
        /// <param name="y1">Maximum y to where a neighbor can be</param>
        /// <param name="prioritizedFilePaths">File paths, in order or preference, that will be used to find the Rooms to use</param>
        /// <param name="wallBlocks">Whether or not to create rooms of walls in areas within the specified coordinates if no other room can be reached or fit.  A wall texture mustbe selected in order for wall blocks to be created.</param>
        /// <returns>The Rooms which were created</returns>
        public Room[] AddNeighbors(int x0, int y0, int x1, int y1, string[] prioritizedFilePaths = null, bool wallBlocks = true)
        {
            var neighborsAdded = new List<Room>();
            var openRooms = GetOpenRooms(x0, y0, x1, y1);
            while (openRooms.Length > 0)
            {
                openRooms = GetOpenRooms(x0, y0, x1, y1);
                foreach (var room in openRooms)
                {
                    var roomNeighborsAdded = AddNeighbors(room, prioritizedFilePaths);
                    neighborsAdded.AddRange(roomNeighborsAdded);
                }
            }
            if (wallTexture && wallBlocks)
            {
                for (int x = x0; x <= x1; x++)
                {
                    for (int y = y0; y <= y1; y++)
                    {
                        var wallPoint = new Point2(x, y);
                        if (!grid.ContainsKey(wallPoint))
                        {
                            CreateWallBlock(wallPoint);
                        }
                    }
                }
            }
            return neighborsAdded.ToArray();
        }

        /// <summary>
        /// Adds random neighbors surrounding the room.
        /// Neighbors must be loaded from the Resources folder.
        /// </summary>
        /// <param name="room">Room to which neighbors will be added</param>
        /// <param name="prioritizedFilePaths">File paths, in order or preference, that will be used to find the Rooms to use</param>
        /// <returns>Neighboring Rooms which were added</returns>
        public Room[] AddNeighbors(Room room, string[] prioritizedFilePaths = null)
        {
            if (!room)
            {
                return null;
            }
            else if (room.GetDoors().Where(d => d.IsOpen()).Count() == 0)
            {
                return new Room[0];
            }
            if (prioritizedFilePaths == null || prioritizedFilePaths.Length == 0)
            {
                prioritizedFilePaths = new string[] { "" };
            }
            var neighborsAdded = new List<Room>();
            foreach (var roomDoor in room.GetDoors())
            {
                if (!roomDoor.IsOpen())
                {
                    continue;
                }
                Room neighborAdded = null;
                foreach (var filePath in prioritizedFilePaths)
                {
                    var potentialNeighbors = Resources.LoadAll<Room>(filePath);
                    potentialNeighbors = potentialNeighbors.OrderBy(r => r.GetWeight() / Random.Range(MinRandomWeight, MaxRandomWeight)).ToArray();
                    neighborAdded = AddNeighbor(roomDoor, potentialNeighbors);
                    if (neighborAdded)
                    {
                        neighborsAdded.Add(neighborAdded);
                        break;
                    }
                }
                if (!neighborAdded && !grid.ContainsKey(roomDoor.GetNeighborPoint()))
                {
                    neighborAdded = AddPrimitiveNeighbor(roomDoor, false);
                    if (neighborAdded)
                    {
                        neighborsAdded.Add(neighborAdded);
                        break;
                    }
                    else
                    {
                        Debug.LogError("Failed to find primitive neighbor for " + room.name + " at Door #" + roomDoor.GetIndex() + "!", room.gameObject);
                    }
                }
            }
            return neighborsAdded.ToArray();
        }

        /// <summary>
        /// Create a giant wall for a closed off room
        /// </summary>
        /// <param name="point">The global point of where to create the closed off room</param>
        private void CreateWallBlock(Point2 point)
        {
            var westPoint = new Point2(point.x - 1, point.y);
            var northPoint = new Point2(point.x, point.y + 1);
            var eastPoint = new Point2(point.x + 1, point.y);
            var southPoint = new Point2(point.x, point.y - 1);
            var westDoor = GetDoors(westPoint).Where(d => d.GetSide() == Direction.East).FirstOrDefault();
            if (westDoor != null)
            {
                AddPrimitiveNeighbor(westDoor);
                return;
            }
            var northDoor = GetDoors(northPoint).Where(d => d.GetSide() == Direction.South).FirstOrDefault();
            if (northDoor != null)
            {
                AddPrimitiveNeighbor(northDoor);
                return;
            }
            var eastDoor = GetDoors(eastPoint).Where(d => d.GetSide() == Direction.West).FirstOrDefault();
            if (eastDoor != null)
            {
                AddPrimitiveNeighbor(eastDoor);
                return;
            }
            var southDoor = GetDoors(southPoint).Where(d => d.GetSide() == Direction.North).FirstOrDefault();
            if (southDoor != null)
            {
                AddPrimitiveNeighbor(southDoor);
                return;
            }
            var wallBlock = GameObject.CreatePrimitive(PrimitiveType.Quad);
            wallBlock.name = WallBlockName;
            wallBlock.transform.SetParent(transform);
            Destroy(wallBlock.GetComponent<MeshCollider>());
            var wallRenderer = wallBlock.GetComponent<MeshRenderer>();
            wallRenderer.shadowCastingMode = ShadowCastingMode.Off;
            wallRenderer.receiveShadows = false;
#if UNITY_5_4_OR_NEWER
            wallRenderer.lightProbeUsage = LightProbeUsage.Off;
#else
            wallRenderer.useLightProbes = false;
#endif
            wallRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            var wallMaterial = new Material(Shader.Find(WallShader));
            wallMaterial.mainTexture = wallTexture;
            var scale = new Vector2(GridSize, GridSize);
            wallMaterial.mainTextureScale = scale / GetWallWidth();
            wallBlock.transform.localScale = scale;
            wallBlock.GetComponent<MeshRenderer>().sharedMaterial = wallMaterial;
            var existingRoom = GetRoom(point);
            if (existingRoom)
            {
                Debug.LogError(wallBlock.name + " has failed to place at " + point + ", because " + existingRoom.name + " already exists there!", existingRoom.gameObject);
                return;
            }
            var wallChunk = wallBlock.AddComponent<Room>();
            bool[] doorCombination = GetPrimitiveDoorCombination(point);
            wallChunk.SetDoors(doorCombination);
            wallChunk.SetGlobalPoint(point);
            var gridPoint = wallChunk.GetGlobalPoint();
            if (grid.ContainsKey(gridPoint))
            {
                Debug.LogError("Attempting to place " + wallChunk + " at " + gridPoint + ", but " + grid[gridPoint] + " already exists there!", wallChunk.gameObject);
            }
            grid.Add(gridPoint, wallChunk);
        }

        /// <summary>
        /// Add a 1x1 neighor to a door
        /// </summary>
        /// <param name="door">The door to add the neighbor at</param>
        /// <param name="closedRoom">Whether or not to disallow other doors to be on the primitive room</param>
        /// <returns>The primitive Room which was added</returns>
        private Room AddPrimitiveNeighbor(Door door, bool closedRoom = true)
        {
            var primitivePoint = door.GetNeighborPoint();
            var existingRoom = GetRoom(primitivePoint);
            if (existingRoom)
            {
                Debug.LogError(PrimitiveRoomName + " has failed to place at " + primitivePoint + ", because " + existingRoom.name + " already exists there!", existingRoom.gameObject);
                return null;
            }
            var primitiveRoom = new GameObject(PrimitiveRoomName).AddComponent<Room>();
            bool[] doorCombination = GetPrimitiveDoorCombination(primitivePoint, closedRoom);
            primitiveRoom.SetDoors(doorCombination);
            primitiveRoom.SetGlobalPoint(primitivePoint);
            AddPrimitiveRoom(primitiveRoom);
            return primitiveRoom;
        }

        /// <summary>
        /// Add a 1x1 room at its existing point
        /// </summary>
        /// <param name="primitiveRoom">The Room to add</param>
        private void AddPrimitiveRoom(Room primitiveRoom)
        {
            if (!primitiveRoom)
            {
                return;
            }
            primitiveRoom.transform.parent = transform;
            var gridPoint = primitiveRoom.GetGlobalPoint();
            if (grid.ContainsKey(gridPoint))
            {
                Debug.LogError("Attempting to place " + primitiveRoom + " at " + gridPoint + ", but " + grid[gridPoint] + " already exists there!", primitiveRoom.gameObject);
            }
            grid.Add(gridPoint, primitiveRoom);
            CloseDoors(primitiveRoom);
            primitiveRoom.BuildWalls();
        }

        /// <summary>
        /// The bools of whether or not a door belongs at each index
        /// </summary>
        /// <param name="primitivePoint">The point to place the room at</param>
        /// <param name="closedRoom">Whether or not the minimum number of doors will be used</param>
        /// <returns>The array of indexed bools</returns>
        private bool[] GetPrimitiveDoorCombination(Point2 primitivePoint, bool closedRoom = true)
        {
            bool[] doorCombination = new bool[4] { false, false, false, false };
            var westNeighborPoint = new Point2(primitivePoint.x - 1, primitivePoint.y);
            if (GetRoom(westNeighborPoint))
            {
                if (GetDoors(westNeighborPoint).Where(d => d.GetSide() == Direction.East).Count() > 0)
                {
                    doorCombination[0] = true;
                }
            }
            else
            {
                doorCombination[0] = !closedRoom;
            }
            var northNeighborPoint = new Point2(primitivePoint.x, primitivePoint.y + 1);
            if (GetRoom(northNeighborPoint))
            {
                if (GetDoors(northNeighborPoint).Where(d => d.GetSide() == Direction.South).Count() > 0)
                {
                    doorCombination[1] = true;
                }
            }
            else
            {
                doorCombination[1] = !closedRoom;
            }
            var eastNeighborPoint = new Point2(primitivePoint.x + 1, primitivePoint.y);
            if (GetRoom(eastNeighborPoint))
            {
                if (GetDoors(eastNeighborPoint).Where(d => d.GetSide() == Direction.West).Count() > 0)
                {
                    doorCombination[2] = true;
                }
            }
            else
            {
                doorCombination[2] = !closedRoom;
            }
            var southNeighborPoint = new Point2(primitivePoint.x, primitivePoint.y - 1);
            if (GetRoom(southNeighborPoint))
            {
                if (GetDoors(southNeighborPoint).Where(d => d.GetSide() == Direction.North).Count() > 0)
                {
                    doorCombination[3] = true;
                }
            }
            else
            {
                doorCombination[3] = !closedRoom;
            }
            return doorCombination;
        }

        /// <summary>
        /// Removes all rooms outside the view of the main camera
        /// </summary>
        private void RemoveOutOfScopeRooms()
        {
            var visiblePoints = Camera.main.GetPointsVisibleByCamera();
            int minX = visiblePoints.xMin - Room.MaxWidth - 1;
            int minY = visiblePoints.yMin - Room.MaxHeight - 1;
            int maxX = visiblePoints.xMax + Room.MaxWidth + 1;
            int maxY = visiblePoints.yMax + Room.MaxHeight + 1;
            var roomsToRemove = grid.Values.Where(c => c.GetGlobalPoint().x < minX || c.GetGlobalPoint().x > maxX || c.GetGlobalPoint().y < minY || c.GetGlobalPoint().y > maxY).ToArray();
            DestroyRooms(roomsToRemove);
        }

        /// <summary>
        /// Updates whether or not the doors are open or closed based on attached rooms
        /// </summary>
        /// <param name="room">The room whose doors to check</param>
        private void CloseDoors(Room room)
        {
            if (!room)
            {
                return;
            }
            room.UpdateDoors();
            var doors = room.GetDoors();
            if (doors == null || doors.Length == 0)
            {
                return;
            }
            foreach (var door in doors)
            {
                var neighborDoorPoint = door.GetNeighborPoint();
                var neighbor = GetRoom(neighborDoorPoint);
                if (neighbor)
                {
                    neighbor.UpdateDoors();
                }
                var neighborDoor = GetDoors(neighborDoorPoint).Where(d => d.PairsWith(door)).FirstOrDefault();
                if (neighborDoor != null)
                {
                    neighborDoor.SetOpen(false);
                    door.SetOpen(false);
                }
            }
        }

        /// <summary>
        /// All the doors at a local point
        /// </summary>
        /// <param name="point">The room's local point</param>
        /// <returns>An array of the doors attached to the room at the specified point</returns>
        private Door[] GetDoors(Point2 point)
        {
            var doorList = new List<Door>();
            if (grid.ContainsKey(point))
            {
                var room = grid[point];
                var doors = room.GetDoors();
                foreach (var door in doors)
                {
                    if (door.GetGlobalPoint() == point)
                    {
                        doorList.Add(door);
                    }
                }
            }
            return doorList.ToArray();
        }

        /// <summary>
        /// Adds a room attached to a particular door from a list of room prefabs
        /// </summary>
        /// <param name="roomDoor">The door to attach the room to</param>
        /// <param name="potentialNeighbors">The potential prefabs which can be used</param>
        /// <returns>The newly created and attached room</returns>
        private Room AddNeighbor(Door roomDoor, Room[] potentialNeighbors)
        {
            foreach (var neighbor in potentialNeighbors)
            {
                foreach (var neighborDoor in neighbor.GetDoors())
                {
                    Room neighborAdded = AddNeighbor(roomDoor, neighbor, neighborDoor);
                    if (neighborAdded)
                    {
                        return neighborAdded;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Adds a specified room to a specified door
        /// </summary>
        /// <param name="roomDoor">The existing door to attach the new Room to</param>
        /// <param name="neighbor">The new room to attach</param>
        /// <param name="neighborDoor">The new room's door used to attach to the existing room's door</param>
        /// <returns></returns>
        private Room AddNeighbor(Door roomDoor, Room neighbor, Door neighborDoor)
        {
            var neighborPoint = roomDoor.GetNeighborPoint();
            neighborPoint -= neighborDoor.GetLocalPoint();
            var roomCreated = CreateRoom(neighbor, neighborPoint);
            return roomCreated;
        }

        /// <summary>
        /// Creates a room from a prefab at a specified point
        /// </summary>
        /// <param name="room">The room to be created</param>
        /// <param name="point">The global point to place the room at</param>
        /// <returns>The Room which was created</returns>
        private Room CreateRoom(Room room, Point2 point)
        {
            if (!IsValidPlacement(room, point))
            {
                return null;
            }
            room = Instantiate(room, Vector2.zero, Quaternion.identity) as Room;
            room.name = room.name.TrimEnd(CloneSuffix);
            room.transform.SetParent(transform);
            room.SetGlobalPoint(point);
            AddRoom(room);
            return room;
        }

        /// <summary>
        /// Checks if a room can be placed at a specified point
        /// </summary>
        /// <param name="room">The Room to check</param>
        /// <param name="point">The potential point for the room</param>
        /// <returns>Whether or not it is a valid placement</returns>
        private bool IsValidPlacement(Room room, Point2 point)
        {
            return !CollidesWithExistingRooms(room, point) && DoorsMatch(room, point);
        }

        /// <summary>
        /// Checks if a room collides with other rooms if placed at a specified point
        /// </summary>
        /// <param name="room">The room to check for collisions with</param>
        /// <param name="point">The point to check for the room placement at</param>
        /// <returns>Whether or not it is a valid room placement</returns>
        private bool CollidesWithExistingRooms(Room room, Point2 point)
        {
            for (int x = point.x; x < point.x + room.GetWidth(); x++)
            {
                for (int y = point.y; y < point.y + room.GetHeight(); y++)
                {
                    if (grid.ContainsKey(new Point2(x, y)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if a room has a valid door that it can be attached to
        /// </summary>
        /// <param name="room">The room to check</param>
        /// <returns>Whether or not it has a valid door</returns>
        private bool HasValidDoor(Room room)
        {
            foreach (var door in room.GetDoors())
            {
                var neighborPoint = door.GetNeighborPoint();
                switch (door.GetSide())
                {
                    case Direction.West:
                        if (GetDoors(neighborPoint).Where(d => d.GetSide() == Direction.East).Count() > 0)
                        {
                            return true;
                        }
                        break;
                    case Direction.North:
                        if (GetDoors(neighborPoint).Where(d => d.GetSide() == Direction.South).Count() > 0)
                        {
                            return true;
                        }
                        break;
                    case Direction.East:
                        if (GetDoors(neighborPoint).Where(d => d.GetSide() == Direction.West).Count() > 0)
                        {
                            return true;
                        }
                        break;
                    case Direction.South:
                        if (GetDoors(neighborPoint).Where(d => d.GetSide() == Direction.North).Count() > 0)
                        {
                            return true;
                        }
                        break;
                    default:
                        Debug.LogError("Invalid Door Direction (" + door.GetSide() + " received!", gameObject);
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the neighboring doors would match for a specified room at a specified point
        /// </summary>
        /// <param name="room">The room to check</param>
        /// <param name="point">The global point where the room would be</param>
        /// <returns>Whether or not the doors would match</returns>
        private bool DoorsMatch(Room room, Point2 point)
        {
            int minX = point.x;
            int minY = point.y;
            int maxX = point.x + room.GetWidth() - 1;
            int maxY = point.y + room.GetHeight() - 1;
            for (int y = minY; y <= maxY; y++)
            {
                int x = minX;
                var homePoint = new Point2(x, y);
                var neighborPoint = new Point2(x - 1, y);
                if (!grid.ContainsKey(neighborPoint))
                {
                    continue;
                }
                var localDoor = room.GetDoors(homePoint - point).Where(d => d.GetSide() == Direction.West).FirstOrDefault();
                var neighborDoor = GetDoors(neighborPoint).Where(d => d.GetSide() == Direction.East).FirstOrDefault();
                if ((localDoor == null) != (neighborDoor == null))
                {
                    return false;
                }
            }
            for (int x = minX; x <= maxX; x++)
            {
                int y = maxY;
                var homePoint = new Point2(x, y);
                var neighborPoint = new Point2(x, y + 1);
                if (!grid.ContainsKey(neighborPoint))
                {
                    continue;
                }
                var localDoor = room.GetDoors(homePoint - point).Where(d => d.GetSide() == Direction.North).FirstOrDefault();
                var neighborDoor = GetDoors(neighborPoint).Where(d => d.GetSide() == Direction.South).FirstOrDefault();
                if ((localDoor == null) != (neighborDoor == null))
                {
                    return false;
                }
            }
            for (int y = maxY; y >= minY; y--)
            {
                int x = maxX;
                var homePoint = new Point2(x, y);
                var neighborPoint = new Point2(x + 1, y);
                if (!grid.ContainsKey(neighborPoint))
                {
                    continue;
                }
                var localDoor = room.GetDoors(homePoint - point).Where(d => d.GetSide() == Direction.East).FirstOrDefault();
                var neighborDoor = GetDoors(neighborPoint).Where(d => d.GetSide() == Direction.West).FirstOrDefault();
                if ((localDoor == null) != (neighborDoor == null))
                {
                    return false;
                }
            }
            for (int x = maxX; x >= minX; x--)
            {
                int y = minY;
                var homePoint = new Point2(x, y);
                var neighborPoint = new Point2(x, y - 1);
                if (!grid.ContainsKey(neighborPoint))
                {
                    continue;
                }
                var localDoor = room.GetDoors(homePoint - point).Where(d => d.GetSide() == Direction.South).FirstOrDefault();
                var neighborDoor = GetDoors(neighborPoint).Where(d => d.GetSide() == Direction.North).FirstOrDefault();
                if ((localDoor == null) != (neighborDoor == null))
                {
                    return false;
                }
            }
            return true;
        }
    }
}