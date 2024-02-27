using UnityEngine.UI;

namespace CustomUnityLibrary
{
    /// <summary>
    /// Extensions for Buttons
    /// </summary>
    public static class ButtonExtensions
    {
        /// <summary>
        /// Changes the navigation mode for the button
        /// </summary>
        /// <param name="button">Button to change navigation for</param>
        /// <param name="navigationMode">Mode to set navigation to</param>
        public static void SetNavigation(this Button button, Navigation.Mode navigationMode)
        {
            var navigation = button.navigation;
            navigation.mode = navigationMode;
            button.navigation = navigation;
        }
    }
}