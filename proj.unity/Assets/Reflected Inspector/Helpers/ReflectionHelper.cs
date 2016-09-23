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

    public static T GetFieldValue<T>(string fieldSequence, object @object)
    {
        bool wasSuccessful = false;
        FieldInfo fieldInfo = null;
        object value = GetFieldValue(fieldSequence, @object, out wasSuccessful, out fieldInfo);

        if( value == null && typeof(T).IsValueType)
        {
            value = default(T);
        }
        return (T)System.Convert.ChangeType(value, typeof(T));
    }


    public static object GetFieldValue(string fieldSequence, object @object, out bool wasSuccessful, out FieldInfo fieldInfo)
    {
        if( string.IsNullOrEmpty(fieldSequence))
        {
            fieldInfo = null;
            wasSuccessful = false;
            return null;
        }

        fieldInfo = null;

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

                if( list.Count <= index )
                {
                    // We are out of range we just create a new instance.
                    wasSuccessful = false;
                    return GetDefault(GetElementType(list)); ;
                }

                // Load our value
                @object = list[index];
            }
            else if( SequenceHelper.IsDictionaryPath(entries[i]))
            {
                IDictionary dictionary = @object as IDictionary;

                string key = SequenceHelper.GetDictionaryKey(entries[i]);

                if (dictionary.Contains(key))
                {
                    @object = dictionary[key];
                    wasSuccessful = true;
                }
                else
                {
                    wasSuccessful = false;
                    return null; 
                }
            }
            else // Not an array path
            {

                // Load our type  
                type = @object.GetType();

                // Look for our field
                fieldInfo = type.GetField(entries[i], INSTANCE_BINDING_FLAGS);

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
                    throw new System.InvalidCastException("Object of type: " + @object.ToString() + " does not inherit IList");
                }

                // Get our index
                int index = SequenceHelper.GetArrayIndex(entries[i]);

                Type itemType = GetElementType(@object.GetType());

                if (i == entries.Length - 1)
                {
                    if( index >= list.Count )
                    {
                        // TODO: This has a chance of reordering a list which is not ideal. Add would throw 
                        // an exception if the value was null which sucked.
                        list.Add(value);
                    }
                    else
                    {
                        // Load our value
                        list[index] = value;
                    }
                }
                else
                {
                    if( index >= list.Count )
                    {
                        // We are at a sub object that has no idex that is valid.
                        return;
                    }
                    else
                    {
                        @object = list[index];
                    }
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
                        // Our parent is null so we can't be set
                        throw new System.ArgumentNullException("The field at path '" + fieldSequence + "' has a parent that is null. Values can be set on nested types that are null. Please make sure the parent has a field property that is set first");
                    }

                    // Continue on with our current value
                    @object = subObject;

                    if (i == entries.Length - 1)
                    {
                        // We are on our last loop
                        fieldInfo.SetValue(@object, value);
                    }
                }

                // Our current object is null so we set our current position to null and break.
                if (@object == null)
                {
                    fieldInfo.SetValue(@object, null);
                    return;
                }

            }
        }

    }

    internal static void GetDictionaryTypes(DictionaryEntry dictionary, out Type keyType, out Type valueType)
    {
        Type type = dictionary.GetType();
        keyType = null;
        valueType = null;
        if (typeof(DictionaryEntry).IsAssignableFrom(type))
        {
            Type[] args = type.GetGenericArguments();
            keyType = args[0];
            valueType = args[1];
        }
    }

    public static Type GetElementType(object listObject)
    {
        Type type = listObject.GetType();

        if( typeof(IList).IsAssignableFrom(type))
        {
            return type.GetGenericArguments()[0];
        }
        return null;
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
