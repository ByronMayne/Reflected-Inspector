using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TinyJSON;

public class ReflectedBehaviour : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField]
    private string m__SerailzedData;

    [SerializeField]
    private bool m__HasValueChanges = false;

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
        m__SerailzedData = JSON.Dump(this, EncodeOptions.EncodePrivateVariables);
    }
}
