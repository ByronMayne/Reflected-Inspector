using ReflectedInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class SequenceHelper
{
    public static string AppendListEntryToSequence(string currentSequence, int index)
    {
        return currentSequence + "." + Constants.LIST_ENTRY_START + index.ToString() + Constants.LIST_ENTRY_END;
    }

    public static string AppendDictionaryEntryToSequence(string currentSequence, object key)
    {

        return currentSequence + "." + Constants.DICTIONARY_ENTRY_START + key.ToString() + Constants.DICTIONARY_ENTRY_END;
    }

    /// <summary>
    /// Given a full path finds the 'array[(Any Number)]' and returns it's value as an int.
    /// </summary>
    public static int GetArrayIndex(string path)
    {
        int openIndex = path.IndexOf(Constants.LIST_ENTRY_START);

        if (openIndex == -1)
        {
            return -1;
        }

        openIndex += Constants.LIST_ENTRY_START.Length;

        int closeIndex = path.IndexOf(Constants.LIST_ENTRY_END);

        int intergerCount = closeIndex - openIndex;

        if (intergerCount <= 0)
        {
            return -1;
        }

        char[] numbers = new char[intergerCount];

        for (int i = 0; i < intergerCount; i++)
        {
            numbers[i] = path[i + openIndex];
        }

        return int.Parse(new string(numbers));
    }

    public static string GetDictionaryKey(string path)
    {
        int openIndex = path.IndexOf(Constants.DICTIONARY_ENTRY_START);

        if (openIndex == -1)
        {
            return "";
        }

        openIndex += Constants.DICTIONARY_ENTRY_START.Length;

        int closeIndex = path.IndexOf(Constants.DICTIONARY_ENTRY_END);

        int keyLength = closeIndex - openIndex;

        if (keyLength <= 0)
        {
            return "";
        }

        char[] key = new char[keyLength];

        for (int i = 0; i < keyLength; i++)
        {
            key[i] = path[i + openIndex];
        }

        return new string(key);
    }

    public static string GetDisplayNameFromPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }

        int periodIndex = path.LastIndexOf('.');

        if (periodIndex > 0)
        {
            periodIndex++;
            path = path.Substring(periodIndex, path.Length - periodIndex);
        }

        if (path.StartsWith("m_"))
        {
            path = path.Replace("m_", string.Empty);
        }

        StringBuilder newpath = new StringBuilder(path.Length * 2);
        newpath.Append(char.ToUpper(path[0]));
        for (int i = 1; i < path.Length; i++)
        {
            if (char.IsUpper(path[i]))
            {

                if ((path[i - 1] != ' ' && !char.IsUpper(path[i - 1])) || char.IsUpper(path[i - 1]) && i < path.Length - 1 && !char.IsUpper(path[i + 1]))
                {
                    newpath.Append(' ');
                }
            }

            newpath.Append(path[i]);
        }
        return newpath.ToString();
    }

    /// <summary>
    /// Checks to see if an path entry is an array.
    /// </summary>
    public static bool IsArrayPath(string pathEntry)
    {
        return pathEntry.StartsWith(Constants.LIST_ENTRY_START);
    }

    public static bool IsDictionaryPath(string pathEntry)
    {
        return pathEntry.StartsWith(Constants.DICTIONARY_ENTRY_START);
    }
}
