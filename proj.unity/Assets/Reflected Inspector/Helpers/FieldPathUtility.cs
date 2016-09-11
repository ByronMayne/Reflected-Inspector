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

    /// <summary>
    /// Checks to see if an path entry is an array.
    /// </summary>
    public static bool IsArrayPath(string pathEntry)
    {
        return pathEntry.StartsWith(Constants.LIST_ENTRY_START);
    }
}
