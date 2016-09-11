using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ReflectedInspector
{ 
  public class PropertyDrawer
  {
    private PropertyAttribute m_Attribute;


    protected PropertyDrawer()
    {

    }

    //
    // Summary:
    //     ///
    //     The PropertyAttribute for the property. Not applicable for custom class drawers.
    //     (Read Only)
    //     ///
    public PropertyAttribute attribute
    {
      get { return m_Attribute; }
    }

    //
    // Summary:
    //     ///
    //     Override this method to specify how tall the GUI for this field is in pixels.
    //     ///
    //
    // Parameters:
    //   property:
    //     The SerializedProperty to make the custom GUI for.
    //
    //   label:
    //     The label of this property.
    //
    // Returns:
    //     ///
    //     The height in pixels.
    //     ///
    public virtual float GetPropertyHeight(ReflectedField property, GUIContent label)
    {
      return EditorGUIUtility.singleLineHeight;
    }

    //
    // Summary:
    //     ///
    //     Override this method to make your own GUI for the property.
    //     ///
    //
    // Parameters:
    //   position:
    //     Rectangle on the screen to use for the property GUI.
    //
    //   property:
    //     The SerializedProperty to make the custom GUI for.
    //
    //   label:
    //     The label of this property.
    public virtual void OnGUI(Rect position, ReflectedField property, GUIContent label)
    {

    }
  }
}
