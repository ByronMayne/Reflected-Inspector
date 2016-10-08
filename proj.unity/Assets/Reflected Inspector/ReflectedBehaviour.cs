using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TinyJSON;

public class ReflectedBehaviour : MonoBehaviour, ISerializationCallbackReceiver
{

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
    }
}
