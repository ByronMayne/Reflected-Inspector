
using System;

namespace ReflectedInspector
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class HideField : FieldAttribute
    {
        /// <summary>
        /// Hides this field from being drawn in the Reflected Inspector. 
        /// </summary>
        public HideField()
        {

        }
    }
}
