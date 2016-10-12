using ReflectedInspector;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(ReflectedBehaviour), true)]
public class RelfectedBehaviourEditor : Editor
{
    //ReflectedObject m_ReflectedObject;
    ReflectedObject objectAspect;

    public void OnEnable()
    {
        //m_ReflectedObject = new ReflectedObject(targets);
        objectAspect = new ReflectedObject(targets);
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
