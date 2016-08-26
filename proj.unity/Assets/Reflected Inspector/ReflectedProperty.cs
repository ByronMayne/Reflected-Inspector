using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using Object = UnityEngine.Object;
using UnityEditor;
using System.Collections.Generic;
using System.Runtime.Serialization;
//
// Summary:
//     ///
//     Type of a ReflectedProperty.
//     ///
public enum ReflectedPropertyType
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
  IEnumerable = 19,
}

/// <summary>
///   <para>ReflectedProperty and ReflectedObject are classes for editing
///   properties on objects in a completely generic way that automatically 
///   handles undo and styling UI for prefabs.</para>
/// </summary>
public sealed class ReflectedProperty
{
  private FieldInfo m_Property;

  private ReflectedObject m_ReflectedObject;

  private ReflectedPropertyType m_Type;
  private object m_Value;
  private string m_Name;
  private string m_Path;
  private string m_DisplayName;
  private bool m_IsExplanded = false;
  private int m_ArraySize;
  private List<ReflectedProperty> m_Children;
  private Type m_FieldSystemType;
  private Type m_ValueSystemType;

  /// <summary>
  /// Gets the <see cref="ReflectedProperty"/> that this property
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
    m_ReflectedObject.SaveValue(this);

    for (int i = 0; i < m_Children.Count; i++)
    {
      m_Children[i].SaveModifications();
    }
  }

  /// <summary>
  /// Type of this property (Read Only)
  /// </summary>
  public ReflectedPropertyType propertyType
  {
    get { return m_Type; }
  }

  /// <summary>
  /// Value of this integer property
  /// </summary>
  public int intValue
  {
    get
    {
      if (m_Type == ReflectedPropertyType.Integer)
      {
        return (int)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type int it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.Integer)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type int it's a " + m_Type.ToString());
      }
    }
  }

  public object objectValue
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
      if (m_Type == ReflectedPropertyType.Integer)
      {
        return (long)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type long it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.Integer)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type long it's a " + m_Type.ToString());
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
      if (m_Type == ReflectedPropertyType.Boolean)
      {
        return (bool)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type Integer it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.Boolean)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type bool it's a " + m_Type.ToString());
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
      if (m_Type == ReflectedPropertyType.Float)
      {
        return (float)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type float it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.Float)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type float it's a " + m_Type.ToString());
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
      if (m_Type == ReflectedPropertyType.Float)
      {
        return (double)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type double it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.Float)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type double it's a " + m_Type.ToString());
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
      if (m_Type == ReflectedPropertyType.String)
      {
        return (string)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type string it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.String)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type string it's a " + m_Type.ToString());
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
      if (m_Type == ReflectedPropertyType.Color)
      {
        return (Color)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type color it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.Color)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type color it's a " + m_Type.ToString());
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
      if (m_Type == ReflectedPropertyType.AnimationCurve)
      {
        return (AnimationCurve)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type animation curve it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.AnimationCurve)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type animation curve it's a " + m_Type.ToString());
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
      if (m_Type == ReflectedPropertyType.Gradient)
      {
        return (Gradient)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type gradient  it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.Gradient)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type gradient it's a " + m_Type.ToString());
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
      if (m_Type == ReflectedPropertyType.ObjectReference)
      {
        return (Object)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type Object it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.ObjectReference)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type Object it's a " + m_Type.ToString());
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
      if (m_Type == ReflectedPropertyType.Enum)
      {
        return (int)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type enum it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.Enum)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type enum it's a " + m_Type.ToString());
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
      if (m_Type == ReflectedPropertyType.Vector2)
      {
        return (Vector2)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type Vector2 it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.Vector2)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type Vector2 it's a " + m_Type.ToString());
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
      if (m_Type == ReflectedPropertyType.Vector3)
      {
        return (Vector3)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type Vector3 it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.Vector3)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type Vector3 it's a " + m_Type.ToString());
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
      if (m_Type == ReflectedPropertyType.Vector3)
      {
        return (Vector4)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type Vector4 it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.Vector4)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type Vector4 it's a " + m_Type.ToString());
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
      if (m_Type == ReflectedPropertyType.Quaternion)
      {
        return (Quaternion)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type Quaternion it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.Quaternion)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type Quaternion it's a " + m_Type.ToString());
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
      if (m_Type == ReflectedPropertyType.Rect)
      {
        return (Rect)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type Rect it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.Rect)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type Rect it's a " + m_Type.ToString());
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
      if (m_Type == ReflectedPropertyType.Bounds)
      {
        return (Bounds)m_Value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type Bounds it's a " + m_Type.ToString());
      }
    }
    set
    {
      if (m_Type == ReflectedPropertyType.Bounds)
      {
        m_Value = value;
      }
      else
      {
        throw new System.InvalidCastException(name + " is not of type Bounds it's a " + m_Type.ToString());
      }
    }
  }

  /// <summary>
  /// Is this property an array? (Read Only)
  /// </summary>
  public bool isArray
  {
    get { return m_Type == ReflectedPropertyType.IEnumerable; }
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
      if (m_Type == ReflectedPropertyType.ArraySize)
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
      if (m_Type == ReflectedPropertyType.ArraySize)
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
              InsetArrayElementAtIndex(arraySize - 1);
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
      switch (m_Type)
      {
        case ReflectedPropertyType.Integer:
        case ReflectedPropertyType.Boolean:
        case ReflectedPropertyType.Float:
        case ReflectedPropertyType.LayerMask:
        case ReflectedPropertyType.Enum:
        case ReflectedPropertyType.Vector2:
        case ReflectedPropertyType.Vector3:
        case ReflectedPropertyType.Rect:
        case ReflectedPropertyType.Vector4:
        case ReflectedPropertyType.ArraySize:
        case ReflectedPropertyType.Character:
        case ReflectedPropertyType.Bounds:
        case ReflectedPropertyType.Quaternion:
        case ReflectedPropertyType.BitwiseFlag:
          return true;
        case ReflectedPropertyType.Gradient:
        case ReflectedPropertyType.String:
        case ReflectedPropertyType.Generic:
        case ReflectedPropertyType.Color:
        case ReflectedPropertyType.ObjectReference:
        case ReflectedPropertyType.AnimationCurve:
        case ReflectedPropertyType.IEnumerable:
        default:
          return false;
      }
    }
  }


  /// <summary>
  /// Hidden constructor for serialized properties.
  /// </summary>
  internal ReflectedProperty(ReflectedObject reflectedObject, ReflectedPropertyType type, Type systemType, string path)
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

    m_Path = path;
    m_Type = type;
    m_FieldSystemType = systemType;
    m_DisplayName = UnityEditor.ObjectNames.NicifyVariableName(m_Name);
    m_Children = new List<ReflectedProperty>();

    m_ReflectedObject.LoadValue(this);

    m_ValueSystemType = m_Value.GetType();

    if(m_ValueSystemType != m_FieldSystemType)
    {

    }

    if (type == ReflectedPropertyType.Generic)
    {
      FieldInfo[] fileds = systemType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

      for (int i = 0; i < fileds.Length; i++)
      {
        Type fieldType = fileds[i].FieldType;

        ReflectedPropertyType propertyType = GetReflectedPropertyType(fieldType);

        ReflectedProperty newProperty = new ReflectedProperty(m_ReflectedObject, propertyType, fieldType, path + "." + fileds[i].Name);

        m_ReflectedObject.LoadValue(newProperty);

        m_Children.Add(newProperty);
      }
    }
    else if (type == ReflectedPropertyType.IEnumerable)
    {
      IEnumerable collection = (IEnumerable)m_Value;

      m_ArraySize = 0;

      if (collection != null)
      {
        foreach (var item in collection)
        {
          ReflectedPropertyType propertyType = GetReflectedPropertyType(item.GetType());

          ReflectedProperty newProperty = new ReflectedProperty(m_ReflectedObject, propertyType, item.GetType(), path + ".[" + m_Children.Count + "]");

          m_ReflectedObject.LoadValue(newProperty);

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
  public static bool EqualContents(ReflectedProperty x, ReflectedProperty y)
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

  public static void Copy(ReflectedProperty from, ReflectedProperty to)
  {

  }

  /// <summary>
  ///   Returns a copy of the Reflected iterator in its current state. This is useful if you want to 
  ///   keep a reference to the current property but continue with the iteration.
  /// </summary>
  public ReflectedProperty Copy()
  {
    return null;
  }

  /// <summary>
  ///   <para>Retrieves the SerializedProperty at a relative path to the current property.</para>
  /// </summary>
  /// <param name="relativePropertyPath"></param>
  public ReflectedProperty FindPropertyRelative(string relativePropertyPath)
  {
    ReflectedProperty serializedProperty = this.Copy();
    //TODO: Find
    return serializedProperty;
  }


  /// <summary>
  ///   <para>Retrieves the SerializedProperty that defines the end range of this property.</para>
  /// </summary>
  /// <param name="includeInvisible"></param>
  public ReflectedProperty GetEndProperty(bool includeInvisible = false)
  {
    ReflectedProperty reflectedProperty = this.Copy();
    if (includeInvisible)
    {
      reflectedProperty.Next(false);
    }
    else
    {
      reflectedProperty.NextVisible(false);
    }
    return reflectedProperty;
  }


  internal static ReflectedPropertyType GetReflectedPropertyType(Type type)
  {
    ReflectedPropertyType propertyType = ReflectedPropertyType.Generic;


    if (type == typeof(AnimationCurve))
    {
      propertyType = ReflectedPropertyType.AnimationCurve;
    }
    else if (type == typeof(string))
    {
      propertyType = ReflectedPropertyType.String;
    }
    else if (typeof(IEnumerable).IsAssignableFrom(type))
    {
      propertyType = ReflectedPropertyType.IEnumerable;
    }
    else if (type == typeof(bool))
    {
      propertyType = ReflectedPropertyType.Boolean;
    }
    else if (type == typeof(Bounds))
    {
      propertyType = ReflectedPropertyType.Bounds;
    }
    else if (type == typeof(char))
    {
      propertyType = ReflectedPropertyType.Character;
    }
    else if (type == typeof(Color))
    {
      propertyType = ReflectedPropertyType.Color;
    }
    else if (typeof(Enum).IsAssignableFrom(type))
    {
      propertyType = ReflectedPropertyType.Enum;
    }
    else if (type == typeof(float))
    {
      propertyType = ReflectedPropertyType.Float;
    }
    else if (type == typeof(Gradient))
    {
      propertyType = ReflectedPropertyType.Gradient;
    }
    else if (type == typeof(int))
    {
      propertyType = ReflectedPropertyType.Integer;
    }
    else if (type == typeof(Object))
    {
      propertyType = ReflectedPropertyType.ObjectReference;
    }
    else if (type == typeof(Quaternion))
    {
      propertyType = ReflectedPropertyType.Quaternion;
    }
    else if (type == typeof(Rect))
    {
      propertyType = ReflectedPropertyType.Rect;
    }
    else if (type == typeof(Vector2))
    {
      propertyType = ReflectedPropertyType.Vector2;
    }
    else if (type == typeof(Vector3))
    {
      propertyType = ReflectedPropertyType.Vector3;
    }
    else if (type == typeof(Vector4))
    {
      propertyType = ReflectedPropertyType.Vector4;
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
  public ReflectedProperty GetArrayElementAtIndex(int index)
  {
    ReflectedProperty reflectedProperty = this.Copy();
    //TODO: Find array element
    return null;
  }

  /// <summary>
  /// Inserts a new reflected property at an array index.
  /// </summary>
  public ReflectedProperty InsetArrayElementAtIndex(int index)
  {
    ReflectedProperty child = new ReflectedProperty(m_ReflectedObject, ReflectedPropertyType.Integer, m_FieldSystemType.GetElementType(), propertyPath + ".[" + index.ToString() + "]");
    m_Children.Add(child);
    return child;
  }

  /// <summary>
  /// Deletes an element at an index.
  /// </summary>
  public void DeleteArrayElmentAtIndex(int index)
  {
    m_Children.RemoveAt(index);
  }

  /// <summary>
  /// Move an array element from srcIndex to dstIndex
  /// </summary>
  public void MoveArrayElement(int srcIndex, int destIndexx)
  {
  }

  internal void SetToValueOfTarget(UnityEngine.Object target)
  {
    ReflectedProperty serializedProperty = new ReflectedObject(target).FindProperty(this.propertyPath);
    if (serializedProperty == null)
    {
      UnityEngine.Debug.LogError(target.name + " does not have the property " + this.propertyPath);
      return;
    }
    switch (this.propertyType)
    {
      case ReflectedPropertyType.Integer:
        this.intValue = serializedProperty.intValue;
        break;
      case ReflectedPropertyType.Boolean:
        this.boolValue = serializedProperty.boolValue;
        break;
      case ReflectedPropertyType.Float:
        this.floatValue = serializedProperty.floatValue;
        break;
      case ReflectedPropertyType.String:
        this.stringValue = serializedProperty.stringValue;
        break;
      case ReflectedPropertyType.Color:
        this.colorValue = serializedProperty.colorValue;
        break;
      case ReflectedPropertyType.ObjectReference:
        this.objectReferenceValue = serializedProperty.objectReferenceValue;
        break;
      case ReflectedPropertyType.LayerMask:
        this.intValue = serializedProperty.intValue;
        break;
      case ReflectedPropertyType.Enum:
        this.enumValueIndex = serializedProperty.enumValueIndex;
        break;
      case ReflectedPropertyType.Vector2:
        this.vector2Value = serializedProperty.vector2Value;
        break;
      case ReflectedPropertyType.Vector3:
        this.vector3Value = serializedProperty.vector3Value;
        break;
      case ReflectedPropertyType.Vector4:
        this.vector4Value = serializedProperty.vector4Value;
        break;
      case ReflectedPropertyType.Rect:
        this.rectValue = serializedProperty.rectValue;
        break;
      case ReflectedPropertyType.ArraySize:
        this.intValue = serializedProperty.intValue;
        break;
      case ReflectedPropertyType.Character:
        this.intValue = serializedProperty.intValue;
        break;
      case ReflectedPropertyType.AnimationCurve:
        this.animationCurveValue = serializedProperty.animationCurveValue;
        break;
      case ReflectedPropertyType.Bounds:
        this.boundsValue = serializedProperty.boundsValue;
        break;
      case ReflectedPropertyType.Gradient:
        this.gradientValue = serializedProperty.gradientValue;
        break;
    }
  }

#if UNITY_EDITOR
  internal void DoLayout()
  {
    GUILayout.BeginHorizontal();
    {
      if (!isValuetype && m_Value == null)
      {
        EditorGUILayout.LabelField(displayName, "Null", (GUIStyle)"sv_label_6", GUILayout.ExpandWidth(false));
        GUILayout.FlexibleSpace();
      }
      else
      {
        switch (this.propertyType)
        {
          case ReflectedPropertyType.Integer:
            intValue = UnityEditor.EditorGUILayout.IntField(displayName, intValue);
            break;
          case ReflectedPropertyType.Boolean:
            boolValue = UnityEditor.EditorGUILayout.Toggle(displayName, boolValue);
            break;
          case ReflectedPropertyType.Float:
            floatValue = UnityEditor.EditorGUILayout.FloatField(displayName, floatValue);
            break;
          case ReflectedPropertyType.String:
            stringValue = UnityEditor.EditorGUILayout.TextField(displayName, stringValue);
            break;
          case ReflectedPropertyType.Color:
            colorValue = UnityEditor.EditorGUILayout.ColorField(displayName, colorValue);
            break;
          case ReflectedPropertyType.ObjectReference:
            objectReferenceValue = UnityEditor.EditorGUILayout.ObjectField(displayName, objectReferenceValue, typeof(UnityEngine.Object), false);
            break;
          case ReflectedPropertyType.LayerMask:
            enumValueIndex = UnityEditor.EditorGUILayout.LayerField(displayName, enumValueIndex);
            break;
          case ReflectedPropertyType.Enum:
            enumValueIndex = UnityEditor.EditorGUILayout.Popup(displayName, enumValueIndex, enumNames);
            break;
          case ReflectedPropertyType.Vector2:
            vector2Value = UnityEditor.EditorGUILayout.Vector2Field(displayName, vector2Value);
            break;
          case ReflectedPropertyType.Vector3:
            vector3Value = UnityEditor.EditorGUILayout.Vector3Field(displayName, vector3Value);
            break;
          case ReflectedPropertyType.Vector4:
            vector4Value = UnityEditor.EditorGUILayout.Vector4Field(displayName, vector4Value);
            break;
          case ReflectedPropertyType.Rect:
            rectValue = UnityEditor.EditorGUILayout.RectField(displayName, rectValue);
            break;
          case ReflectedPropertyType.ArraySize:
            arraySize = UnityEditor.EditorGUILayout.IntField(displayName, arraySize);
            break;
          case ReflectedPropertyType.AnimationCurve:
            animationCurveValue = UnityEditor.EditorGUILayout.CurveField(displayName, animationCurveValue);
            break;
          case ReflectedPropertyType.Bounds:
            boundsValue = UnityEditor.EditorGUILayout.BoundsField(displayName, boundsValue);
            break;
          case ReflectedPropertyType.Generic:
            m_IsExplanded = EditorGUILayout.Foldout(isExpanded, displayName);
            break;
          case ReflectedPropertyType.IEnumerable:
            m_IsExplanded = EditorGUILayout.Foldout(isExpanded, displayName);
            break;
        }
      }

      if (!isValuetype)
      {
        EditorGUI.BeginDisabledGroup(m_Value != null);
        if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false)))
        {
          if (m_FieldSystemType == typeof(string))
          {
            m_Value = string.Empty;
          }
          else
          {
            m_Value = FormatterServices.GetUninitializedObject(m_FieldSystemType);
          }
        }
        EditorGUI.EndDisabledGroup();
        if (GUILayout.Button("X", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false)))
        {
          m_Value = null;

          m_Children.Clear();

        }
      }
    }
    GUILayout.EndHorizontal();

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
#endif
}
