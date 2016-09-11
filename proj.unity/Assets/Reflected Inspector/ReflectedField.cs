using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using Object = UnityEngine.Object;
using UnityEditor;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using TinyJSON;
using ReflectedInspector;
//
// Summary:
//     ///
//     Type of a ReflectedField.
//     ///
public enum ReflectedFieldType
{
    Generic = -1,
    Integer = 0,
    Boolean = 1,
    Float = 2,
    String = 3,
    Color = 4,
    ObjectReference = 5,
    LayerMask = 6,
    Enum = 7,
    Vector2 = 8,
    Vector3 = 9,
    Vector4 = 10,
    Rect = 11,
    ArraySize = 12,
    Character = 13,
    AnimationCurve = 14,
    Bounds = 15,
    Gradient = 16,
    Quaternion = 17,
    BitwiseFlag = 18,
    IList = 19,
    Null,
}

/// <summary>
///   <para>ReflectedField and ReflectedObject are classes for editing
///   properties on objects in a completely generic way that automatically 
///   handles undo and styling UI for prefabs.</para>
/// </summary>
public sealed class ReflectedField
{
    [Exclude]
    private FieldInfo m_Property;
    [Exclude]
    private ReflectedObject m_ReflectedObject;
    [Include]
    private ReflectedFieldType m_PropertyType = ReflectedFieldType.Null;
    [Include]
    private object m_Value;
    [Include]
    private string m_Name;
    [Include]
    private string m_Path;
    [Include]
    private string m_DisplayName;
    [Exclude]
    private bool m_IsExplanded = false;
    [Include]
    private int m_ArraySize;
    [Include]
    private List<ReflectedField> m_Children;
    [Include]
    private Type m_ValueType;
    [Include]
    private Type m_ValueTrueType;

    /// <summary>
    /// Gets the <see cref="ReflectedField"/> that this property
    /// belongs too. (Read Only)
    /// </summary>
    public ReflectedObject reflectedObject
    {
        get { return m_ReflectedObject; }
    }

    /// <summary>
    ///  Does this property represent multiple different values due to multi-object editing? (Read Only)
    /// </summary>
    public bool hasMultipleDifferentValues
    {
        get { return false; }
    }

    /// <summary>
    /// Nice name that is displayed as the title to this property. (Read Only)
    /// </summary>
    public string displayName
    {
        get { return m_DisplayName; }
    }

    /// <summary>
    /// Gets the name of this property. (Read Only)
    /// </summary>
    public string name
    {
        get { return m_Name; }
    }

    /// <summary>
    /// Type name of the property. (Read Only)
    /// </summary>
    public string typeName
    {
        get { return null; }
    }

    /// <summary>
    /// Tool-tip of the property. (Read Only)
    /// </summary>
    public string tooltip
    {
        get { return string.Empty; }
    }

    /// <summary>
    /// Nesting depth of the property. (Read Only)
    /// </summary>
    public int depth
    {
        get { return 0; }
    }

    /// <summary>
    /// Full path of the property. (Read Only)
    /// </summary>
    public string propertyPath
    {
        get { return m_Path; }
    }

    /// <summary>
    /// Is this property editable? (Read Only)
    /// </summary>
    public bool isEditable
    {
        get { return true; }
    }

    /// <summary>
    /// Is this property Animated? (Read Only)
    /// </summary>
    public bool isAnimated
    {
        get { return true; }
    }

    /// <summary>
    /// Is this property expanded in the inspector? (Read Only)
    /// </summary>
    public bool isExpanded
    {
        get { return m_IsExplanded; }
        set { m_IsExplanded = value; }
    }

    /// <summary>
    /// Does this have child properties? (Read Only)
    /// </summary>
    public bool hasChildren
    {
        get { return m_Children.Count > 0; }
    }

    /// <summary>
    /// Is property part of a prefab instance? (Read Only)
    /// </summary>
    public bool isInstantiatedPrefab
    {
        get { return true; }
    }

    /// <summary>
    /// Is property's value different from the prefab it belongs to?
    /// </summary>
    public bool isOverridingPrefab
    {
        get { return true; }
    }

    internal void SaveModifications()
    {
        Debug.Log("Saving: " + propertyPath);
        m_ReflectedObject.SaveValue(this);

        for (int i = 0; i < m_Children.Count; i++)
        {
            m_Children[i].SaveModifications();
        }
    }

