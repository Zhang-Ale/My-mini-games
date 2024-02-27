///Uncomment the line below if you are using Creative Spore's Super Tilemap Editor
//#define USING_SUPER_TILEMAP_EDITOR

namespace LevelGenerator2D
{
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.Rendering;
    using CustomUnityLibrary;
#if USING_SUPER_TILEMAP_EDITOR
    using CreativeSpore.SuperTilemapEditor;
#endif

    /// <summary>
    /// A predefined segment of a level to be used in the Level
    /// </summary>
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class Room : MonoBehaviour
    {
        /// <summary>
        /// Maximum width of a room in grid units.  If Max Width is used in your level controller, the Max Width should be kept as low as possible to improve performance.
        /// </summary>
        public const int MaxWidth = 10;

        /// <summary>
        /// Maximum height of a room in grid units.  If Max Height is used in your level controller, the Max Height should be kept as low as possible to improve performance.
        /// </summary>
        public const int MaxHeight = 10;

        private const string LevelName = "Level";
        private const string DoorNamePrefix = "Door #";
        private const string WallName = "Wall";
        private const string RoomName = "Room";
        private const string WallShader = "Unlit/Texture";
        private const string WallMaterialsSubFolder = "Materials/Walls/";
        private const string MaterialSuffix = ".mat";
        private const int PixelsPerTexture = 100;
        private readonly Color OutlineColor = Color.red;
        private readonly Color InlineColor = Color.blue;
        private readonly Color WallColor = Color.white;

        [Tooltip("Global position of Room within grid, where this is the top left section of the Room.  This should be used to adjust the position of the Room.")]
        [SerializeField]
        private Point2 globalPoint;

        [Tooltip("Width of Room in Cells")]
        [SerializeField]
        [Range(1, MaxWidth)]
        private int width = 1;

        [Tooltip("Height of Room in Cells")]
        [SerializeField]
        [Range(1, MaxHeight)]
        private int height = 1;

        [Tooltip("Whether or not to lock children within the Room")]
        [SerializeField]
        private bool lockChildren = true;

        [Tooltip("Link between Rooms.  The Doors rotate clockwise around the Room starting at the bottom left corner.")]
        [SerializeField]
        private Door[] doors = new Door[1];

        [Tooltip("How often the Room will spawn.")]
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float weight = 0.5f;

        private Level level;

        void Awake()
        {
            transform.hideFlags = HideFlags.HideInInspector;
            SetLevelUsedForWalls();
            InitializeDoors();
        }

        void OnValidate()
        {
            UpdatePosition();
            InitializeDoors();
        }

        void Reset()
        {
            transform.hideFlags = HideFlags.HideInInspector;
        }

        void OnDrawGizmos()
        {
            DrawDoors();
            DrawOutline();
        }

        void Update()
        {
            UpdatePosition();
            if (!Application.isPlaying && GetChildlock())
            {
                EncompassChildren();
            }
        }

        /// <summary>
        /// Updates the global point for all doors
        /// </summary>
        public void UpdateDoors()
        {
            foreach (var door in doors)
            {
                door.SetSide(GetSide(door));
                switch (door.GetSide())
                {
                    case Direction.West:
                        door.SetLocalPoint(new Point2(0, door.GetIndex()));
                        break;
                    case Direction.North:
                        door.SetLocalPoint(new Point2(door.GetIndex() - height, height - 1));
                        break;
                    case Direction.East:
                        door.SetLocalPoint(new Point2(width - 1, height - 1 - door.GetIndex() + height + width));
                        break;
                    case Direction.South:
                        door.SetLocalPoint(new Point2(width - 1 - door.GetIndex() + height * 2 + width, 0));
                        break;
                    default:
                        Debug.LogError("Invalid Door Direction (" + door.GetSide() + " received!", gameObject);
                        break;
                }
                door.SetGlobalPoint(globalPoint + door.GetLocalPoint());
            }
        }

        /// <summary>
        /// Takes in the local Point of the Room and returns its doors
        /// </summary>
        /// <param name="localPoint">Point to get the door from</param>
        /// <returns>The door at the specified point</returns>
        public Door[] GetDoors(Point2 localPoint)
        {
            var doorList = new List<Door>();
            foreach (var door in doors)
            {
                if (door.GetLocalPoint() == localPoint)
                {
                    doorList.Add(door);
                }
            }
            return doorList.ToArray();
        }

        /// <summary>
        /// Gets the max number of doors that can fit in this room based on its size
        /// </summary>
        /// <returns>Max door count</returns>
        public int GetMaxDoors()
        {
            return width * 2 + height * 2;
        }

        /// <summary>
        /// Gets whether or not the children are to be locked within the room
        /// </summary>
        /// <returns>If children are locked</returns>
        public bool GetChildlock()
        {
            return lockChildren;
        }

        /// <summary>
        /// Sets whether or not the children will be locked within the room
        /// </summary>
        /// <param name="childlock">Whether or not to lock the children</param>
        public void SetChildlock(bool childlock)
        {
            lockChildren = childlock;
            if (!Application.isPlaying && lockChildren)
            {
                EncompassChildren();
            }
        }

        /// <summary>
        /// Removes a door with the specified index
        /// </summary>
        /// <param name="index">Index for Door to be removed</param>
        public void RemoveDoor(int index)
        {
            if (index < 0 || index >= doors.Length)
            {
                Debug.LogError("Cannot remove a door at an index outside of the range of existing doors!", gameObject);
                return;
            }
            doors[index] = null;
            doors = doors.Where(d => d != null).ToArray();
        }

        /// <summary>
        /// Gets the Point where this Room is located globally
        /// </summary>
        /// <returns>Global point of the room</returns>
        public Point2 GetGlobalPoint()
        {
            return globalPoint;
        }

        /// <summary>
        /// Sets the Point where this Room is located globally
        /// </summary>
        /// <param name="globalPoint">Global point to move the room to</param>
        public void SetGlobalPoint(Point2 globalPoint)
        {
            this.globalPoint = globalPoint;
            UpdatePosition();
        }

        /// <summary>
        /// Gets the width in grid units
        /// </summary>
        /// <returns>The width of the level</returns>
        public int GetWidth()
        {
            return width;
        }

        /// <summary>
        /// Sets the width of this room in grid units
        /// </summary>
        /// <param name="width">The room's new width</param>
        public void SetWidth(int width)
        {
            this.width = width;
        }

        /// <summary>
        /// Gets the weight of this room, determining how often it is spawned
        /// </summary>
        /// <returns>Weight of room</returns>
        public float GetWeight()
        {
            return weight;
        }

        /// <summary>
        /// Sets the weight for this room, determining how often it is spawned
        /// </summary>
        /// <param name="weight">Room weight</param>
        public void SetWeight(float weight)
        {
            this.weight = weight;
        }

        /// <summary>
        /// Gets the height in grid units
        /// </summary>
        /// <returns>The height of the level</returns>
        public int GetHeight()
        {
            return height;
        }

        /// <summary>
        /// Sets the height of this room in grid units
        /// </summary>
        /// <param name="height">The room's new height</param>
        public void SetHeight(int height)
        {
            this.height = height;
        }

        /// <summary>
        /// Gets the doors associated with this Room
        /// </summary>
        /// <returns>The level's doors</returns>
        public Door[] GetDoors()
        {
            return doors;
        }

        /// <summary>
        /// Sets the doors for the room.
        /// All null doors will be initialized.
        /// </summary>
        /// <param name="doors">The new doors to use</param>
        public void SetDoors(Door[] doors)
        {
            this.doors = doors;
            InitializeDoors();
        }

        /// <summary>
        /// Sets the doors in the indexed order
        /// </summary>
        /// <param name="doorCombination">The combination of doors</param>
        public void SetDoors(bool[] doorCombination)
        {
            int totalDoors = height * 2 + width * 2;
            if (totalDoors != doorCombination.Length)
            {
                Debug.LogWarning("Attempting to set the doors with " + doorCombination.Length + " doors, but " + name + " has " + totalDoors + " doors!  Set the size of " + name + " before attempting to set its doors!", gameObject);
                return;
            }
            int numDoors = doorCombination.Where(d => d).Count();
            doors = new Door[numDoors];
            int currentIndex = 0;
            for (int i = 0; i < doorCombination.Length; i++)
            {
                if (doorCombination[i])
                {
                    doors[currentIndex] = new Door(DoorNamePrefix + " " + i, i);
                    currentIndex++;
                }
            }
        }

        /// <summary>
        /// Checks whether or not this room has any open doors
        /// </summary>
        /// <returns>Whether or not this room is open</returns>
        public bool IsOpen()
        {
            foreach (var door in GetDoors())
            {
                if (door.IsOpen())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Builds the walls for this Room
        /// </summary>
        public void BuildWalls()
        {
            if (!level)
            {
                SetLevelUsedForWalls();
            }
            if (!level.GetWallTexture() || level.GetWallWidth() == 0)
            {
                return;
            }
            if (level.GetWallTexture().wrapMode != TextureWrapMode.Repeat)
            {
                Debug.LogWarning("The Wrap Mode of " + level.GetWallTexture().name + " must be Repeat, or else tiling issues may occur.", level.GetWallTexture());
            }
            var vertices = CalculateWallVertices();
            for (int i = 0; i < vertices.Length - 1; i += 2)
            {
                var wall = InstantiateWall();
                wall.transform.SetParent(transform);
                wall.transform.position = vertices[i] + (vertices[i + 1] - vertices[i]) / 2.0f;
                var wallMaterial = new Material(Shader.Find(WallShader));
                wallMaterial.mainTexture = level.GetWallTexture();
                var scale = vertices[i + 1] - vertices[i];
                scale = new Vector2(Mathf.Abs(scale.x), Mathf.Abs(scale.y));
                scale += new Vector2(level.GetWallWidth(), level.GetWallWidth());
                wallMaterial.mainTextureScale = scale / level.GetWallWidth();
                wall.transform.localScale = scale;
                wall.GetComponent<MeshRenderer>().sharedMaterial = wallMaterial;
#if UNITY_5_4_OR_NEWER
                var wallCollider = wall.AddComponent<BoxCollider2D>();
                wallCollider.size = Vector2.one;
#else
                wall.AddComponent<BoxCollider2D>();
#endif
            }
        }

        /// <summary>
        /// Update the room's transform based on its global point
        /// </summary>
        public void UpdatePosition()
        {
            var position = globalPoint * Level.GridSize;
            position.x += (width / 2.0f - 0.5f) * Level.GridSize;
            position.y += (height / 2.0f - 0.5f) * Level.GridSize;
            transform.position = position;
        }

        /// <summary>
        /// Set up the doors so that only the correct indices are set
        /// </summary>
        public void InitializeDoors()
        {
            int maxDoors = GetMaxDoors();
            if (doors.Length == 0)
            {
                doors = new Door[1];
            }
            if (doors.Length > maxDoors)
            {
                Array.Resize(ref doors, maxDoors);
            }
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i] == null)
                {
                    doors[i] = new Door(DoorNamePrefix + " " + i, i);
                }
            }
            int numReorderAttempts = doors.Length * doors.Length;
            do
            {
                for (int i = 0; i < doors.Length; i++)
                {
                    bool doorBehind = i > 0;
                    bool doorAhead = i + 1 < doors.Length;
                    if (doorAhead && doors[i].GetIndex() >= doors[i + 1].GetIndex())
                    {
                        if (doors[i + 1].GetIndex() == 0)
                        {
                            doors[i + 1].SetIndex(doors[i + 1].GetIndex() + 1);
                        }
                        else
                        {
                            doors[i].SetIndex(doors[i + 1].GetIndex() - 1);
                        }
                        if (doors[i].GetIndex() < 0)
                        {
                            doors[i].SetIndex(0);
                        }
                        if (doorBehind && doors[i - 1].GetIndex() == doors[i].GetIndex())
                        {
                            doors[i].SetIndex((doors[i].GetIndex() + 1));
                        }
                    }
                    doors[i].SetIndex(Math.Min(Math.Max(0, doors[i].GetIndex()), maxDoors - 1));
                }
                for (int i = 0; i < doors.Length; i++)
                {
                    bool doorBehind = i > 0;
                    if (doorBehind && doors[i].GetIndex() <= doors[i - 1].GetIndex())
                    {
                        if (doors[i - 1].GetIndex() >= maxDoors - 1)
                        {
                            doors[i - 1].SetIndex(doors[i - 1].GetIndex() - 1);
                        }
                        else
                        {
                            doors[i].SetIndex((doors[i - 1].GetIndex() + 1));
                        }
                    }
                    doors[i].SetIndex(Math.Min(Math.Max(0, doors[i].GetIndex()), maxDoors - 1));
                }
                foreach (var door in doors)
                {
                    door.SetName(DoorNamePrefix + door.GetIndex());
                    door.SetOpen(true);
                }
                numReorderAttempts--;
            } while (CheckForDuplicateDoorIndices() && numReorderAttempts > 0);
            if (CheckForDuplicateDoorIndices())
            {
                Debug.LogWarning(name + " failed to reorder all of the doors correctly.  It should be done manually in order to prevent unwanted behavior.", gameObject);
            }
            UpdateDoors();
        }

