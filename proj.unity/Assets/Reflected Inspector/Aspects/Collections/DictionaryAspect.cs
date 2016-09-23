using Type = System.Type;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor;
using System.Collections;
using System.Reflection;
using TinyJSON;
using UnityEngine;

namespace ReflectedInspector
{
    public class DictionaryAspect : CollectionAspect
    {
        /// <summary>
        /// Is this item expanded in the editor?
        /// </summary>
        private bool m_IsExpanded = false;

        private int m_ArraySize = 0;

        private object m_Value;

        /// <summary>
        /// The true value that this class holds.
        /// </summary>
        [Include]
        private Dictionary<MemberAspect, MemberAspect> m_Children;

        /// <summary>
        /// Returns back typeof(List<ArrayAspect>) since this aspect is of that type. 
        /// </summary>
        public override Type aspectType
        {
            get
            {
                return typeof(List<ArrayAspect>);
            }
        }

        public DictionaryAspect(ReflectedAspect objectAspect, string aspectPath) : base(objectAspect, aspectPath)
        {
        }

        /// <summary>
        /// Does this object have value?
        /// </summary>
        public override bool hasValue
        {
            get { return m_Children != null; }
        }

        /// <summary>
        /// Returns the number of elements in this collection.
        /// </summary>
        protected override int collectionCount
        {
            get
            {
                return m_Children.Count;
            }
        }

        /// <summary>
        /// Returns the type of this collection.
        /// </summary>
        protected override Type collectionType
        {
            get
            {
                return m_PolymorphicType ?? m_FieldType;
            }
        }

        /// <summary>
        /// Returns that list value of this collection.
        /// </summary>
        public override object value
        {
            get
            {
                return m_Children;
            }
        }

        /// <summary>
        /// Called when this object should be loaded from disk.
        /// </summary>
        protected override void LoadValue()
        {
            m_Children = new Dictionary<MemberAspect, MemberAspect>();

            bool wasSuccessful = false;
            FieldInfo field;
            m_Value = ReflectionHelper.GetFieldValue(aspectPath, reflectedAspect.targets[0], out wasSuccessful, out field);
            m_FieldType = field.FieldType.GetGenericArguments()[1];


            if (wasSuccessful)
            {
                IDictionary dictionary = m_Value as IDictionary;
                var keys = dictionary.Keys;
                foreach (DictionaryEntry entry in dictionary)
                {
                    string path = SequenceHelper.AppendDictionaryEntryToSequence(aspectPath, entry.Key.ToString());
                    StringAspect key = new StringAspect(reflectedAspect, null);
                    key.stringValue = entry.Key.ToString();
                    key.memberName = "Key";
                    MemberAspect child = reflectedAspect.CreateAspectForType(m_FieldType, path);
                    child.memberName = "Value";
                    m_Children.Add(key, child);
                    m_ElementCount++;
                }
            }
        }

        public override void SaveValue()
        {
        }

        /// <summary>
        /// A function used to copy all members to a copy of this class. 
        /// </summary>
        protected override void CloneMembers(MemberAspect clone)
        {
            DictionaryAspect arrayAspect = clone as DictionaryAspect;
            arrayAspect.m_ArraySize = m_ArraySize;
            for (int i = 0; i < m_Children.Count; i++)
            {
                // MemberAspect childClone = m_Children[i].Copy();
                //arrayAspect.m_Children.Add(childClone);
            }
        }

        protected override void OnDrawContent()
        {
            elementCount = EditorGUILayout.IntField("Size", elementCount);

            GUILayout.BeginVertical(GUI.skin.box);
            {
                foreach (var it in m_Children)
                {
                    it.Key.OnGUILayout();

                    Rect valueRect = EditorGUILayout.BeginVertical();
                    {
                        EditorGUI.indentLevel++;
                        it.Value.OnGUILayout();
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();
                    valueRect.x = (EditorGUI.indentLevel + 1) * 16f;
                    GUI.Box(valueRect, GUIContent.none);
                }
            }
            GUILayout.EndVertical();
        }

        /// <summary>
        /// Returns the last element in our list.
        /// </summary>
        protected override MemberAspect GetLastElement()
        {
            return null;
        }
    }
}
