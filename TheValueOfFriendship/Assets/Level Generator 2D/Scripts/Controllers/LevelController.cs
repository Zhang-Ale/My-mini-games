using LevelGenerator2D;
using UnityEngine;

/// <summary>
/// The controller used to create the Level.  This is for demo purposes and can be extended/replaced.
/// Use SimpleLevelController instead for easier control.
/// </summary>
[DisallowMultipleComponent] ///Only allow one controller per level
[RequireComponent(typeof(Level))] ///Requires a level to control
public class LevelController : MonoBehaviour
{
    /// <summary>
    /// The random seed to be used when generating levels.  This can be used to ensure consistent results.
    /// </summary>
    [Tooltip("Seed to be used for random number generation.  If left blank, a random seed will be used.")]
    [SerializeField]
    private string seed;

    /// <summary>
    /// This is how far off-Camera Rooms will be loaded when the Level is first generated.  This must be greater than or equal to the runtime load extent, because there is no reason to force a runtime load when a preload will handle the same case.
    /// </summary>
    [Tooltip("Extent at which to preload the level.  This will cause a loading time, but can prevent runtime loading until the edge is reached.  The preload extent must be greater than or equal to the runtime load extent.")]
    [SerializeField]
    [Range(0, 25)]
    private int preloadExtent = 10;

    /// <summary>
    /// This is how far off-Camera Rooms will be loaded when the Camera moves.  For a faster runtime, a smaller value can be used.  But using the Room's maximum width/height is recommended, so that a Room will have a high probability of being shown on Camera, instead of empty space (when not yet loaded).
    /// </summary>
    [Tooltip("Extent at which to load the level during runtime. A smaller number here can lead to a faster runtime, but can cause late loading of some Rooms.")]
    [SerializeField]
    [Range(0, 10)]
    private int runtimeLoadExtent = 10;

    /// <summary>
    /// The maximum number of rooms allowed before primitive rooms will be added to close off the level.  This can be used if you want to cap the level size.  Or you can keep it at 0 to allow for an infinitely generated level.
    /// </summary>
    [Tooltip("Maximum number of Rooms before they start closing.  The actual maximum number of Rooms will vary, but there will be at least this many.  If 0, there will be unlimited rooms.  The preload build will not be affected by the max number of rooms.")]
    [SerializeField]
    [Range(0, 1000)]
    private int maxRooms = 0;

    /// <summary>
    /// These are the folders that the Rooms will be loaded from.  If no folders are added, all Rooms in the Resources folders will be chosen from.  Otherwise, it will search the first folder for a fitting room before moving onto the next folder.  This can be useful for changing difficulty of loaded rooms during runtime.
    /// </summary>
    [Tooltip("Folders whose rooms can be loaded.  Rooms will be loaded in order of priority here. When no room can fit to be loaded, a primitive 1x1 room will be used instead.  To prevent primtive Rooms from being loaded, every door combination of 1x1 Rooms should be added to the load paths.")]
    [SerializeField]
    private string[] roomFolders;

    /// <summary>
    /// The level that will be controlled.
    /// </summary>
    private Level level;

    /// <summary>
    /// Initializes any references needed, and sets up the random seed
    /// </summary>
    void Awake()
    {
        SetRandomSeed(); ///Sets the random seed used in the level generation.
        level = GetComponent<Level>(); ///Grabs the level which will be generated.
    }

    /// <summary>
    /// Populates the Level with any existing Rooms, then builds the base level off of it, taking the preload extent into account.
    /// </summary>
    void Start()
    {
        PopulateLevelWithPreexistingRooms(); ///Gets all Rooms in the Scene, and adds them to the Level
        if (level.GetRooms().Length > 0) ///Checks if there are any Rooms which were added to the Level in the line above
        {
            BuildBaseLevel(); ///Builds the preload Rooms off of the starting Room's in the Level
        }
        else ///Reacts if no Rooms were added ahead of time
        {
            Debug.LogError("There is no initial Room.  The Level Controller has failed to build the Level.  Add a Room to the Scene, so that the Level Controller can build off of it.", gameObject); ///Throws an error.  The algorithm works by building off of existing Rooms.  At least one Room must be added to the Level initially.
        }
    }

