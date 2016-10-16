using Type = System.Type;
using Attribute = System.Attribute;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using UnityEngine;
using TinyJSON;
using System;
using System.Linq;

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

        /// <summary>
        /// Gets the raw value of this object. 
        /// </summary>
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
            get { return fieldInfo.FieldType.IsValueType; }
        }

        /// <summary>
        /// Finds a child with the name. 
        /// </summary>
        /// <param name="relativeName">The name you want to find.</param>
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
                    GUILayout.Label(memberName);
                }

                Rect buttonRect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.popup);

                if (hasValue)
                {
                    GUI.Label(buttonRect, m_ValueType.FullName, EditorStyles.popup);
                }
                else
                {
                    GUI.Label(buttonRect, "Null", EditorStyles.popup);
                }

                if(Event.current.type == EventType.MouseDown && buttonRect.Contains(Event.current.mousePosition))
                {
                    ShowTypeOptions();

                }
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

        /// <summary>
        /// Loads this objects value from a target. 
        /// </summary>
        /// <param name="loadFrom">The object we want to load the values from.</param>
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

            // Load all our fields. 
            LoadChildren();
        }

        /// <summary>
        /// Deletes our value and destroys all it's children.
        /// </summary>
        protected override void DestroyInstance()
        {
            m_Value = null;
            m_Children.Clear();
        }

        /// <summary>
        /// Creates a new instance for out type . 
        /// </summary>
        protected override void RevertTOFieldType()
        {
            m_ValueType = fieldInfo.FieldType;
            m_Value = System.Activator.CreateInstance(fieldInfo.FieldType, nonPublic: true);
            LoadChildren();
        }

        /// <summary>
        /// Creates a menu item to show all the types that this object can be. Lets the user pick one
        /// and converts our object to that type. 
        /// </summary>
        protected override void ShowTypeOptions()
        {

            Assembly assembly = Assembly.GetCallingAssembly();

            Type[] types = assembly.GetTypes();
            GenericMenu typesMenu = new GenericMenu();
            typesMenu.AddItem(new GUIContent("Null"), false, DestroyInstance);
            for (int i = 0; i < types.Length; i++)
            {
                if (!types[i].IsAbstract && fieldInfo.FieldType.IsAssignableFrom(types[i]))
                {
                    Type type = types[i];
                    typesMenu.AddItem(new GUIContent(types[i].Name), false, ()=> { AssignType(type); });
                }
            }

            typesMenu.ShowAsContext();
        }

        /// <summary>
        /// Used to assign our object a type if our value is currently null this creates
        /// a new instance otherwise it will convert our type. 
        /// </summary>
        /// <param name="type"></param>
        public void AssignType(Type type)
        {
            if(!hasValue)
            {
                CreateNewInstanceOfType(type);
            }
            else if( type != m_ValueType )
            {
                ConvertType(type);
            }
        }

        /// <summary>
        /// Creates a new instance to overwrite our old one. If any value has already
        /// been set it will be cleared.
        /// </summary>
        /// <param name="type">The new type instance you want to create</param>
        private void CreateNewInstanceOfType(Type type)
        {
            m_ValueType = type;
            m_Value = System.Activator.CreateInstance(type, nonPublic: true);
            LoadChildren();
        }

        /// <summary>
        /// Takes our current instance and converts it into a new type. This preserves all values
        /// that have the same name. If we don't currently value a value you should call
        /// <see cref="CreateNewInstanceOfType(Type)"/>. If our value is already that type this
        /// function has no effect. 
        /// </summary>
        /// <param name="newType">The type you want to convert too.</param>
        protected virtual void ConvertType(Type newType)
        {
            // Our new type is null so we don't need to convert.
            if (newType == null)
            {
                DestroyInstance();
                return;
            }

            // We are already this type no point running our logic.
            if (newType == m_ValueType)
            {
                return;
            }

            // We have not type assigned so we can just create a new one.
            if (!hasValue)
            {
                Debug.LogWarning("Can't Convert Type if we have no current instance set");
                CreateNewInstanceOfType(newType);
            }

            FieldInfo[] newFields = newType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // First remove extra fields
            for (int i = m_Children.Count - 1; i >= 0; i--)
            {
                if (!newFields.Any(nf => { return nf.Name == m_Children[i].fieldInfo.Name; }))
                {
                    // This field does not exist on our new type.
                    m_Children.RemoveAt(i);
                }
            }

            List<MemberAspect> sortedChildren = new List<MemberAspect>();

            for (int i = 0; i < newFields.Length; i++)
            {
                // Find this asepct if we have one
                MemberAspect child = m_Children.Where(aspect => { return aspect.fieldInfo.Name == newFields[i].Name; }).FirstOrDefault();

                if (child != null)
                {
                    sortedChildren.Add(child);
                }
                else
                {
                    MemberAspect member = reflectedObject.CreateAspect(newFields[i], null);

                    sortedChildren.Add(member);
                }
            }

            // Save our children  
            m_Children = sortedChildren;
            // Save our new type.
            m_ValueType = newType;
            // Create a new value. 
            m_Value = System.Activator.CreateInstance(m_ValueType, nonPublic: true);
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
