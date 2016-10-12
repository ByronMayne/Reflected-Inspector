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
        /// The true value that this class holds.
        /// </summary>
        [Include]
        private List<MemberAspect> m_Children;

        /// <summary>
        /// Is this item expanded in the editor?
        /// </summary>
        private bool m_IsExpanded = false;

        /// <summary>
        /// Is this class serializable by Unity?
        /// </summary>
        private bool m_IsUnitySerializable = false;

        /// <summary>
        /// The root value of our object
        /// </summary>
        private object m_Value;
        /// <summary>
        /// The true type of the object.
        /// </summary>
        private Type m_ValueType;

        public ObjectAspect(ReflectedObject reflectedObject, FieldInfo field) : base(reflectedObject, field)
        {
        }

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
            get { return m_Value != null; }
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

        /// <summary>
        /// Is this object a value type?
        /// </summary>
        protected override bool isValueType
        {
            get { return false; }
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

        internal override void LoadValue(object loadFrom)
        {
            m_Children = new List<MemberAspect>();
            m_Value = fieldInfo.GetValue(loadFrom);

            if (m_Value != null)
            {
                m_ValueType = m_Value.GetType();
            }
            else
            {
                m_ValueType = fieldInfo.FieldType;
            }

            // Set our flag if Unity can serialize us.
            CheckIfSerializableByUnity();
            // Load all our fields. 
            LoadChildren();
        }

        /// <summary>
        /// Checks our value type to see if Unity can serialize it.
        /// </summary>
        private void CheckIfSerializableByUnity()
        {
            // Unity can't serialize interfaces
            m_IsUnitySerializable = !m_ValueType.IsInterface;
            // Or generic types
            m_IsUnitySerializable &= !m_ValueType.IsGenericType;

            if (m_IsUnitySerializable)
            {
                if (typeof(MonoBehaviour).IsAssignableFrom(fieldInfo.FieldType))
                {
                    // It's a component.
                    m_IsUnitySerializable = true;
                }

                Attribute[] attributes = fieldInfo.GetCustomAttributes(true) as Attribute[];

                for (int i = 0; i < attributes.Length; i++)
                {
                    Type attributeType = attributes[i].GetType();

                    if (attributeType == typeof(System.SerializableAttribute))
                    {
                        m_IsUnitySerializable = true;
                    }

                    if (attributeType == typeof(System.NonSerializedAttribute))
                    {
                        m_IsUnitySerializable = false;
                        break;
                    }
                }
            }
        }

        internal override void SaveValue(object saveTo)
        {
            base.SaveValue(saveTo);
        }

        protected override void OnAspectSetToNull()
        {
            m_Value = null;
            m_Children.Clear();
        }

        protected override void CreateNewValue()
        {
            m_ValueType = fieldInfo.FieldType;
            m_Value = System.Activator.CreateInstance(fieldInfo.FieldType, nonPublic: true);
            LoadChildren();
        }

        protected override void CreatePoloymorphicValue()
        {

            Assembly assembly = Assembly.GetCallingAssembly();

            Type[] types = assembly.GetTypes();
            GenericMenu typesMenu = new GenericMenu();
            for(int i = 0; i < types.Length; i++)
            {
                if(!types[i].IsAbstract && fieldInfo.FieldType.IsAssignableFrom(types[i]))
                {
                    typesMenu.AddItem(new GUIContent(types[i].Name), false, OnPolomorphicTypeChagned, types[i]);
                }
            }

            typesMenu.ShowAsContext();
        }

        private void OnPolomorphicTypeChagned(object value)
        {
            m_ValueType = (Type)value;
            m_Value = System.Activator.CreateInstance((Type)value, nonPublic: true);
            
            LoadChildren();
        }

        /// <summary>
        /// Loads all the children from our current value.
        /// </summary>
        private void LoadChildren()
        {
            if (hasValue)
            {
                FieldInfo[] fields = m_ValueType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                for (int i = 0; i < fields.Length; i++)
                {
                    object value = fields[i].GetValue(m_Value);

                    MemberAspect member = reflectedObject.CreateAspect(fields[i], value);

                    m_Children.Add(member);
                }
            }
        }
    }
}
