using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[System.Serializable]
public class Person
{
    private int m_Age;
    private string m_Name;
    private string m_LastName;
}

public class BayWindow : Window
{
    public float m_PanelCount = 3;
}


[System.Serializable]
public class Window
{
    [SerializeField]
    protected Vector2 m_Size;
    protected bool m_IsOpen;
}
[System.Serializable]
public struct Door
{
    public bool isOpen;
    public Color color;
}


public class House : ReflectedBehaviour
{
    //[System.NonSerialized]
    //
    //private HashSet<Person> m_Occupents;
    //public string address = "123 Fake Street";
    //public ICollection<Door> m_Doors = new List<Door>();
    //private Dictionary<string, string> m_Dictionary = new Dictionary<string, string>();

    //public List<string> m_Names = new List<string>()
    //{
    //  "Byron",
    //  "Mayne"
    //};

    private string m_Name = "Byron";
    private int m_Age;
    private bool m_IsAlive;
    private float m_Length;
    [System.NonSerialized]
    public Door m_Door;
    [System.NonSerialized]
    public Window m_Window;


    //private List<Window> m_Windows = new List<Window>();
}
