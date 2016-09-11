using ReflectedInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

public static class ReflectionHelper
{
    public static readonly BindingFlags INSTANCE_BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    public static object GetFieldValue(string fieldSequence, object @object)
    {
        bool wasSuccessful = false;
        return GetFieldValue(fieldSequence, @object, out wasSuccessful);
    }

    public static object GetFieldValue(string fieldSequence, object @object, out bool wasSuccessful)
    {
        // We can't work if we don't have a root object.
        if (@object == null)
        {
            throw new System.ArgumentNullException("Root is Null");
        }

        // Get our current type
        Type type = @object.GetType();

        // Break up the path (zero periods is also valid)
        string[] entries = fieldSequence.Split(Constants.PATH_SEPARATOR);

        for (int i = 0; i < entries.Length; i++)
        {
            // Is it an array?
            if (SequenceHelper.IsArrayPath(entries[i]))
            {
                IList list = @object as IList;

                if (list == null)
                {
                    throw new System.InvalidCastException("Object of type: " + @object.ToString() + " does not inheirt IList");
                }

                // Get our index
                int index = SequenceHelper.GetArrayIndex(entries[i]);

                // Load our value
                @object = list[index];
            }
            else // Not an array path
            {

                // Load our type  
                type = @object.GetType();

                // Look for our field
                FieldInfo fieldInfo = type.GetField(entries[i], INSTANCE_BINDING_FLAGS);

                if (fieldInfo == null)
                {
                    // Unable to find field by that name. 
                    wasSuccessful = false;
                    return null;
                }

                // Load the value from our field.
                @object = fieldInfo.GetValue(@object);
            }

            // If we are not on the last loop and we have a null.
            if (@object == null && i < entries.Length - 1)
            {
                // Some object along the way was null. 
                wasSuccessful = false;
                return @object;
            }
        }

        // We reached the end and the last value was retrieved (even if it was null).  
        wasSuccessful = true;
        return @object;
    }

    public static void SetFieldValue(string fieldSequence, object @object, object value)
    {
        // We can't work if we don't have a root object.
        if (@object == null)
        {
            throw new System.ArgumentNullException("Root is Null");
        }

        // Get our current type
        Type type = @object.GetType();

        // Break up the path (zero periods is also valid)
        string[] entries = fieldSequence.Split(Constants.PATH_SEPARATOR);


        for (int i = 0; i < entries.Length; i++)
        {

            if (SequenceHelper.IsArrayPath(entries[i]))
            {
                IList list = @object as IList;

                if (list == null)
                {
                    throw new System.InvalidCastException("Object of type: " + @object.ToString() + " does not inheirt IList");
                }

                // Get our index
                int index = SequenceHelper.GetArrayIndex(entries[i]);


                int diff = index - list.Count + 1;

                Type itemType = @object.GetType().GetGenericArguments()[0];

                // We have to resize our array since it's too small.
                for (int d = 0; d < diff; d++)
                {
                    object newEntry = GetDefault(itemType);
                    list.Add(newEntry);
                }

 

                if (i == entries.Length - 1)
                {
                    // Load our value
                    list[index] = value;
                }
                else
                {
                    @object = list[index];
                }
            }
            else
            {
                // Load our type  
                type = @object.GetType();

                // Look for our field
                FieldInfo fieldInfo = type.GetField(entries[i], INSTANCE_BINDING_FLAGS);

                if (fieldInfo == null)
                {
                    // Unable to find field by that name. 
                    throw new TargetException("No field on the target " + @object.ToString() + " has the name " + entries[i]);
                }

                if (i == entries.Length - 1)
                {
                    // We have gone deep enough
                    fieldInfo.SetValue(@object, value);
                }
                else
                {
                    // We have to dive deeper to set the value.

                    // We use this holder just in case our value is null.
                    object subObject = fieldInfo.GetValue(@object);

                    // If we are not on the last loop and we have a null.
                    if (subObject == null && i < entries.Length - 1)
                    {
                        // Create a new instance
                        subObject = FormatterServices.GetUninitializedObject(fieldInfo.FieldType);

                        // Set our current value
                        fieldInfo.SetValue(@object, subObject);
                    }

                    // Continue on with our current value
                    @object = subObject;

                    if (i == entries.Length - 1)
                    {
                        // We are on our last loop
                        fieldInfo.SetValue(@object, value);
                    }
                }
            }
        }

    }

    public static object GetDefault(Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }
        return null;
    }

}