    public Type valueTrueType
    {
        get { return m_ValueTrueType; }
    }


    /// <summary>
    /// Type of this property (Read Only)
    /// </summary>
    public ReflectedFieldType propertyType
    {
        get { return m_PropertyType; }
    }

    /// <summary>
    /// Value of this integer property
    /// </summary>
    public int intValue
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.Integer)
            {
                return (int)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type int it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.Integer)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type int it's a " + m_PropertyType.ToString());
            }
        }
    }

    public object rawValue
    {
        get
        {
            return m_Value;
        }
        set
        {
            m_Value = value;
        }
    }


    /// <summary>
    /// Value of this long property
    /// </summary>
    public long longValue
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.Integer)
            {
                return (long)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type long it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.Integer)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type long it's a " + m_PropertyType.ToString());
            }
        }
    }

    /// <summary>
    /// Value of this bool property.
    /// </summary>
    public bool boolValue
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.Boolean)
            {
                return (bool)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type Integer it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.Boolean)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type bool it's a " + m_PropertyType.ToString());
            }
        }
    }

    /// <summary>
    /// Value of this float property.
    /// </summary>
    public float floatValue
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.Float)
            {
                return (float)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type float it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.Float)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type float it's a " + m_PropertyType.ToString());
            }
        }
    }

    /// <summary>
    /// Value of this double property.
    /// </summary>
    public double doubleValue
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.Float)
            {
                return (double)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type double it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.Float)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type double it's a " + m_PropertyType.ToString());
            }
        }
    }

    /// <summary>
    /// Value of this string property.
    /// </summary>
    public string stringValue
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.String)
            {
                return (string)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type string it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.String)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type string it's a " + m_PropertyType.ToString());
            }
        }
    }

    /// <summary>
    /// Value of this color property.
    /// </summary>
    public Color colorValue
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.Color)
            {
                return (Color)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type color it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.Color)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type color it's a " + m_PropertyType.ToString());
            }
        }
    }

    /// <summary>
    /// Value of this animation curve property.
    /// </summary>
    public AnimationCurve animationCurveValue
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.AnimationCurve)
            {
                return (AnimationCurve)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type animation curve it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.AnimationCurve)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type animation curve it's a " + m_PropertyType.ToString());
            }
        }
    }

    /// <summary>
    /// Value of this gradient property.
    /// </summary>
    public Gradient gradientValue
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.Gradient)
            {
                return (Gradient)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type gradient  it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.Gradient)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type gradient it's a " + m_PropertyType.ToString());
            }
        }
    }

    /// <summary>
    /// Value of the object reference value of this property.
    /// </summary>
    public Object objectReferenceValue
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.ObjectReference)
            {
                return (Object)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type Object it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.ObjectReference)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type Object it's a " + m_PropertyType.ToString());
            }
        }
    }

    /// <summary>
    /// Gets the value of this enum property.
    /// </summary>
    public int enumValueIndex
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.Enum)
            {
                return (int)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type enum it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.Enum)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type enum it's a " + m_PropertyType.ToString());
            }
        }
    }

    /// <summary>
    /// Display-friendly names of enumeration of an enum property.
    /// </summary>
    public string[] enumNames
    {
        get { return null; }
        set { }
    }

    /// <summary>
    /// Value of the 2D vector property.
    /// </summary>
    public Vector2 vector2Value
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.Vector2)
            {
                return (Vector2)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type Vector2 it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.Vector2)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type Vector2 it's a " + m_PropertyType.ToString());
            }
        }
    }
    /// <summary>
    /// Value of the #D vector property.
    /// </summary>
    public Vector3 vector3Value
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.Vector3)
            {
                return (Vector3)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type Vector3 it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.Vector3)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type Vector3 it's a " + m_PropertyType.ToString());
            }
        }
    }
    /// <summary>
    /// Value of the 4D vector property.
    /// </summary>
    public Vector4 vector4Value
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.Vector3)
            {
                return (Vector4)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type Vector4 it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.Vector4)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type Vector4 it's a " + m_PropertyType.ToString());
            }
        }
    }

    /// <summary>
    /// Value of the quaternion property.
    /// </summary>
    public Quaternion quaternionValue
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.Quaternion)
            {
                return (Quaternion)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type Quaternion it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.Quaternion)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type Quaternion it's a " + m_PropertyType.ToString());
            }
        }
    }

    /// <summary>
    /// Value of the rect property.
    /// </summary>
    public Rect rectValue
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.Rect)
            {
                return (Rect)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type Rect it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.Rect)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type Rect it's a " + m_PropertyType.ToString());
            }
        }
    }

    /// <summary>
    /// Value of the bounds property.
    /// </summary>
    public Bounds boundsValue
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.Bounds)
            {
                return (Bounds)m_Value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type Bounds it's a " + m_PropertyType.ToString());
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.Bounds)
            {
                m_Value = value;
            }
            else
            {
                throw new System.InvalidCastException(name + " is not of type Bounds it's a " + m_PropertyType.ToString());
            }
        }
    }

    /// <summary>
    /// Is this property an array? (Read Only)
    /// </summary>
    public bool isArray
    {
        get { return m_PropertyType == ReflectedFieldType.IList; }
    }

    /// <summary>
    /// The number of elements in the array. If the ReflectedObject
    /// contains multiple objects it will return the smallest number of elements. 
    /// So it is always possible to iterate through the ReflectedObject and 
    /// only get properties found in all objects.
    /// </summary>
    public int arraySize
    {
        get
        {
            if (m_PropertyType == ReflectedFieldType.ArraySize)
            {
                return (int)m_Value;
            }
            else
            {
                return m_ArraySize;
            }
        }
        set
        {
            if (m_PropertyType == ReflectedFieldType.ArraySize)
            {
                m_Value = value;
            }
            else
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (m_ArraySize != value)
                {
                    int differnce = value - m_ArraySize;

                    if (differnce > 0)
                    {
                        for (int i = 0; i < differnce; i++)
                        {
                            // Adding
                            InsetArrayElementAtIndex(arraySize);
                        }
                    }
                    else
                    {
                        for (int i = differnce; i < 0; i++)
                        {
                            // Removing
                            DeleteArrayElmentAtIndex(arraySize - 1);
                        }
                    }
                }
            }
        }
    }

    public bool isValuetype
    {
        get
        {
            switch (m_PropertyType)
            {
                case ReflectedFieldType.Integer:
                case ReflectedFieldType.Boolean:
                case ReflectedFieldType.Float:
                case ReflectedFieldType.LayerMask:
                case ReflectedFieldType.Enum:
                case ReflectedFieldType.Vector2:
                case ReflectedFieldType.Vector3:
                case ReflectedFieldType.Rect:
                case ReflectedFieldType.Vector4:
                case ReflectedFieldType.ArraySize:
                case ReflectedFieldType.Character:
                case ReflectedFieldType.Bounds:
                case ReflectedFieldType.Quaternion:
                case ReflectedFieldType.BitwiseFlag:
                    return true;
                case ReflectedFieldType.Gradient:
                case ReflectedFieldType.String:
                case ReflectedFieldType.Generic:
                case ReflectedFieldType.Color:
                case ReflectedFieldType.ObjectReference:
                case ReflectedFieldType.AnimationCurve:
                case ReflectedFieldType.IList:
                default:
                    return false;
            }
        }
    }

    public ReflectedField()
    {

    }


    /// <summary>
    /// Hidden constructor for serialized properties.
    /// </summary>
    internal ReflectedField(ReflectedObject reflectedObject, ReflectedFieldType type, Type systemType, string path)
    {
        m_ReflectedObject = reflectedObject;

        int index = path.LastIndexOf('.') + 1;

        if (index > 0)
        {
            m_Name = path.Substring(index, path.Length - index);
        }
        else
        {
            m_Name = path;
        }

        m_Name = m_Name.Replace("[", "");
        m_Name = m_Name.Replace("]", "");

        m_Path = path;
        m_PropertyType = type;
        m_ValueType = systemType;
        m_DisplayName = UnityEditor.ObjectNames.NicifyVariableName(m_Name);
        m_Children = new List<ReflectedField>();

        object instanceValue = m_ReflectedObject.LoadValue(this);

        if (m_ValueType.IsValueType)
        {
            if (instanceValue == null)
            {
                // Struts can't be null
                m_Value = Activator.CreateInstance(m_ValueType);
            }
            else
            {
                m_Value = instanceValue;
            }
        }
        else
        if (m_ValueType == typeof(string))
        {
            m_Value = instanceValue;
        }
        else
        if (instanceValue != null)
        {
            // Check for polymorphic type. 
            m_ValueTrueType = instanceValue.GetType();
        }
        else
        {
            m_Value = null;
            m_ValueTrueType = m_ValueType;
            m_PropertyType = ReflectedFieldType.Null;
        }

        LoadChildren(instanceValue);
    }

    private void LoadChildren(object instanceValue)
    {
        if (m_PropertyType == ReflectedFieldType.Generic)
        {
            FieldInfo[] fileds = m_ValueTrueType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            for (int i = 0; i < fileds.Length; i++)
            {
                Type fieldType = fileds[i].FieldType;

                ReflectedFieldType propertyType = GetReflectedFieldType(fieldType);

                ReflectedField newProperty = new ReflectedField(m_ReflectedObject, propertyType, fieldType, propertyPath + "." + fileds[i].Name);

                m_Children.Add(newProperty);
            }
        }
        else if (m_PropertyType == ReflectedFieldType.IList)
        {
            IList list = (IList)instanceValue;

            m_ArraySize = 0;

            if (list != null)
            {
                foreach (var item in list)
                {
                    ReflectedFieldType propertyType = GetReflectedFieldType(item.GetType());

                    ReflectedField newProperty = new ReflectedField(m_ReflectedObject, propertyType, item.GetType(), SequenceHelper.AppendListEntryToSequence(propertyPath, m_Children.Count));

                    m_Children.Add(newProperty);

                    m_ArraySize++;
                }
            }
        }
    }

    /// <summary>
    ///   <para>See if contained serialized properties are equal.</para>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public static bool EqualContents(ReflectedField x, ReflectedField y)
    {
        return false;
    }

    /// <summary>
    /// Move to the next property.
    /// </summary>
    public bool Next(bool enterChildren)
    {
        return false;
    }

    /// <summary>
    /// Move to next visible property.
    /// </summary>
    public bool NextVisible(bool enterChildren)
    {
        return false;
    }

    /// <summary>
    /// Move to the first property of the object.
    /// </summary>
    public void Reset()
    {
    }

    /// <summary>
    /// Count remaining visible properties.
    /// </summary>
    /// <returns></returns>
    public int CountRemaining()
    {
        return 0;
    }

    /// <summary>
    /// Count visible children of this property, including this property itself.
    /// </summary>
    /// <returns></returns>
    public int CountInProperty()
    {
        return 0;
    }

    public static void Copy(ReflectedField from, ReflectedField to)
    {

    }

    /// <summary>
    ///   Returns a copy of the Reflected iterator in its current state. This is useful if you want to 
    ///   keep a reference to the current property but continue with the iteration.
    /// </summary>
    public ReflectedField Copy()
    {
        ReflectedField clone = new ReflectedField();
        clone.m_Children = new List<ReflectedField>();
        clone.m_PropertyType = m_PropertyType;
        clone.m_ReflectedObject = m_ReflectedObject;
        clone.m_ValueType = m_ValueType;
        clone.m_ValueTrueType = m_ValueTrueType;
        clone.m_Name = m_Name;
        clone.m_DisplayName = m_DisplayName;
        clone.m_IsExplanded = false;
        clone.m_Path = m_Path;
        clone.m_Value = m_Value;
        clone.m_ArraySize = m_ArraySize;

        CopyChildren(this, clone);

        return clone;
    }

    private void CopyChildren(ReflectedField from, ReflectedField to)
    {
        for (int i = 0; i < from.m_Children.Count; i++)
        {
            ReflectedField child = from.m_Children[i].Copy();
            to.m_Children.Add(child);
        }
    }

    /// <summary>
    ///   <para>Retrieves the SerializedProperty at a relative path to the current property.</para>
    /// </summary>
    /// <param name="relativePropertyPath"></param>
    public ReflectedField FindFieldRelative(string relativePropertyPath)
    {
        ReflectedField serializedProperty = this.Copy();
        //TODO: Find
        return serializedProperty;
    }


    /// <summary>
    ///   <para>Retrieves the SerializedProperty that defines the end range of this property.</para>
    /// </summary>
    /// <param name="includeInvisible"></param>
    public ReflectedField GetEndProperty(bool includeInvisible = false)
    {
        ReflectedField ReflectedField = this.Copy();
        if (includeInvisible)
        {
            ReflectedField.Next(false);
        }
        else
        {
            ReflectedField.NextVisible(false);
        }
        return ReflectedField;
    }


    internal static ReflectedFieldType GetReflectedFieldType(Type type)
    {
        ReflectedFieldType propertyType = ReflectedFieldType.Generic;


        if (type == typeof(AnimationCurve))
        {
            propertyType = ReflectedFieldType.AnimationCurve;
        }
        else if (type == typeof(string))
        {
            propertyType = ReflectedFieldType.String;
        }
        else if (typeof(IEnumerable).IsAssignableFrom(type))
        {
            propertyType = ReflectedFieldType.IList;
        }
        else if (type == typeof(bool))
        {
            propertyType = ReflectedFieldType.Boolean;
        }
        else if (type == typeof(Bounds))
        {
            propertyType = ReflectedFieldType.Bounds;
        }
        else if (type == typeof(char))
        {
            propertyType = ReflectedFieldType.Character;
        }
        else if (type == typeof(Color))
        {
            propertyType = ReflectedFieldType.Color;
        }
        else if (typeof(Enum).IsAssignableFrom(type))
        {
            propertyType = ReflectedFieldType.Enum;
        }
        else if (type == typeof(float))
        {
            propertyType = ReflectedFieldType.Float;
        }
        else if (type == typeof(Gradient))
        {
            propertyType = ReflectedFieldType.Gradient;
        }
        else if (type == typeof(int))
        {
            propertyType = ReflectedFieldType.Integer;
        }
        else if (type == typeof(Object))
        {
            propertyType = ReflectedFieldType.ObjectReference;
        }
        else if (type == typeof(Quaternion))
        {
            propertyType = ReflectedFieldType.Quaternion;
        }
        else if (type == typeof(Rect))
        {
            propertyType = ReflectedFieldType.Rect;
        }
        else if (type == typeof(Vector2))
        {
            propertyType = ReflectedFieldType.Vector2;
        }
        else if (type == typeof(Vector3))
        {
            propertyType = ReflectedFieldType.Vector3;
        }
        else if (type == typeof(Vector4))
        {
            propertyType = ReflectedFieldType.Vector4;
        }
        return propertyType;
    }



    /// <summary>
    ///   <para>Retrieves an iterator that allows you to iterator over the current nexting of a serialized property.</para>
    /// </summary>
    public IEnumerator GetEnumerator()
    {
        return null;
    }

    /// <summary>
    ///   <para>Returns the element at the specified index in the array.</para>
    /// </summary>
    /// <param name="index"></param>
    public ReflectedField GetArrayElementAtIndex(int index)
    {
        return m_Children[index];
    }

    /// <summary>
    /// Inserts a new reflected property at an array index.
    /// </summary>
    public ReflectedField InsetArrayElementAtIndex(int index)
    {
        Type elementType = null;
        ReflectedField child = null;

        if (arraySize > 0)
        {
            // We have an element we can just copy so we reuse it. 
            child = m_Children[arraySize - 1].Copy();
        }
        else
        {
            if (m_ValueType.IsArray)
            {
                // Normal Array
            }
            else if (m_ValueType.IsGenericType)
            {
                // Mostly likely a list, HashSet, or LinkedList
                elementType = m_ValueType.GetGenericArguments()[0];
                child = new ReflectedField(m_ReflectedObject, GetReflectedFieldType(elementType), elementType, SequenceHelper.AppendListEntryToSequence(propertyPath, index));
            }
        }

        child.m_Name = SequenceHelper.AppendListEntryToSequence(propertyPath, index + 1);
        child.m_DisplayName = "Element " + index.ToString() + " [" + child.valueTrueType.Name + "]";
        m_ArraySize++;
        m_Children.Add(child);
        return child;
    }

    /// <summary>
    /// Deletes an element at an index.
    /// </summary>
    public void DeleteArrayElmentAtIndex(int index)
    {
        m_Children.RemoveAt(index);
        m_ArraySize--;
    }

    /// <summary>
    /// Move an array element from srcIndex to dstIndex
    /// </summary>
    public void MoveArrayElement(int srcIndex, int destIndexx)
    {
    }

    internal void SetToValueOfTarget(UnityEngine.Object target)
    {
        ReflectedField serializedProperty = new ReflectedObject(target).FindField(this.propertyPath);
        if (serializedProperty == null)
        {
            UnityEngine.Debug.LogError(target.name + " does not have the property " + this.propertyPath);
            return;
        }
        switch (this.propertyType)
        {
            case ReflectedFieldType.Null:
                m_Value = null;
                break;
            case ReflectedFieldType.Integer:
                this.intValue = serializedProperty.intValue;
                break;
            case ReflectedFieldType.Boolean:
                this.boolValue = serializedProperty.boolValue;
                break;
            case ReflectedFieldType.Float:
                this.floatValue = serializedProperty.floatValue;
                break;
            case ReflectedFieldType.String:
                this.stringValue = serializedProperty.stringValue;
                break;
            case ReflectedFieldType.Color:
                this.colorValue = serializedProperty.colorValue;
                break;
            case ReflectedFieldType.ObjectReference:
                this.objectReferenceValue = serializedProperty.objectReferenceValue;
                break;
            case ReflectedFieldType.LayerMask:
                this.intValue = serializedProperty.intValue;
                break;
            case ReflectedFieldType.Enum:
                this.enumValueIndex = serializedProperty.enumValueIndex;
                break;
            case ReflectedFieldType.Vector2:
                this.vector2Value = serializedProperty.vector2Value;
                break;
            case ReflectedFieldType.Vector3:
                this.vector3Value = serializedProperty.vector3Value;
                break;
            case ReflectedFieldType.Vector4:
                this.vector4Value = serializedProperty.vector4Value;
                break;
            case ReflectedFieldType.Rect:
                this.rectValue = serializedProperty.rectValue;
                break;
            case ReflectedFieldType.ArraySize:
                this.intValue = serializedProperty.intValue;
                break;
            case ReflectedFieldType.Character:
                this.intValue = serializedProperty.intValue;
                break;
            case ReflectedFieldType.AnimationCurve:
                this.animationCurveValue = serializedProperty.animationCurveValue;
                break;
            case ReflectedFieldType.Bounds:
                this.boundsValue = serializedProperty.boundsValue;
                break;
            case ReflectedFieldType.Gradient:
                this.gradientValue = serializedProperty.gradientValue;
                break;
        }
    }

