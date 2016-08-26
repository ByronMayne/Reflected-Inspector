using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wheel
{
  public float speed = 5;
}

public struct Window
{
  private float Size;
}


public class Car : ReflectedBehaviour
{
 private Window m_Window;
 private string m_CarName;
 private float m_CarSpeed;
 private IEnumerable m_CarBrand = "Ford";
 private string nullString = null;
 private HashSet<float> m_FloatSet;
 private List<string> m_StringList;
 private Wheel m_Wheel;
}
