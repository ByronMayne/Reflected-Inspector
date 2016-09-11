using System;


namespace ReflectedInspector
{
    /// <summary>
    ///  The base class for all attributes that can be used by Reflected Inspector
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public abstract class FieldAttribute : Attribute
    {
    }
}