#if UNITY_EDITOR
    internal void DoLayout()
    {

        Rect workingRect = new Rect(EditorGUILayout.BeginHorizontal());
        {
            switch (this.propertyType)
            {
                case ReflectedFieldType.Null:
                    //workingRect.y += workingRect.height / 2f;
                    //workingRect.height = 3;
                    //GUI.Box(workingRect, GUIContent.none, (GUIStyle)"(GUIStyle)"SoloToggle"");
                    //GUI.Box(workingRect, "Null", (GUIStyle)"ShurikenModuleTitle");

                    GUIStyle font = new GUIStyle((GUIStyle)"ChannelStripAttenuationMarkerSquare");
                    font.fontStyle = FontStyle.Italic;
                    EditorGUILayout.LabelField(displayName, "Null", font, GUILayout.ExpandWidth(false));
                    GUILayout.FlexibleSpace();
                    break;
                case ReflectedFieldType.Integer:
                    intValue = UnityEditor.EditorGUILayout.IntField(displayName, intValue);
                    break;
                case ReflectedFieldType.Boolean:
                    boolValue = UnityEditor.EditorGUILayout.Toggle(displayName, boolValue);
                    break;
                case ReflectedFieldType.Float:
                    floatValue = UnityEditor.EditorGUILayout.FloatField(displayName, floatValue);
                    break;
                case ReflectedFieldType.String:
                    stringValue = UnityEditor.EditorGUILayout.TextField(displayName, stringValue);
                    break;
                case ReflectedFieldType.Color:
                    colorValue = UnityEditor.EditorGUILayout.ColorField(displayName, colorValue);
                    break;
                case ReflectedFieldType.ObjectReference:
                    objectReferenceValue = UnityEditor.EditorGUILayout.ObjectField(displayName, objectReferenceValue, typeof(UnityEngine.Object), false);
                    break;
                case ReflectedFieldType.LayerMask:
                    enumValueIndex = UnityEditor.EditorGUILayout.LayerField(displayName, enumValueIndex);
                    break;
                case ReflectedFieldType.Enum:
                    enumValueIndex = UnityEditor.EditorGUILayout.Popup(displayName, enumValueIndex, enumNames);
                    break;
                case ReflectedFieldType.Vector2:
                    vector2Value = UnityEditor.EditorGUILayout.Vector2Field(displayName, vector2Value);
                    break;
                case ReflectedFieldType.Vector3:
                    vector3Value = UnityEditor.EditorGUILayout.Vector3Field(displayName, vector3Value);
                    break;
                case ReflectedFieldType.Vector4:
                    vector4Value = UnityEditor.EditorGUILayout.Vector4Field(displayName, vector4Value);
                    break;
                case ReflectedFieldType.Rect:
                    rectValue = UnityEditor.EditorGUILayout.RectField(displayName, rectValue);
                    break;
                case ReflectedFieldType.ArraySize:
                    arraySize = UnityEditor.EditorGUILayout.IntField(displayName, arraySize);
                    break;
                case ReflectedFieldType.AnimationCurve:
                    animationCurveValue = UnityEditor.EditorGUILayout.CurveField(displayName, animationCurveValue);
                    break;
                case ReflectedFieldType.Bounds:
                    boundsValue = UnityEditor.EditorGUILayout.BoundsField(displayName, boundsValue);
                    break;
                case ReflectedFieldType.Generic:
                    m_IsExplanded = EditorGUILayout.Foldout(isExpanded, displayName);
                    break;
                case ReflectedFieldType.IList:
                    m_IsExplanded = EditorGUILayout.Foldout(isExpanded, displayName);
                    break;
            }

            if (!isValuetype)
            {
                EditorGUI.BeginDisabledGroup(m_PropertyType != ReflectedFieldType.Null);
                if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false)))
                {
                    OnElementAdded();
                }
                EditorGUI.EndDisabledGroup();


                EditorGUI.BeginDisabledGroup(m_PropertyType != ReflectedFieldType.Null || m_ValueType.IsValueType || m_ValueType == typeof(string));
                {
                    if (GUILayout.Button("&", EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false)))
                    {
                        OnPolymorphicElementAdded();
                    }
                }
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(m_PropertyType == ReflectedFieldType.Null);
                if (GUILayout.Button("x", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false)))
                {
                    OnElementRemoved();
                }
                EditorGUI.EndDisabledGroup();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel++;
        if (isExpanded)
        {
            if (isArray)
            {
                arraySize = EditorGUILayout.IntField("Size", arraySize);
            }

            for (int i = 0; i < m_Children.Count; i++)
            {
                m_Children[i].DoLayout();
            }
        }
        EditorGUI.indentLevel--;

    }

    public void OnPolymorphicElementAdded()
    {
        GenericMenu menu = new GenericMenu();

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        for (int i = 0; i < assemblies.Length; i++)
        {
            Type[] types = assemblies[i].GetTypes();

            for (int t = 0; t < types.Length; t++)
            {
                if (m_ValueType.IsAssignableFrom(types[t]))
                {
                    Type selectedType = types[t];
                    menu.AddItem(new GUIContent(types[t].Name), false, () =>
                    {
                        if (m_ValueType == typeof(string))
                        {
                            m_Value = string.Empty;
                            m_PropertyType = ReflectedFieldType.String;
                            m_ValueTrueType = typeof(string);
                        }
                        else
                        {
                            m_Value = Activator.CreateInstance(selectedType);
                            m_PropertyType = GetReflectedFieldType(selectedType);
                            m_ValueTrueType = selectedType;
                        }
                        LoadChildren(m_Value);
                    }
                  );
                    menu.ShowAsContext();
                }
            }
        }
    }

    private void OnElementRemoved()
    {
        m_Value = null;
        m_PropertyType = ReflectedFieldType.Null;
        m_Children.Clear();
    }

    private void OnElementAdded()
    {
        if (m_ValueType == typeof(string))
        {
            m_Value = string.Empty;
            m_PropertyType = ReflectedFieldType.String;
        }
        else
        {
            m_Value = Activator.CreateInstance(m_ValueType);
            m_PropertyType = GetReflectedFieldType(m_ValueType);
        }
        LoadChildren(m_Value);
    }


#endif
}
