using UnityEngine;

namespace ReflectedInspector
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class ReadOnlyAttribute : FieldAttribute
    {
        /// <summary>
        /// When applied to a field it will be viewable but not editable. 
        /// </summary>
        public ReadOnlyAttribute()
        {

        }
    }
}