    /// <summary>
    /// Once per frame, the Level will check if it should be extended based on the Camera's position.  Taking runtime load extents and max Rooms into account, it will add additional Rooms to the level if the Camera gets close enough to Rooms without neighbors.
    /// </summary>
    void Update()
    {
        ExpandLevel(); ///Adds Rooms to the level, if the Camera position deems fit
    }

    /// <summary>
    /// Keeps data up valid when changed in the inspector.
    /// </summary>
    void OnValidate()
    {
        preloadExtent = Mathf.Max(preloadExtent, runtimeLoadExtent); ///Forces the preload extent to be at least the value of the runtime load extent.
    }

    /// <summary>
    /// Sets the random seed used in the level generation.
    /// </summary>
    private void SetRandomSeed()
    {
        if (!string.IsNullOrEmpty(seed)) ///Checks if a seed was input into the inspector
        {
#if UNITY_5_4_OR_NEWER
            Random.InitState(seed.GetHashCode()); ///Grabs an int interpretation of the seed, and sets Unity's random seed to that value
#else
            Random.seed = seed.GetHashCode(); ///Grabs an int interpretation of the seed, and sets Unity's random seed to that value
#endif
        }
    }

    /// <summary>
    /// Moves all existing rooms to the attached level
    /// </summary>
    private void PopulateLevelWithPreexistingRooms()
    {
        var startingRooms = FindObjectsOfType<Room>(); ///Grabs any Rooms in the current Scene
        level.AddRooms(startingRooms); ///Adds those Rooms to the Level
    }

    /// <summary>
    /// Creates the initial level
    /// </summary>
    private void BuildBaseLevel()
    {
        var visiblePoints = Camera.main.GetPointsVisibleByCamera(); ///Gets a rectangle with all of the grid points visible by the Camera
        int minX = visiblePoints.xMin - preloadExtent; ///Gets the minimum grid X such that it is visible by the Camera and beyond as far as the preload extents
        int minY = visiblePoints.yMin - preloadExtent; ///Gets the minimum grid Y such that it is visible by the Camera and beyond as far as the preload extents
        int maxX = visiblePoints.xMax + preloadExtent; ///Gets the maximum grid X such that it is visible by the Camera and beyond as far as the preload extents
        int maxY = visiblePoints.yMax + preloadExtent; ///Gets the maximum grid Y such that it is visible by the Camera and beyond as far as the preload extents
        level.AddNeighbors(minX, minY, maxX, maxY, roomFolders); ///Adds Rooms the Level such that the Room's position is within the bounds of the Camera's view and the preload extents
    }

    /// <summary>
    /// Expands the level as needed
    /// </summary>
    private void ExpandLevel()
    {
        if (!IsLevelFull()) ///Checks the if max number of Rooms has not been reached
        {
            var visiblePoints = Camera.main.GetPointsVisibleByCamera(); ///Gets a rectangle with all of the grid points visible by the Camera
            int minX = visiblePoints.xMin - runtimeLoadExtent; ///Gets the minimum grid X such that it is visible by the Camera and beyond as far as the runtime load extents
            int minY = visiblePoints.yMin - runtimeLoadExtent; ///Gets the minimum grid Y such that it is visible by the Camera and beyond as far as the runtime load extents
            int maxX = visiblePoints.xMax + runtimeLoadExtent; ///Gets the maximum grid X such that it is visible by the Camera and beyond as far as the runtime load extents
            int maxY = visiblePoints.yMax + runtimeLoadExtent; ///Gets the maximum grid Y such that it is visible by the Camera and beyond as far as the runtime load extents
            var rooms = level.GetRooms(minX, minY, maxX, maxY); ///Gets all of the Rooms within the bounds
            foreach (var room in rooms) ///Iterates through every Room within the bounds
            {
                if (room.IsOpen()) ///Checks if the Room is open
                {
                    level.AddNeighbors(room, roomFolders); ///Adds neighbors to the open Room, taking folder priority into account
                }
            }
        }
        else ///Reacts if the max number of Rooms has been reached
        {
            var openRooms = level.GetOpenRooms(); ///Gets all open Rooms in the level
            level.CloseRooms(openRooms); ///Closes all open Rooms with primitive Rooms
        }
    }

    /// <summary>
    /// Checks if the level has reached its size limit
    /// </summary>
    /// <returns>Whether or not the room is full</returns>
    private bool IsLevelFull()
    {
        return maxRooms > 0 && level.GetRooms().Length >= maxRooms; ///Returns if the max number of rooms has been met
    }
}