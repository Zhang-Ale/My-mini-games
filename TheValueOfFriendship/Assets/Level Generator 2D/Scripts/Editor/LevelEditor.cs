namespace LevelGenerator2D
{
    using UnityEngine;
    using UnityEditor;

    /// <summary>
    /// Custom editor UI for inspector for Level
    /// </summary>
    [CustomEditor(typeof(Level))]
    public class LevelEditor : Editor
    {
        private const string RoomPersistanceLabel = "Persistant Rooms";
        private const string RoomPersistanceTooltip = "Whether or not to retain Rooms after they leave the Main Camera.  This could create latency issues in larger levels if enabled.";
        private const string WallTextureLabel = "Wall Texture";
        private const string WallTextureTooltip = "This is the Texture2D to be used for creating walls.  If None, no walls will be created. If a Texture2D is used, its size should be a power of 2.";
        private const string WallThicknessLabel = "Wall Thickness";
        private const string WallThicknessTooltip = "This determines how thick the walls will be";
        private const string DoorSizeLabel = "Door Size";
        private const string DoorSizeTooltip = "This determines the size of the opening on the doors by changing the size of gaps in the walls.";
        private const float MinDoorSize = 0.0f;
        private const float MaxDoorSize = 5.0f;

        public override void OnInspectorGUI()
        {
            var level = (Level)target;
            MaintainRoomPersistance(level);
            MaintainWalls(level);
            SceneView.RepaintAll();
        }

        /// <summary>
        /// Draws the UI for the inspector for room persistance
        /// </summary>
        /// <param name="level">Level to draw the UI for</param>
        private void MaintainRoomPersistance(Level level)
        {
            var guiContent = new GUIContent(RoomPersistanceLabel, RoomPersistanceTooltip);
            var roomPersistance = EditorGUILayout.Toggle(guiContent, level.IsPersistantRooms());
            level.SetPersistantRooms(roomPersistance);
        }

        /// <summary>
        /// Draws UI in the inspector for wall usage
        /// </summary>
        /// <param name="level">Level to draw the UI for</param>
        private void MaintainWalls(Level level)
        {
            var wallTextureGuiContent = new GUIContent(WallTextureLabel, WallTextureTooltip);
            var wallTexture = (Texture2D)EditorGUILayout.ObjectField(wallTextureGuiContent, level.GetWallTexture(), typeof(Texture2D), false);
            level.SetWallTexture(wallTexture);
            if (wallTexture)
            {
                var wallThicknessGuiContent = new GUIContent(WallThicknessLabel, WallThicknessTooltip);
                var wallThickness = (WallWidth)EditorGUILayout.EnumPopup(wallThicknessGuiContent, level.GetWallThickness());
                level.SetWallThickness(wallThickness);
                var doorSizeGuiContent = new GUIContent(DoorSizeLabel, DoorSizeTooltip);
                var doorSize = EditorGUILayout.Slider(doorSizeGuiContent, level.GetDoorSize(), MinDoorSize, MaxDoorSize);
                level.SetDoorSize(doorSize);
            }
        }
    }
}