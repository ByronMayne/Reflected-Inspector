﻿using System;
using UnityEditor;
using UnityEngine;
using Type = System.Type;


namespace ReflectedInspector
{
    public class BoundsAspect : MemberAspect
    {
        /// <summary>
        /// The true value that this class holds.
        /// </summary>
        private Bounds m_Value;

        /// <summary>
        /// Returns back typeof(float) since this aspect is of that type. 
        /// </summary>
        public override Type aspectType
        {
            get
            {
                return typeof(Bounds);
            }
        }

        /// <summary>
        /// Gets the color value for this class.
        /// </summary>
        public override Bounds boundsValue
        {
            get
            {
                return m_Value;
            }

            set
            {
                m_Value = value;
            }
        }

        /// <summary>
        /// Does this object have value?
        /// </summary>
        protected override bool hasValue
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

        public BoundsAspect(ReflectedAspect objectAspect, string aspectPath) : base(objectAspect, aspectPath)
        {
        }


        /// <summary>
        /// Called when this object should be loaded from disk.
        /// </summary>
        protected override void LoadValue()
        {
            m_Value = ReflectionHelper.GetFieldValue<Bounds>(aspectPath, reflectedAspect.targets[0]);
        }

        /// <summary>
        /// A function used to copy all members to a copy of this class. 
        /// </summary>
        protected override void CloneMembers(MemberAspect clone)
        {
            BoundsAspect boundsAspect = clone as BoundsAspect;
            boundsAspect.m_Value = m_Value;
        }

        public override void OnGUILayout()
        {
            m_Value = EditorGUILayout.BoundsField(memberName, m_Value);
        }
    }
}