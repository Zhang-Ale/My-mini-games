namespace LevelGenerator2D
{
    using UnityEngine;
    using CustomUnityLibrary;
    /// <summary>
    /// These are extensions for Cameras
    /// </summary>
    public static partial class CameraExtensions
    {
        /// <summary>
        /// Gets the area that the camera covers in grid view
        /// </summary>
        /// <param name="camera">The camera to look from</param>
        /// <returns>The area covered by the camera's viewport</returns>
        public static IntRect GetPointsVisibleByCamera(this Camera camera)
        {
            var cameraViewport = camera.GetViewport();
            float minX = cameraViewport.x;
            float minY = cameraViewport.y;
            float maxX = minX + cameraViewport.width;
            float maxY = minY + cameraViewport.height;
            int gridX = Mathf.FloorToInt(minX / Level.GridSize);
            int gridY = Mathf.FloorToInt(minY / Level.GridSize);
            int gridWidth = Mathf.FloorToInt(maxX / Level.GridSize) - gridX;
            int gridHeight = Mathf.FloorToInt(maxY / Level.GridSize) - gridY;
            return new IntRect(gridX, gridY, gridWidth, gridHeight);
        }
    }
}