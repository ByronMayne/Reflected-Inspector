using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using ReflectedInspector;
using System.Text;

/// <summary>
///   <para>ReflectedObject and ReflectedField are classes for editing
///   properties on objects in a completely generic way that automatically 
///   handles undo and styling UI for prefabs. These support types that Unity's
///   serialization system does not support.</para>
/// </summary>
public sealed class ReflectedObject
{

    private IntPtr m_Property;

    private object[] m_Targets;
    private bool m_HasModifiedProperites;
    private HashSet<ReflectedField> m_Fields;

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
    public ReflectedObject(object obj) : this(new object[1] { obj })
    {

    }

    /// <summary>
    /// Creates a new reflected object for an inspected object.
    /// </summary>
    public ReflectedObject(object[] objs)
    {
        m_Fields = new HashSet<ReflectedField>();

        m_Targets = objs;

        Type type = m_Targets[0].GetType();

        FieldInfo[] fileds = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        for (int i = 0; i < fileds.Length; i++)
        {
            Type fieldType = fileds[i].FieldType;

            ReflectedFieldType propertyType = ReflectedField.GetReflectedFieldType(fieldType);

            ReflectedField newProperty = new ReflectedField(this, propertyType, fileds[i].FieldType, fileds[i].Name);

            m_Fields.Add(newProperty);
        }
    }

    internal void SaveValue(ReflectedField property)
    {
        for (int i = 0; i < m_Targets.Length; i++)
        {
            ReflectionHelper.SetFieldValue(property.propertyPath, m_Targets[0], property.rawValue);
        }
    }

    internal object LoadValue(ReflectedField property)
    {
        bool hasMixedValues = false;
        object lhsValue = null, rhsValue = null;

        bool wasSuccessful = false;
        Type fieldType = null;
        rhsValue = ReflectionHelper.GetFieldValue(property.propertyPath, m_Targets[0], out wasSuccessful, out fieldType);

        for (int i = 1; i < m_Targets.Length; i++)
        {
            lhsValue = ReflectionHelper.GetFieldValue(property.propertyPath, m_Targets[0], out wasSuccessful, out fieldType);

            if (lhsValue != rhsValue)
            {
                hasMixedValues = true;
                break;
            }
        }

        return rhsValue;
    }

    /// <summary>
    /// UPdates the reflected object's representation.
    /// </summary>
    public void Update()
    {

    }

    public void DoLayout()
    {
        foreach (var property in m_Fields)
        {
            property.DoLayout();
        }
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
    public ReflectedField GetIterator()
    {
        return null;
    }

    /// <summary>
    /// Finds a reflected property by name that is a part of the reflected object.
    /// </summary>
    /// <returns></returns>
    public ReflectedField FindField(string propertyPath)
    {
        foreach (var reflectedField in m_Fields)
        {
            if (string.CompareOrdinal(reflectedField.propertyPath, propertyPath) == 0)
            {
                return reflectedField;
            }
        }
        return null;
    }

    /// <summary>
    /// Applies all pending modifications for each property. Returns true if any
    /// modifications were made. 
    /// </summary>
    public void ApplyModifiedFields()
    {
        foreach (var property in m_Fields)
        {
            for (int i = 0; i < m_Targets.Length; i++)
            {
                property.SaveModifications();
            }
        }
    }

    /// <summary>
    /// Copies a value from a ReflectedField to the same reflected property on this reflected object.
    /// </summary>
    public void CopyReflectedField(ReflectedField prop)
    {

    }

    public void Print()
    {
        StringBuilder builder = new StringBuilder();
        foreach (var child in m_Fields)
        {
            child.AppendInfo(builder, 0);
        }
        Debug.Log(builder.ToString());
    }

}
