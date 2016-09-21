using Type = System.Type;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System.Collections;

namespace ReflectedInspector
{
    public class ReflectedAspect
    {

        /// <summary>
        /// Is this item expanded in the editor?
        /// </summary>
        private bool m_IsExpanded = false;

        private object[] m_Targets;

        /// <summary>
        /// The true value that this class holds.
        /// </summary>
        private List<MemberAspect> m_Children;

        public object[] targets
        {
            get { return m_Targets; }
            set { m_Targets = value; }
        }

        /// <summary>
        /// Creates a new reflected object for an inspected object.
        /// </summary>
        public ReflectedAspect(object target) : this(new object[1] { target })
        {

        }

        public ReflectedAspect(object[] targets)
        {
            m_Children = new List<MemberAspect>();

            m_Targets = targets;

            Type type = m_Targets[0].GetType();

            FieldInfo[] fileds = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            for (int i = 0; i < fileds.Length; i++)
            {
                Type fieldType = fileds[i].FieldType;

                MemberAspect member = CreateAspectForType(fieldType, fileds[i].Name);

                m_Children.Add(member);
            }
        }

        public MemberAspect CreateAspectForType(Type type, string aspectPath)
        {
            if (type == typeof(int))
            {
                return new IntAspect(this, aspectPath);
            }

            if (type == typeof(float))
            {
                return new FloatAspect(this, aspectPath);
            }

            if (type == typeof(string))
            {
                return new StringAspect(this, aspectPath);
            }

            if (type == typeof(bool))
            {
                return new BoolAspect(this, aspectPath);
            }

            if (type == typeof(UnityEngine.Color))
            {
                return new ColorAspect(this, aspectPath);
            }

            if (type == typeof(UnityEngine.Vector2))
            {
                return new Vector2Aspect(this, aspectPath);
            }

            if (type == typeof(UnityEngine.Vector3))
            {
                return new Vector3Aspect(this, aspectPath);
            }

            if (type == typeof(UnityEngine.Vector4))
            {
                return new Vector3Aspect(this, aspectPath);
            }

            if (type == typeof(UnityEngine.Bounds))
            {
                return new BoundsAspect(this, aspectPath);
            }

            if (type == typeof(UnityEngine.Rect))
            {
                return new RectAspect(this, aspectPath);
            }

            if (type == typeof(UnityEngine.AnimationCurve))
            {
                return new AnimationCurveAspect(this, aspectPath);
            }

            if (typeof(IList).IsAssignableFrom(type))
            {
                return new ArrayAspect(this, aspectPath);
            }

            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                return new ArrayAspect(this, aspectPath);
            }

            return new ObjectAspect(this, aspectPath);
        }

        public void ApplyModifiedChanges()
        {
            for (int i = 0; i < m_Children.Count; i++)
            {
                m_Children[i].SaveValue();
            }
        }


        /// <summary>
        /// Handles the drawing logic for this array and all it's children.
        /// </summary>
        public void OnGUILayout()
        {
            for (int i = 0; i < m_Children.Count; i++)
            {
                m_Children[i].OnGUILayout();
            }
        }
    }
}