        /// <summary>
        /// Get the wall settings from the Level
        /// </summary>
        private void SetLevelUsedForWalls()
        {
            var levels = FindObjectsOfType<Level>();
            if (levels.Length == 0)
            {
                level = new GameObject(LevelName).AddComponent<Level>();
            }
            else
            {
                level = levels[0];
            }
            if (levels.Length > 1)
            {
                Debug.LogWarning("There is more than one Level in the Scene.  Only " + level.name + " will be used for wall settings.");
            }
        }

        /// <summary>
        /// Calculate the positions whre the walls are to be placed
        /// </summary>
        /// <returns>The verticies to be used for walls</returns>
        private Vector2[] CalculateWallVertices()
        {
            var verticesMap = new Dictionary<Direction, List<Vector2>>();
            var westStartingPosition = (Vector2)transform.position + new Vector2(-width * Level.GridSize + level.GetWallWidth(), -height * Level.GridSize + level.GetWallWidth()) / 2.0f;
            var westEndingPosition = (Vector2)transform.position + new Vector2(-width * Level.GridSize + level.GetWallWidth(), height * Level.GridSize - level.GetWallWidth()) / 2.0f;
            var eastStartingPosition = (Vector2)transform.position + new Vector2(width * Level.GridSize - level.GetWallWidth(), height * Level.GridSize - level.GetWallWidth()) / 2.0f;
            var eastEndingPosition = (Vector2)transform.position + new Vector2(width * Level.GridSize - level.GetWallWidth(), -height * Level.GridSize + level.GetWallWidth()) / 2.0f;
            var northStartingPosition = westEndingPosition + new Vector2(level.GetWallWidth(), 0);
            var northEndingPosition = eastStartingPosition - new Vector2(level.GetWallWidth(), 0);
            var southStartingPosition = eastEndingPosition - new Vector2(level.GetWallWidth(), 0);
            var southEndingPosition = westStartingPosition + new Vector2(level.GetWallWidth(), 0);
            verticesMap.Add(Direction.West, new List<Vector2>());
            verticesMap[Direction.West].Add(westStartingPosition);
            verticesMap.Add(Direction.North, new List<Vector2>());
            verticesMap[Direction.North].Add(northStartingPosition);
            verticesMap.Add(Direction.East, new List<Vector2>());
            verticesMap[Direction.East].Add(eastStartingPosition);
            verticesMap.Add(Direction.South, new List<Vector2>());
            verticesMap[Direction.South].Add(southStartingPosition);
            foreach (var door in doors)
            {
                if (level.GetDoorSize() == 0)
                {
                    break;
                }
                var position = (globalPoint + door.GetLocalPoint()) * Level.GridSize;
                switch (door.GetSide())
                {
                    case Direction.West:
                        position.x -= Level.GridSize / 2;
                        verticesMap[Direction.West].Add(position + new Vector2(level.GetWallWidth(), -level.GetWallWidth() - level.GetDoorSize()) / 2.0f);
                        verticesMap[Direction.West].Add(position + new Vector2(level.GetWallWidth(), level.GetWallWidth() + level.GetDoorSize()) / 2.0f);
                        break;
                    case Direction.North:
                        position.y += Level.GridSize / 2;
                        verticesMap[Direction.North].Add(position + new Vector2(-level.GetWallWidth() - level.GetDoorSize(), -level.GetWallWidth()) / 2.0f);
                        verticesMap[Direction.North].Add(position + new Vector2(level.GetWallWidth() + level.GetDoorSize(), -level.GetWallWidth()) / 2.0f);
                        break;
                    case Direction.East:
                        position.x += Level.GridSize / 2;
                        verticesMap[Direction.East].Add(position + new Vector2(-level.GetWallWidth(), level.GetWallWidth() + level.GetDoorSize()) / 2.0f);
                        verticesMap[Direction.East].Add(position + new Vector2(-level.GetWallWidth(), -level.GetWallWidth() - level.GetDoorSize()) / 2.0f);
                        break;
                    case Direction.South:
                        position.y -= Level.GridSize / 2;
                        verticesMap[Direction.South].Add(position + new Vector2(level.GetWallWidth() + level.GetDoorSize(), level.GetWallWidth()) / 2.0f);
                        verticesMap[Direction.South].Add(position + new Vector2(-level.GetWallWidth() - level.GetDoorSize(), level.GetWallWidth()) / 2.0f);
                        break;
                    default:
                        Debug.LogError("Invalid Door Direction (" + door.GetSide() + " received!", gameObject);
                        break;
                }
            }
            verticesMap[Direction.West].Add(westEndingPosition);
            verticesMap[Direction.North].Add(northEndingPosition);
            verticesMap[Direction.East].Add(eastEndingPosition);
            verticesMap[Direction.South].Add(southEndingPosition);
            RemoveVertexOverlap(verticesMap);
            var westVertices = verticesMap[Direction.West].ToArray();
            var northVertices = verticesMap[Direction.North].ToArray();
            var eastVertices = verticesMap[Direction.East].ToArray();
            var southVertices = verticesMap[Direction.South].ToArray();
            var vertices = westVertices.Concat(northVertices).Concat(eastVertices).Concat(southVertices).ToArray();
            return vertices;
        }

