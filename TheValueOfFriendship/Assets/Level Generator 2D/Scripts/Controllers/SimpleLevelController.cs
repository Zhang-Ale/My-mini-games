using LevelGenerator2D;
using UnityEngine;

/// <summary>
/// A simplified version of the controller used to create the Level.  This is for demo purposes and can be extended/replaced.
/// Use LevelController instead for more control.
/// </summary>
[DisallowMultipleComponent] ///Only allow one controller per level
[RequireComponent(typeof(Level))] ///Requires a level to control
public class SimpleLevelController : MonoBehaviour
{
    /// <summary>
    /// The level which is to be controlled
    /// </summary>
    private Level level;

    /// <summary>
    /// References any needed instances
    /// </summary>
    void Awake()
    {
        level = GetComponent<Level>(); ///Grabs the Level component off of the SimpleLevelController's GameObject
    }

    /// <summary>
    /// Starts off the Level generation by adding any existing Rooms to the Level
    /// </summary>
    void Start()
    {
        PopulateLevelWithPreexistingRooms(); ///Adds any Rooms in the current Scene to the Level
    }

    /// <summary>
    /// Every frame, it checks if the Level should be extended based on the Camera's postiion.  It will add new Room's if its Camera position gets close enough to rooms without neighbors.
    /// </summary>
    void Update()
    {
        ExpandLevel(); ///Expands the Level based on what is seen by the Camera
    }

    /// <summary>
    /// Adds all existing Rooms in the Scene to the Level
    /// </summary>
    private void PopulateLevelWithPreexistingRooms()
    {
        var startingRooms = FindObjectsOfType<Room>(); ///Grabs all Rooms in the current Scene
        level.AddRooms(startingRooms); ///Adds all of the Scene's Rooms to the Level attached to the SimpleLevelController's GameObject
    }

    /// <summary>
    /// Expands the Level as needed.  When a Room that has a Door without a connecting Room is visible by the Camera, a connecting Room is added.
    /// This is the most simplified way to construct the Room.  It has flaws such seeing empty space where Rooms have not yet spawned.  Use LevelController instead for more control.
    /// </summary>
    private void ExpandLevel()
    {
        foreach (var room in level.GetRoomsVisibleByCamera()) ///Iterates through all Rooms which are visible by the Camera.
        {
            if (room.IsOpen()) ///Check if a Room has an open Door, such that there is a Door with no Room currently connected to it.
            {
                level.AddNeighbors(room); ///Add a neighbor to each of the open Room's doors.
            }
        }
    }
}