using Type = System.Type;
using System.Collections.Generic;
using UnityEditor;
using System;
using System.Collections;

namespace ReflectedInspector
{
    public abstract class CollectionAspect : MemberAspect
    {
        /// <summary>
        /// The base type that is collection is.
        /// </summary>
        protected Type m_FieldType;

        /// <summary>
        /// The polymorphic type of this collection.
        /// </summary>
        protected Type m_PolymorphicType;

        /// <summary>
        /// Is this item expanded in the editor?
        /// </summary>
        private bool m_IsExpanded = false;

        /// <summary>
        /// How many elements are currently in this collection
        /// </summary>
        private int m_ElementCount;

        /// <summary>
        /// Gets or set the type that is contained in this collection.
        /// </summary>
        public Type fieldType
        {
            get { return m_FieldType; }
        }

        /// <summary>
        /// Gets or sets the polymorphic type.
        /// </summary>
        public Type polymorphicType
        {
            get { return m_PolymorphicType; }
        }

        /// <summary>
        /// The default constructor for this class.
        /// </summary>
        public CollectionAspect(ReflectedAspect reflectedAspect, string aspectPath) : base(reflectedAspect, aspectPath)
        {
        }

        /// <summary>
        /// Returns the number of elements in this collection.
        /// </summary>
        protected abstract int collectionCount { get; }

        /// <summary>
        /// Gets the last element in this collection, null if it 
        /// does not have any elements.
        /// </summary>
        /// <returns></returns>
        protected abstract MemberAspect GetLastElement();

        /// <summary>
        /// Returns the number of elements in this collection.
        /// </summary>
        public override int elementCount
        {
            get
            {
                return m_ElementCount;
            }
            set
            {
                if (value < 0)
                {
                    // We can't have negative values. 
                    value = 0;
                }

                if (collectionCount != value)
                {

                    // We are setting our element count and need to add or remove elements to get
                    // to the same size.
                    int collectionSizeDifference = value - collectionCount;

                    if (collectionSizeDifference > 0)
                    {
                        for (int i = 0; i < collectionSizeDifference; i++)
                        {
                            // Adding
                            InsetArrayElementAtIndex(elementCount);
                        }
                    }
                    else
                    {
                        for (int i = collectionSizeDifference; i < 0; i++)
                        {
                            // Removing
                            DeleteArrayElmentAtIndex(elementCount - 1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the type that this collection is
        /// </summary>
        protected abstract Type collectionType { get; }

        /// <summary>
        /// Creates a new element at a set index.
        /// </summary>
        /// <param name="index"></param>
        public override void InsetArrayElementAtIndex(int index)
        {
            MemberAspect child = GetLastElement();

            if (child != null)
            {
                // Just copy the last element
                child = child.Copy();
            }
            else
            {
                child = reflectedAspect.CreateAspectForType(collectionType, SequenceHelper.AppendListEntryToSequence(aspectPath, elementCount));
            }
            this[index] = child;
            m_ElementCount++;
        }

        /// <summary>
        /// Invoked whenever we remove an element from our collection
        /// </summary>
        /// <param name="index"></param>
        public override void DeleteArrayElmentAtIndex(int index)
        {
            m_ElementCount--;
        }

        /// <summary>
        /// Is this object a value type?
        /// </summary>
        protected override bool isValueType
        {
            get { return false; }
        }

        /// <summary>
        /// Handles the drawing logic for this array and all it's children.
        /// </summary>
        public sealed override void OnGUILayout()
        {
            EditorGUILayout.BeginHorizontal();
            {
                m_IsExpanded = EditorGUILayout.Foldout(m_IsExpanded, memberName);
                base.OnGUILayout();
            }
            EditorGUILayout.EndHorizontal();

            if (m_IsExpanded)
            {
                EditorGUI.indentLevel++;
                OnDrawContent();
                EditorGUI.indentLevel--;
            }
        }

        /// <summary>
        /// This function is used to draw the content of a collection.
        /// </summary>
        protected abstract void OnDrawContent();
    }
}