        /// <summary>
        /// Cancel the duplicate wall verticies on the corners of a Room
        /// </summary>
        /// <param name="verticesMap">The list of vertices and side of room they are on</param>
        private void RemoveVertexOverlap(Dictionary<Direction, List<Vector2>> verticesMap)
        {
            foreach (var verticesPair in verticesMap)
            {
                switch (verticesPair.Key)
                {
                    case Direction.West:
                        var westVerticesList = verticesPair.Value;
                        for (int i = 0; i < westVerticesList.Count - 1; i += 2)
                        {
                            if (westVerticesList[i].y > westVerticesList[i + 1].y)
                            {
                                if (i < westVerticesList.Count)
                                {
                                    westVerticesList[i + 1] = westVerticesList[i];
                                }
                                else
                                {
                                    westVerticesList[i] = westVerticesList[i + 1];
                                }
                            }
                        }
                        break;
                    case Direction.North:
                        var northVerticesList = verticesPair.Value;
                        for (int i = 0; i < northVerticesList.Count - 1; i += 2)
                        {
                            if (northVerticesList[i].x > northVerticesList[i + 1].x)
                            {
                                if (i < northVerticesList.Count)
                                {
                                    northVerticesList[i + 1] = northVerticesList[i];
                                }
                                else
                                {
                                    northVerticesList[i] = northVerticesList[i + 1];
                                }
                            }
                        }
                        break;
                    case Direction.East:
                        var eastVerticesList = verticesPair.Value;
                        for (int i = 0; i < eastVerticesList.Count - 1; i += 2)
                        {
                            if (eastVerticesList[i].y < eastVerticesList[i + 1].y)
                            {
                                if (i < eastVerticesList.Count)
                                {
                                    eastVerticesList[i + 1] = eastVerticesList[i];
                                }
                                else
                                {
                                    eastVerticesList[i] = eastVerticesList[i + 1];
                                }
                            }
                        }
                        break;
                    case Direction.South:
                        var southVerticesList = verticesPair.Value;
                        for (int i = 0; i < southVerticesList.Count - 1; i += 2)
                        {
                            if (southVerticesList[i].x < southVerticesList[i + 1].x)
                            {
                                if (i < southVerticesList.Count)
                                {
                                    southVerticesList[i + 1] = southVerticesList[i];
                                }
                                else
                                {
                                    southVerticesList[i] = southVerticesList[i + 1];
                                }
                            }
                        }
                        break;
                    default:
                        Debug.LogError("Invalid Vertex Direction (" + verticesPair.Key + " received!", gameObject);
                        break;
                }
            }
        }

