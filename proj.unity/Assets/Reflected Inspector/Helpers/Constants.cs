namespace ReflectedInspector
{
    public static class Constants
    {
        /// <summary>
        /// The separator between fields when they are in a string path.
        /// </summary>
        public static readonly char PATH_SEPARATOR = '.';

        /// <summary>
        /// The sequence path that represents an array element start
        /// </summary>
        public static readonly string LIST_ENTRY_START = "list[";

        /// <summary>
        /// The sequence path that represents an array element end
        /// </summary>
        public static readonly string LIST_ENTRY_END = "]";

        /// <summary>
        /// The sequence path that represents an dictionary entry start
        /// </summary> 
        public static readonly string DICTIONARY_ENTRY_START = "dictionary[";

        /// <summary>
        /// The sequence path that represents an dictionary entry end
        /// </summary> 
        public static readonly string DICTIONARY_ENTRY_END = "]";
    }
}
