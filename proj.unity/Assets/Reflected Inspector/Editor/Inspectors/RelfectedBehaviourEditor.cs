using ReflectedInspector;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ReflectedBehaviour), true)]
public class RelfectedBehaviourEditor : Editor
{
    //ReflectedObject m_ReflectedObject;
    ReflectedAspect objectAspect;

    public void OnEnable()
    {
        //m_ReflectedObject = new ReflectedObject(targets);
        objectAspect = new ReflectedAspect(targets);
    }

    public void OnDisable()
    {
        //m_ReflectedObject.ApplyModifiedFields();
        objectAspect.ApplyModifiedChanges();
    }

    public override void OnInspectorGUI()
    {
        objectAspect.OnGUILayout();

        //m_ReflectedObject.DoLayout();

        //if (GUILayout.Button("Print"))
        //{
        //    m_ReflectedObject.Print();
        //}
    }
}
