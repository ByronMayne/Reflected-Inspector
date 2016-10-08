using Type = System.Type;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using TinyJSON;

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
        /// A flag we use to know when we should save. 
        /// </summary>
        private bool m_IsDiry = false;

        /// <summary>
        /// The true value that this class holds.
        /// </summary>
        [Include]
        private List<MemberAspect> m_Members;

        [Include]
        private List<MemberAspect> m_Aspects;

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
            m_Members = new List<MemberAspect>();
            m_Aspects = new List<MemberAspect>();

            m_Targets = targets;

            Type type = m_Targets[0].GetType();

            FieldInfo[] fileds = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            for (int i = 0; i < fileds.Length; i++)
            {
                Type fieldType = fileds[i].FieldType;

                bool successful = false;
                FieldInfo field;
                object value = ReflectionHelper.GetFieldValue(fileds[i].Name, m_Targets[0], out successful, out field);
                MemberAspect member = null;

                if (value != null)
                {
                    member = CreateAspectForType(value.GetType(), fileds[i].Name);
                }
                else
                {
                    member = CreateAspectForType(fieldType, fileds[i].Name);
                }


                m_Members.Add(member);
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
                Type keyType = type.GetGenericArguments()[0];

                if (keyType == typeof(string) || keyType == typeof(int) || keyType == typeof(float))
                {
                    return new DictionaryAspect(this, aspectPath);
                }
                else
                {
                    throw new System.NotSupportedException("Dictionaries with keys that are not int, float, or string are not support");
                }
            }

            return new ObjectAspect(this, aspectPath);
        }

        public void ApplyModifiedChanges()
        {
            for (int i = 0; i < m_Aspects.Count; i++)
            {
                m_Aspects[i].SaveValue();
            }
        }

        public void MarkDirty()
        {
            m_IsDiry = true;
            ApplyModifiedChanges();
        }

        internal void AddAspect(MemberAspect aspect)
        {
            m_Aspects.Add(aspect);
        }

        internal void RemoveAspect(MemberAspect aspect)
        {
            m_Aspects.Remove(aspect);
        }

        /// <summary>
        /// Handles the drawing logic for this array and all it's children.
        /// </summary>
        public void OnGUILayout()
        {
            UnityEditor.EditorGUI.BeginChangeCheck();
            {
                for (int i = 0; i < m_Members.Count; i++)
                {
                    m_Members[i].OnGUI();
                }
            }
            if(UnityEditor.EditorGUI.EndChangeCheck())
            {
                ApplyModifiedChanges();
            }
        }
    }
}
