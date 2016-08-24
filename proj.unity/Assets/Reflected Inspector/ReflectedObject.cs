using System;

/// <summary>
///   <para>ReflectedObject and ReflectedProperty are classes for editing
///   properties on objects in a completely generic way that automatically 
///   handles undo and styling UI for prefabs. These support types that Unity's
///   serialization system does not support.</para>
/// </summary>
public sealed class ReflectedObject
{
  private IntPtr m_Property;

  private object[] m_Targets;
  private bool m_HasModifiedProperites;

  /// <summary>
  ///   <para>The inspected object (Read Only).</para>
  /// </summary>
  public object targetObject
  {
    get
    {
      return m_Targets[0];
    }
  }

  /// <summary>
  ///   <para>The inspected objects (Read Only).</para>
  /// </summary>
  public object[] targetObjects
  {
    get
    {
      return m_Targets;
    }
  }

  /// <summary>
  /// Has this object had it's properties modified?
  /// </summary>
  public bool hasModifiedProperties
  {
    get
    {
      return m_HasModifiedProperites;
    }
  }

  /// <summary>
  /// Is this inspecting multiple objects? 
  /// </summary>
  public bool isEditingMultipleObjects
  {
    get
    {
      return m_Targets.Length > 1;
    }
  }

  /// <summary>
  /// Creates a new reflected object for an inspected object.
  /// </summary>
  public ReflectedObject(object obj)
  {
    m_Targets = new object[1] { obj };
  }

  /// <summary>
  /// Creates a new reflected object for an inspected object.
  /// </summary>
  public ReflectedObject(object[] objs)
  {
    m_Targets = objs;
  }

  /// <summary>
  /// UPdates the reflected object's representation.
  /// </summary>
  public void Update()
  {

  }

  /// <summary>
  ///  Update hasMultipleDifferentValues cache on the next Update() call.
  /// </summary>
  public void SetIsDifferentCacheDirty()
  {

  }

  /// <summary>
  ///   <para>Update serialized object's representation, only if the object has been modified since the last call to Update or if it is a script.</para>
  /// </summary>
  public void UpdateIfDirtyOrScript()
  {

  }

  /// <summary>
  /// Gets the first serialized property.
  /// </summary>
  /// <returns></returns>
  public ReflectedProperty GetIterator()
  {
    return null;
  }

  /// <summary>
  /// Finds a reflected property by name that is a part of the reflected object.
  /// </summary>
  /// <returns></returns>
  public ReflectedProperty FindProperty(string propertyPath)
  {
    return null;
  }

  /// <summary>
  /// Applies all pending modifications for each property. Returns true if any
  /// modifications were made. 
  /// </summary>
  public bool ApplyModifiedProperties(bool isUndoable)
  {
    return false;
  }

  /// <summary>
  /// Copies a value from a ReflectedProperty to the same reflected property on this reflected object.
  /// </summary>
  public void CopyReflectedProperty(ReflectedProperty prop)
  {

  }

}
