using UnityEngine;
using System.Collections;
using System;

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
  BitwiseFlag = 18
}

/// <summary>
///   <para>ReflectedProperty and ReflectedObject are classes for editing
///   properties on objects in a completely generic way that automatically 
///   handles undo and styling UI for prefabs.</para>
/// </summary>
public sealed class ReflectedProperty
{
  private IntPtr m_Property;

  private ReflectedObject m_ReflectedObject;

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
    get { return null; }
  }

  /// <summary>
  /// Gets the name of this property. (Read Only)
  /// </summary>
  public string name
  {
    get { return null; }
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
    get { return string.Empty; }
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
    get { return true; }
    set { }
  }

  /// <summary>
  /// Does this have child properties? (Read Only)
  /// </summary>
  public bool hasChildren
  {
    get { return true; }
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

  /// <summary>
  /// Type of this property (Read Only)
  /// </summary>
  public ReflectedPropertyType propertyType
  {
    get { return ReflectedPropertyType.Generic; }
  }

  /// <summary>
  /// Value of this integer property
  /// </summary>
  public int intValue
  {
    get { return 0; }
    set { }
  }

  /// <summary>
  /// Value of this long property
  /// </summary>
  public long longValue
  {
    get { return 0L; }
    set { }
  }

  /// <summary>
  /// Value of this bool property.
  /// </summary>
  public bool boolValue
  {
    get { return false; }
    set { }
  }

  /// <summary>
  /// Value of this float property.
  /// </summary>
  public float floatValue
  {
    get { return 0f; }
    set { }
  }

  /// <summary>
  /// Value of this double property.
  /// </summary>
  public double doubleValue
  {
    get { return 0; }
    set { }
  }

  /// <summary>
  /// Value of this string property.
  /// </summary>
  public string stringValue
  {
    get { return string.Empty; }
    set { }
  }

  /// <summary>
  /// Value of this color property.
  /// </summary>
  public Color colorValue
  {
    get { return Color.white; }
    set { }
  }

  /// <summary>
  /// Value of this animation curve property.
  /// </summary>
  public AnimationCurve animationCurveValue
  {
    get { return null; }
    set { }
  }

  /// <summary>
  /// Value of this gradient property.
  /// </summary>
  public Gradient gradientValue
  {
    get { return null; }
    set { }
  }

  /// <summary>
  /// Value of the object reference value of this property.
  /// </summary>
  public UnityEngine.Object objectReferenceValue
  {
    get { return null; }
    set { }
  }

  /// <summary>
  /// Value of the object reference instance ID of this property.
  /// </summary>
  public int objectReferenceInstanceIDValue
  {
    get { return 0; }
    set { }
  }

  /// <summary>
  /// Gets the value of this enum property.
  /// </summary>
  public int enumValueIndex
  {
    get { return 0; }
    set { }
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
    get { return Vector2.zero; }
    set { }
  }
  /// <summary>
  /// Value of the #D vector property.
  /// </summary>
  public Vector3 vector3Value
  {
    get { return Vector3.zero; }
    set { }
  }
  /// <summary>
  /// Value of the 4D vector property.
  /// </summary>
  public Vector4 vector4Value
  {
    get { return Vector4.zero; }
    set { }
  }

  /// <summary>
  /// Value of the quaternion property.
  /// </summary>
  public Quaternion quaternionValue
  {
    get { return Quaternion.identity; }
    set { }
  }

  /// <summary>
  /// Value of the rect property.
  /// </summary>
  public Rect rectValue
  {
    get { return new Rect(); }
    set { }
  }

  /// <summary>
  /// Value of the bounds property.
  /// </summary>
  public Bounds boundsValue
  {
    get { return new Bounds(); }
    set { }
  }

  /// <summary>
  /// Is this property an array? (Read Only)
  /// </summary>
  public bool isArray
  {
    get { return false; }
  }

  /// <summary>
  /// The number of elements in the array. If the ReflectedObject
  /// contains multiple objects it will return the smallest number of elements. 
  /// So it is always possible to iterate through the ReflectedObject and 
  /// only get properties found in all objects.
  /// </summary>
  public int arraySize
  {
    get { return 0; }
    set { }
  }

  /// <summary>
  /// Hidden constructor for serialized properties.
  /// </summary>
  internal ReflectedProperty()
  {

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
    ReflectedProperty reflectedProperty = new ReflectedProperty();
    reflectedProperty.m_ReflectedObject = m_ReflectedObject;
    //TODO: Copy
    return reflectedProperty;
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
    return null;
  }

  /// <summary>
  /// Deletes an element at an index.
  /// </summary>
  public void DeleteArrayElmentAtIndex(int index)
  {
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
    }
  }
#endif
}
