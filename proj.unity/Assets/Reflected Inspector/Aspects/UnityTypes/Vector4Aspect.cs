﻿using System.Reflection;
using TinyJSON;
using UnityEditor;
using UnityEngine;
using Type = System.Type;


namespace ReflectedInspector
{
    public class Vector4Aspect : MemberAspect
    {
        /// <summary>
        /// The true value that this class holds.
        /// </summary>
        [Include]
        private Vector4 m_Value;

        public Vector4Aspect(ReflectedObject reflectedObject, FieldInfo field) : base(reflectedObject, field)
        {
        }

        /// <summary>
        /// Returns back typeof(float) since this aspect is of that type. 
        /// </summary>
        public override Type aspectType
        {
            get
            {
                return typeof(Vector4);
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
        /// Gets the color value for this class.
        /// </summary>
        public override Vector4 vector4Value
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
        /// Is this object a value type?
        /// </summary>
        protected override bool isValueType
        {
            get { return true; }
        }
        public override void OnGUI()
        {
            vector4Value = EditorGUILayout.Vector4Field(memberName, m_Value);
        }

        /// <summary>
        /// A function used to copy all members to a copy of this class. 
        /// </summary>
        protected override void CloneMembers(MemberAspect clone)
        {
            Vector4Aspect vect4Aspect = clone as Vector4Aspect;
            vect4Aspect.m_Value = m_Value;
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

            m_Value = (Vector4)fieldInfo.GetValue(loadFrom);
            base.LoadValue(loadFrom);
        }
    }
}
