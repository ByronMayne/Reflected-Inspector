using System.Reflection;
using TinyJSON;
using UnityEditor;
using UnityEngine;
using Type = System.Type;


namespace ReflectedInspector
{
    public class IntAspect : MemberAspect
    {
        /// <summary>
        /// The true value that this class holds.
        /// </summary>
        [Include]
        private int m_Value;

        public IntAspect(ReflectedObject reflectedObject, FieldInfo field) : base(reflectedObject, field)
        {
        }

        /// <summary>
        /// Returns back typeof(int) since this aspect is of that type. 
        /// </summary>
        public override Type aspectType
        {
            get
            {
                return typeof(int);
            }
        }

        /// <summary>
        /// Does this object have value?
        /// </summary>
        public override bool hasValue
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets the int value of this property.
        /// </summary>
        public override int intValue
        {
            get
            {
                return m_Value;
            }

            set
            {
                if (m_Value != value)
                {
                    m_Value = value;
                    m_IsDirty = true;
                }
            }
        }
        /// <summary>
        /// Returns the raw value of the object.
        /// </summary>
        public override object value
        {
            get
            {
                return m_Value;
            }
        }

        /// <summary>
        /// Is this object a value type?
        /// </summary>
        protected override bool isValueType
        {
            get { return true; }
        }
        public override void OnGUI()
        {
            GUILayout.BeginHorizontal();
            {
                intValue = EditorGUILayout.IntField(memberName, m_Value);
                base.OnGUI();
            }
            GUILayout.EndHorizontal();

        }

        /// <summary>
        /// A function used to copy all members to a copy of this class. 
        /// </summary>
        protected override void CloneMembers(MemberAspect clone)
        {
            IntAspect intAspect = clone as IntAspect;
            intAspect.m_Value = m_Value;
        }

        /// <summary>
        /// Loads a value from a target object if a field name
        /// with the same type exists on that object. 
        /// </summary>
        /// <param name="loadFrom">The object you want to load this members value from.</param>
        internal override void LoadValue(object loadFrom)
        {
            if (loadFrom == null)
            {
                throw new System.NullReferenceException("Reflected Inspector: Can't load value from a null object");
            }

            m_Value = (int)fieldInfo.GetValue(loadFrom);
            base.LoadValue(loadFrom);
        }
    }
}
