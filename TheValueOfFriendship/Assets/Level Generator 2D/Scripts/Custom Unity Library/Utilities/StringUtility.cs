namespace CustomUnityLibrary
{
    public static class StringUtility
    {
        private const string NewLineCharacter = "\n";

        /// <summary>
        /// Gets the \n symbol for a new line
        /// </summary>
        /// <returns>\n</returns>
        public static string NewLine()
        {
            return NewLineCharacter;
        }
    }
}