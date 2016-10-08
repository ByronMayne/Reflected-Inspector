﻿using System;
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
                    m_IsDiry = true;
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
        /// Is this object a value type?
        /// </summary>
        protected override bool isValueType
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

        public BoolAspect(ReflectedAspect objectAspect, string aspectPath) : base(objectAspect, aspectPath)
        {

        }

        /// <summary>
        /// Called when this object should be loaded from disk.
        /// </summary>
        protected override void LoadValue()
        {
            m_Value = ReflectionHelper.GetFieldValue<bool>(aspectPath, reflectedAspect.targets[0]);
        }


        /// <summary>
        /// A function used to copy all members to a copy of this class. 
        /// </summary>
        protected override void CloneMembers(MemberAspect clone)
        {
            BoolAspect boolAspect = clone as BoolAspect;
            boolAspect.m_Value = m_Value;
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
    }
}
