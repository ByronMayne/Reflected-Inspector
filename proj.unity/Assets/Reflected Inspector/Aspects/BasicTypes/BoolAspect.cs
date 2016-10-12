using System;
using System.Reflection;
using TinyJSON;
using UnityEditor;
using UnityEngine;
using Type = System.Type;


namespace ReflectedInspector
{
    public class BoolAspect : MemberAspect
    {
        /// <summary>
        /// The true value that this class holds.
        /// </summary>
        [Include]
        private bool m_Value;

        public BoolAspect(ReflectedObject reflectedObject, FieldInfo field) : base(reflectedObject, field)
        {
        }

        /// <summary>
        /// Returns back typeof(bool) since this aspect is of that type. 
        /// </summary>
        public override Type aspectType
        {
            get
            {
                return typeof(bool);
            }
        }

        /// <summary>
        /// Gets or sets the int value of this property.
        /// </summary>
        public override bool boolValue
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
        /// Does this object have value?
        /// </summary>
        public override bool hasValue
        {
            get { return true; }
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

            m_Value = (bool)fieldInfo.GetValue(loadFrom);
            base.LoadValue(loadFrom);
        }

        public override void OnGUI()
        {
            GUILayout.BeginHorizontal();
            {
                boolValue = EditorGUILayout.Toggle(memberName, m_Value);
                base.OnGUI();
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// A function used to copy all members to a copy of this class. 
        /// </summary>
        protected override void CloneMembers(MemberAspect clone)
        {
            BoolAspect boolAspect = clone as BoolAspect;
            boolAspect.m_Value = m_Value;
        }
    }
}
