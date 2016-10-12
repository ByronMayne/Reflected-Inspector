//using Type = System.Type;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using UnityEditor;
//using System.Collections;
//using System.Reflection;
//using TinyJSON;
//using UnityEngine;

//namespace ReflectedInspector
//{
//    public struct AspectPair
//    {
//        public MemberAspect Key;

//        public MemberAspect Value;

//        public AspectPair(MemberAspect key, MemberAspect value)
//        {
//            Key = key;
//            Value = value;
//        }
//    }

//    public class DictionaryAspect : CollectionAspect
//    {
//        private int m_ArraySize = 0;

//        /// <summary>
//        /// The true value that this class holds.
//        /// </summary>
//        [Include]
//        private List<AspectPair> m_Children;

//        /// <summary>
//        /// Is this item expanded in the editor?
//        /// </summary>
//        private bool m_IsExpanded = false;

//        private GUIStyle m_Skin = null;

//        private bool m_SkinCreated = false;

//        private object m_Value;


//        public DictionaryAspect(reflectedObject reflectedObject, FieldInfo field) : base(reflectedObject, field)
//        {
//        }

//        /// <summary>
//        /// Returns back typeof(List<ArrayAspect>) since this aspect is of that type. 
//        /// </summary>
//        public override Type aspectType
//        {
//            get
//            {
//                return typeof(List<ArrayAspect>);
//            }
//        }

//        /// <summary>
//        /// Does this object have value?
//        /// </summary>
//        public override bool hasValue
//        {
//            get { return m_Children != null; }
//        }

//        /// <summary>
//        /// Returns that list value of this collection.
//        /// </summary>
//        public override object value
//        {
//            get
//            {
//                return m_Children;
//            }
//        }

//        /// <summary>
//        /// Returns the number of elements in this collection.
//        /// </summary>
//        protected override int collectionCount
//        {
//            get
//            {
//                return m_Children.Count;
//            }
//        }

//        /// <summary>
//        /// Returns the type of this collection.
//        /// </summary>
//        protected override Type collectionType
//        {
//            get
//            {
//                return m_PolymorphicType ?? m_FieldType;
//            }
//        }
//        public override void SaveValue()
//        {
//        }

//        /// <summary>
//        /// A function used to copy all members to a copy of this class. 
//        /// </summary>
//        protected override void CloneMembers(MemberAspect clone)
//        {
//            DictionaryAspect arrayAspect = clone as DictionaryAspect;
//            arrayAspect.m_ArraySize = m_ArraySize;
//            for (int i = 0; i < m_Children.Count; i++)
//            {
//                // MemberAspect childClone = m_Children[i].Copy();
//                //arrayAspect.m_Children.Add(childClone);
//            }
//        }

//        /// <summary>
//        /// Returns the last element in our list.
//        /// </summary>
//        protected override MemberAspect GetLastElement()
//        {
//            return m_Children.Count > 0 ? m_Children[m_Children.Count - 1].Value : null;
//        }

//        /// <summary>
//        /// Called when this object should be loaded from disk.
//        /// </summary>
//        protected override void LoadValue()
//        {
//            m_Children = new List<AspectPair>();

//            bool wasSuccessful = false;
//            FieldInfo field;
//            m_Value = ReflectionHelper.GetFieldValue(aspectPath, reflectedObject.targets[0], out wasSuccessful, out field);
//            m_FieldType = field.FieldType.GetGenericArguments()[1];


//            if (wasSuccessful)
//            {
//                IDictionary dictionary = m_Value as IDictionary;
//                var keys = dictionary.Keys;
//                foreach (DictionaryEntry entry in dictionary)
//                {
//                    string path = SequenceHelper.AppendDictionaryEntryToSequence(aspectPath, entry.Key.ToString());
//                    StringAspect key = new StringAspect(reflectedObject, null);
//                    key.stringValue = entry.Key.ToString();
//                    key.memberName = null;
//                    MemberAspect value = reflectedObject.CreateAspectForType(m_FieldType, path);
//                    value.memberName = "Value";
//                    m_Children.Add(new AspectPair(key, value));
//                    m_ElementCount++;
//                }
//            }
//        }
//        protected override void OnDrawContent()
//        {
//            if(!m_SkinCreated)
//            {
//                m_Skin = new GUIStyle(GUI.skin.window);
//                m_Skin.stretchHeight = false;
//                m_Skin.padding.top = 0;
//            }

//            elementCount = EditorGUILayout.IntField("Size", elementCount);

//            for (int i = 0; i < m_Children.Count; i++)
//            {
//                GUILayout.BeginVertical(m_Skin);
//                {
//                    MemberAspect key = m_Children[i].Key;
//                    MemberAspect value = m_Children[i].Value;

//                    EditorGUIUtility.labelWidth = 30;
//                    EditorGUIUtility.fieldWidth = 1;
//                    key.isExpanded = EditorGUILayout.Foldout(key.isExpanded, GUIContent.none);
//                    EditorGUI.indentLevel++;
//                    GUILayout.Space(-EditorGUIUtility.singleLineHeight);
//                    key.OnGUI();
//                    EditorGUIUtility.labelWidth = -1;
//                    EditorGUIUtility.fieldWidth = -1;
//                    if (key.isExpanded)
//                    {
//                        value.OnGUI();
//                    }
//                    EditorGUI.indentLevel--;
//                }
//                GUILayout.EndVertical();
//            }
//            GUI.backgroundColor = Color.white;
//        }
//    }
//}