        /// <summary>
        /// Creates a wall
        /// </summary>
        /// <returns>The wall that was created</returns>
        private GameObject InstantiateWall()
        {
            var wall = GameObject.CreatePrimitive(PrimitiveType.Quad);
            wall.name = WallName;
            DestroyImmediate(wall.GetComponent<MeshCollider>());
            var wallRenderer = wall.GetComponent<MeshRenderer>();
            wallRenderer.shadowCastingMode = ShadowCastingMode.Off;
            wallRenderer.receiveShadows = false;
#if UNITY_5_4_OR_NEWER
            wallRenderer.lightProbeUsage = LightProbeUsage.Off;
#else
            wallRenderer.useLightProbes = false;
#endif
            wallRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            return wall;
        }

        /// <summary>
        /// Check if multiple doors share the same index
        /// </summary>
        /// <returns>Whether or not multiple doors share the same index</returns>
        private bool CheckForDuplicateDoorIndices()
        {
            foreach (var door0 in doors)
            {
                foreach (var door1 in doors)
                {
                    if (door0 != door1 && door0.GetIndex() == door1.GetIndex())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Make sure all children remain within the bounds of the room
        /// </summary>
        private void EncompassChildren()
        {
            if (!level)
            {
                return;
            }
            var descendents = transform.GetComponentsInChildren<Transform>();
            foreach (Transform child in descendents)
            {
#if USING_SUPER_TILEMAP_EDITOR
                if (child.GetComponentsInParent<Tilemap>().Length > 0)
                {
                    continue;
                }
#endif
                var extents = new Vector2(width, height) * Level.GridSize / 2.0f;
                if (level.GetWallTexture() && child.name != WallName)
                {
                    var wallSize = new Vector2(level.GetWallWidth(), level.GetWallWidth());
                    extents -= wallSize;
                }
                var childRenderer = child.GetComponent<Renderer>();
                if (childRenderer)
                {
                    var childExtents = (Vector2)childRenderer.bounds.extents;
                    var scaleMultiplier = Vector2.one;
                    if (childExtents.x > extents.x)
                    {
                        scaleMultiplier.x *= extents.x / childExtents.x;
                    }
                    if (childExtents.y > extents.y)
                    {
                        scaleMultiplier.y *= extents.y / childExtents.y;
                    }
                    child.localScale = Vector2.Scale(child.localScale, scaleMultiplier);
                    childExtents = childRenderer.bounds.extents;
                    extents -= childExtents;
                }
                float newX = Mathf.Clamp(child.position.x, transform.position.x - extents.x, transform.position.x + extents.x);
                float newY = Mathf.Clamp(child.position.y, transform.position.y - extents.y, transform.position.y + extents.y);
                child.transform.position = new Vector2(newX, newY);
            }
        }

        /// <summary>
        /// Draw debug lines to show the walls
        /// </summary>
        private void DrawOutline()
        {
            if (!level)
            {
                SetLevelUsedForWalls();
            }
            Gizmos.color = OutlineColor;
            Gizmos.DrawWireCube(transform.position, new Vector2(width, height) * Level.GridSize);
            if (level.GetWallTexture())
            {
                var wallSize = new Vector2(level.GetWallWidth(), level.GetWallWidth());
                Gizmos.color = InlineColor;
                Gizmos.DrawWireCube(transform.position, new Vector2(width, height) * Level.GridSize - wallSize * 2);
                if (Application.isPlaying)
                {
                    return;
                }
                Gizmos.color = WallColor;
                var vertices = CalculateWallVertices();
                for (int i = 0; i < vertices.Length - 1; i += 2)
                {
                    var position = vertices[i] + (vertices[i + 1] - vertices[i]) / 2.0f;
                    var scale = vertices[i + 1] - vertices[i];
                    scale = new Vector2(Mathf.Abs(scale.x), Mathf.Abs(scale.y));
                    scale += new Vector2(level.GetWallWidth(), level.GetWallWidth());
                    Gizmos.DrawCube(position, scale);
                }
            }
        }

        /// <summary>
        /// Draw the doors to show where doors are, and whether they are closed/open
        /// </summary>
        private void DrawDoors()
        {
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i] == null)
                {
                    doors[i] = new Door(DoorNamePrefix + " " + i, i);
                }
            }
            int numDoors = width * 2 + height * 2;
            int doorIndex = 0;
            for (int i = 0; i < numDoors; i++)
            {
                Vector2 position;
                if (i < height)
                {
                    position = (globalPoint + new Point2(0, i)) * Level.GridSize;
                    position.x -= Level.GridSize / 2;
                }
                else if (i < height + width)
                {
                    position = (globalPoint + new Point2(i - height, height - 1)) * Level.GridSize;
                    position.y += Level.GridSize / 2;
                }
                else if (i < height * 2 + width)
                {
                    position = (globalPoint + new Point2(width - 1, height - 1 - i + height + width)) * Level.GridSize;
                    position.x += Level.GridSize / 2;
                }
                else
                {
                    position = (globalPoint + new Point2(width - 1 - i + height * 2 + width, 0)) * Level.GridSize;
                    position.y -= Level.GridSize / 2;
                }
                if (doorIndex < doors.Length && i == doors[doorIndex].GetIndex())
                {
                    doorIndex++;
                    Gizmos.DrawIcon(position, FilePaths.DoorGizmo, false);
                }
                else
                {
                    Gizmos.DrawIcon(position, FilePaths.NoDoorGizmo);
                }
            }
        }

        /// <summary>
        /// Get which side of the room a door is on
        /// </summary>
        /// <param name="door">The door to check</param>
        /// <returns>Which side of the room the door is on</returns>
        private Direction GetSide(Door door)
        {
            if (door.GetIndex() < height)
            {
                return Direction.West;
            }
            else if (door.GetIndex() < height + width)
            {
                return Direction.North;
            }
            else if (door.GetIndex() < height * 2 + width)
            {
                return Direction.East;
            }
            else
            {
                return Direction.South;
            }
        }
    }
}