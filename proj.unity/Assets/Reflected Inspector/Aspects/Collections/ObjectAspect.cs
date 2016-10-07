using Type = System.Type;
using Attribute = System.Attribute;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using UnityEngine;
using TinyJSON;

namespace ReflectedInspector
{
    public class ObjectAspect : MemberAspect
    {

        /// <summary>
        /// The style we use to draw null values.
        /// </summary>
        private static GUIStyle m_NullStyle;

        /// <summary>
        /// The root value of our object
        /// </summary>
        private object m_Value;

        /// <summary>
        /// Is this class serializable by Unity?
        /// </summary>
        private bool m_IsUnitySerializable = false;

        /// <summary>
        /// Is this item expanded in the editor?
        /// </summary>
        private bool m_IsExpanded = false;

        /// <summary>
        /// The true value that this class holds.
        /// </summary>
        [Include]
        private List<MemberAspect> m_Children;

        /// <summary>
        /// The base type of the object
        /// </summary>
        private Type m_FieldType;

        /// <summary>
        /// The true type of the object.
        /// </summary>
        private Type m_ValueType;

        /// <summary>
        /// Returns back typeof(List<ArrayAspect>) since this aspect is of that type. 
        /// </summary>
        public override Type aspectType
        {
            get
            {
                return typeof(object);
            }
        }

        /// <summary>
        /// Returns the size of this array object.
        /// </summary>
        public override int elementCount
        {
            get
            {
                return m_Children.Count;
            }
        }

        /// <summary>
        /// Does this object have value?
        /// </summary>
        public override bool hasValue
        {
            get { return m_Value != null || m_FieldType.IsValueType; }
        }

        /// <summary>
        /// Is this object a value type?
        /// </summary>
        protected override bool isValueType
        {
            get { return m_FieldType.IsValueType; }
        }


        public override object value
        {
            get
            {
                return m_Value;
            }
        }

        /// <summary>
        /// Returns true if this type can be serialized by Unity false if it can not. 
        /// </summary>
        protected override bool isSerializableByUnity
        {
            get
            {
                return m_IsUnitySerializable;
            }
        }

        public ObjectAspect(ReflectedAspect objectAspect, string aspectPath) : base(objectAspect, aspectPath)
        {
        }

        /// <summary>
        /// Called when this object should be loaded from disk.
        /// </summary>
        protected override void LoadValue()
        {
            // We can only load children if we have a value.
            m_Children = new List<MemberAspect>();

            bool wasSuccessful = false;
            FieldInfo fieldInfo;
            m_Value = ReflectionHelper.GetFieldValue(aspectPath, reflectedAspect.targets[0], out wasSuccessful, out fieldInfo);
            m_FieldType = fieldInfo.FieldType;

            if (wasSuccessful && hasValue)
            {
                m_ValueType = m_Value.GetType();
            }
            else
            {
                m_ValueType = m_FieldType;
            }

            // Unity can't serialize interfaces
            m_IsUnitySerializable = !m_ValueType.IsInterface;
            // Or generic types
            m_IsUnitySerializable &= !m_ValueType.IsGenericType;

            if (m_IsUnitySerializable)
            {
                //TODO: Add a case for type of List<T>

                if (typeof(MonoBehaviour).IsAssignableFrom(m_FieldType))
                {
                    // It's a component.
                    m_IsUnitySerializable = true;
                }

                Attribute[] attributes = fieldInfo.GetCustomAttributes(true) as Attribute[];

                for(int i = 0; i < attributes.Length; i++)
                {
                    Type attributeType = attributes[i].GetType();

                    if( attributeType == typeof(System.SerializableAttribute))
                    {
                        m_IsUnitySerializable = true;
                    }

                    if( attributeType == typeof(System.NonSerializedAttribute))
                    {
                        m_IsUnitySerializable = false;
                        break;
                    }
                }
            }

            LoadChildren();
        }

        /// <summary>
        /// Iterates over a all elements.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<MemberAspect> GetIterator()
        {
            // We never return our self. 
            for (int i = 0; i < m_Children.Count; i++)
            {
                yield return m_Children[i];
            }
        }

        /// <summary>
        /// Loads all the children from our current value.
        /// </summary>
        private void LoadChildren()
        {
            if (hasValue)
            {
                FieldInfo[] fileds = m_ValueType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                for (int i = 0; i < fileds.Length; i++)
                {
                    Type fieldType = fileds[i].FieldType;

                    MemberAspect member = reflectedAspect.CreateAspectForType(fieldType, aspectPath + "." + fileds[i].Name);

                    m_Children.Add(member);
                }
            }
        }

        public override MemberAspect FindFieldRelative(string relativeName)
        {
            for (int i = 0; i < m_Children.Count; i++)
            {
                if (string.CompareOrdinal(relativeName, m_Children[i].aspectPath) == 0)
                {
                    return m_Children[i];
                }
            }
            return null;
        }

        /// <summary>
        /// A function used to copy all members to a copy of this class. 
        /// </summary>
        protected override void CloneMembers(MemberAspect clone)
        {
            ObjectAspect objectAspect = clone as ObjectAspect;
            for (int i = 0; i < m_Children.Count; i++)
            {
                MemberAspect childClone = m_Children[i].Copy();
                objectAspect.m_Children.Add(childClone);
            }
        }

        /// <summary>
        /// Handles the drawing logic for this array and all it's children.
        /// </summary>
        public override void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (hasValue)
                {
                    m_IsExpanded = EditorGUILayout.Foldout(m_IsExpanded, memberName);
                }
                else
                {
                    if (m_NullStyle == null)
                    {
                        m_NullStyle = new GUIStyle("ChannelStripAttenuationMarkerSquare");
                        m_NullStyle.fontStyle = FontStyle.Italic;
                    }
                    EditorGUILayout.LabelField(memberName, "Null", m_NullStyle, GUILayout.ExpandWidth(false));
                    GUILayout.FlexibleSpace();
                }
                base.OnGUI();
            }
            EditorGUILayout.EndHorizontal();

            if (m_IsExpanded)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < m_Children.Count; i++)
                {
                    m_Children[i].OnGUI();
                }
                EditorGUI.indentLevel--;
            }
        }

        protected override void OnAspectSetToNull()
        {
            m_Value = null;
            m_Children.Clear();
        }

        protected override void OnNewValueLoaded()
        {
            m_Value = System.Activator.CreateInstance(m_FieldType, nonPublic: true);
            LoadChildren();
        }

        protected override void OnPoloymorphicTypeAssigned()
        {
            m_Value = System.Activator.CreateInstance(m_ValueType, nonPublic: true);
            LoadChildren();
        }
    }
}
