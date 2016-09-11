using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ReflectedBehaviour), true)]
public class RelfectedBehaviourEditor : Editor
{
    ReflectedObject m_ReflectedObject;

    public void OnEnable()
    {
        m_ReflectedObject = new ReflectedObject(targets);
    }

    public void OnDisable()
    {
        m_ReflectedObject.ApplyModifiedFields();
    }

    public override void OnInspectorGUI()
    {
        m_ReflectedObject.DoLayout();

        if (GUILayout.Button("Print"))
        {
            m_ReflectedObject.Print();
        }
    }
}
