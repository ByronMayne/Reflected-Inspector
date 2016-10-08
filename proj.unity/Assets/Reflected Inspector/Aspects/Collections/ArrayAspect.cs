using Type = System.Type;
using System.Collections.Generic;
using UnityEditor;
using System.Collections;
using System.Reflection;
using TinyJSON;
using System;

namespace ReflectedInspector
{
    public class ArrayAspect : CollectionAspect
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
        private List<MemberAspect> m_Children;

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

        /// <summary>
        /// Invoked whenever we add an element to our array.
        /// </summary>
        public override void InsetArrayElementAtIndex(int index)
        {
            base.InsetArrayElementAtIndex(index);
            UpdateElementNames();
            m_IsDiry = true;
        }

        /// <summary>
        /// Invoked whenver we remove an element from our array.
        /// </summary>
        public override void DeleteArrayElmentAtIndex(int index)
        {
            if (index < m_Children.Count)
            {
                MemberAspect child = m_Children[index];
                m_Children.Remove(child);
                reflectedAspect.RemoveAspect(child);
                child = null;
            }
            base.DeleteArrayElmentAtIndex(index);
            UpdateElementNames();
            m_IsDiry = true;
        }


        /// <summary>
        /// Gets the element in the array at the index provided.
        /// </summary>
        public override MemberAspect this[int index]
        {
            get
            {
                return m_Children[index];
            }
            protected set
            {
                if (index >= m_Children.Count)
                {
                    m_Children.Add(value);
                }
                else
                {
                    m_Children.Insert(index, value);
                }
            }
        }

        public override IEnumerator<MemberAspect> GetIterator()
        {
            base.GetIterator();
            for (int i = 0; i < m_Children.Count; i++)
            {
                yield return m_Children[i];
            }
        }


        public ArrayAspect(ReflectedAspect objectAspect, string aspectPath) : base(objectAspect, aspectPath)
        {
            LoadChildren();
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
                return m_Value;
            }
        }

        /// <summary>
        /// Updates all the elements in our array to have the correct name.
        /// </summary>
        private void UpdateElementNames()
        {
            for (int i = 0; i < m_Children.Count; i++)
            {
                m_Children[i].SetAspectPath(SequenceHelper.AppendListEntryToSequence(aspectPath, i));
            }
        }

        /// <summary>
        /// Loads all the children from our current value.
        /// </summary>
        private void LoadChildren()
        {
            m_ElementCount = 0;
            m_Children = new List<MemberAspect>();

            if (hasValue)
            {
                IList list = m_Value as IList;

                for (int i = 0; i < list.Count; i++)
                {
                    Type fieldType = list[i].GetType();

                    string sequencePath = SequenceHelper.AppendListEntryToSequence(aspectPath, i);
                    MemberAspect member = reflectedAspect.CreateAspectForType(fieldType, sequencePath);

                    m_Children.Add(member);
                    m_ElementCount++;
                }
            }
        }

        public override void SaveValue()
        {
            if (m_IsDiry)
            {
                // Get the true type that we are using. 
                Type workingType = GetWorkingType();
                // Create a new instance
                IList newList = Activator.CreateInstance(workingType) as IList;
                // Get our default value for our new array
                object defaultValue = ReflectionHelper.GetDefault(workingType);
                // Insert null values for each of our children.
                for (int i = 0; i < m_Children.Count; i++)
                {
                    // Insert null for class types and default for structs. 
                    newList.Add(defaultValue);
                }
                // Set our local value.
                m_Value = newList;
                // Call the base to write it to our field.
                base.SaveValue();
            }
        }

        /// <summary>
        /// Called when this object should be loaded from disk.
        /// </summary>
        protected override void LoadValue()
        {
            m_Children = new List<MemberAspect>();

            bool wasSuccessful = false;
            FieldInfo field;
            m_Value = ReflectionHelper.GetFieldValue(aspectPath, reflectedAspect.targets[0], out wasSuccessful, out field);
            m_FieldType = field.FieldType;

            if (wasSuccessful)
            {
                IList list = m_Value as IList;
                m_PolymorphicType = ReflectionHelper.GetElementType(list);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] != null)
                    {
                        string path = SequenceHelper.AppendListEntryToSequence(aspectPath, i);
                        MemberAspect member = reflectedAspect.CreateAspectForType(list[i].GetType(), path);
                        m_Children.Add(member);
                    }
                }
            }
        }

        /// <summary>
        /// A function used to copy all members to a copy of this class. 
        /// </summary>
        protected override void CloneMembers(MemberAspect clone)
        {
            ArrayAspect arrayAspect = clone as ArrayAspect;
            arrayAspect.m_ArraySize = m_ArraySize;
            for (int i = 0; i < m_Children.Count; i++)
            {
                MemberAspect childClone = m_Children[i].Copy();
                arrayAspect.m_Children.Add(childClone);
            }
        }

        protected override void OnDrawContent()
        {
            elementCount = EditorGUILayout.IntField("Size", elementCount);
            for (int i = 0; i < m_Children.Count; i++)
            {
                m_Children[i].OnGUI();
            }
        }

        /// <summary>
        /// Returns the last element in our list.
        /// </summary>
        protected override MemberAspect GetLastElement()
        {
            return m_Children.Count > 0 ? m_Children[m_Children.Count - 1] : null;
        }
    }
}
