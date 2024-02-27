namespace LevelGenerator2D
{
    using UnityEngine;
    using UnityEditor;

    /// <summary>
    /// Adds options to the file menu
    /// </summary>
    public class MenuOptions
    {
        private const string LevelName = "Level";
        private const string RoomName = "Room";
        private const string SpawnerName = "Spawner";

        /// <summary>
        /// Creates a new Room
        /// </summary>
        [MenuItem("GameObject/Level Generator 2D/Room %#r")]
        private static void InstantiateRoom()
        {
            var level = Object.FindObjectOfType<Level>();
            if (!level)
            {
                level = new GameObject(LevelName).AddComponent<Level>();
            }
            var room = new GameObject(RoomName);
            room.AddComponent<Room>();
            room.transform.SetParent(level.transform);
        }

        [MenuItem("GameObject/Level Generator 2D/Spawner %#w")]
        private static void InstantiateSpawner()
        {
            var selectedTransform = Selection.activeTransform;
            if (!selectedTransform || !selectedTransform.GetComponent<Room>())
            {
                Debug.LogWarning("A Room must be selected to instantiate a Spawner.");
                return;
            }
            var spawner = new GameObject(SpawnerName).AddComponent<Spawner>();
            spawner.transform.SetParent(selectedTransform);
            Selection.activeGameObject = spawner.gameObject;
        }
    }
}