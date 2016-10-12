using Type = System.Type;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using TinyJSON;
using UnityEditor;
using UnityEngine;

namespace ReflectedInspector
{
    public class ReflectedObject
    {
        /// <summary>
        /// Is this item expanded in the editor?
        /// </summary>
        private bool m_IsExpanded = false;

        /// <summary>
        /// The array of objects we are targeting.
        /// </summary>
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
        public ReflectedObject(object target) : this(new object[1] { target })
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targets"></param>
        public ReflectedObject(object[] targets)
        {
            m_Members = new List<MemberAspect>();
            m_Aspects = new List<MemberAspect>();

            m_Targets = targets;

            Type type = m_Targets[0].GetType();

            FieldInfo[] fileds = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            for (int i = 0; i < fileds.Length; i++)
            {
                MemberAspect member = CreateAspect(fileds[i]);
                m_Members.Add(member);
            }

            LoadMemberValues();
        }

        /// <summary>
        /// Loads all the values for this classes members and also
        /// checks to see if they have mixed values.
        /// </summary>
        internal void LoadMemberValues()
        {
            for (int x = 0; x < m_Members.Count; x++)
            {
                for (int y = 0; y < m_Targets.Length; y++)
                {
                    if (y == 0)
                    {
                        m_Members[x].LoadValue(m_Targets[y]);
                    }
                    else if (m_Members[x].CheckForMixedValues(m_Targets[y]))
                    {
                        // We found one mixed value so we don't have to keep checking the other targets.
                        break;
                    }
                }
            }
        }

        public MemberAspect CreateAspect(FieldInfo field, object target = null)
        {
            Type type = null;

            if (target != null)
            {
                type = target.GetType();
            }
            else
            {
                type = field.FieldType;
            }

            if (type == typeof(int))
            {
                return new IntAspect(this, field);
            }

            if (type == typeof(float))
            {
                return new FloatAspect(this, field);
            }

            if (type == typeof(string))
            {
                return new StringAspect(this, field);
            }

            if (type == typeof(bool))
            {
                return new BoolAspect(this, field);
            }

            if (type == typeof(UnityEngine.Color))
            {
                return new ColorAspect(this, field);
            }

            if (type == typeof(UnityEngine.Vector2))
            {
                return new Vector2Aspect(this, field);
            }

            if (type == typeof(UnityEngine.Vector3))
            {
                return new Vector3Aspect(this, field);
            }

            if (type == typeof(UnityEngine.Vector4))
            {
                return new Vector3Aspect(this, field);
            }

            if (type == typeof(UnityEngine.Bounds))
            {
                return new BoundsAspect(this, field);
            }

            if (type == typeof(UnityEngine.Rect))
            {
                return new RectAspect(this, field);
            }

            if (type == typeof(UnityEngine.AnimationCurve))
            {
                return new AnimationCurveAspect(this, field);
            }

            //if (typeof(IList).IsAssignableFrom(type))
            //{
            //    return new ArrayAspect(this, field);
            //}

            //if (typeof(IDictionary).IsAssignableFrom(type))
            //{
            //    Type keyType = type.GetGenericArguments()[0];

            //    if (keyType == typeof(string) || keyType == typeof(int) || keyType == typeof(float))
            //    {
            //        return new DictionaryAspect(this, field);
            //    }
            //    else
            //    {
            //        throw new System.NotSupportedException("Dictionaries with keys that are not int, float, or string are not support");
            //    }
            //}

            return new ObjectAspect(this, field);
        }

        public void ApplyModifiedChanges()
        {
            for (int i = 0; i < m_Members.Count; i++)
            {
                for (int x = 0; x < m_Targets.Length; x++)
                {
                    m_Members[i].SaveValue(m_Targets[x]);
                }
                m_Members[i].ClearDirtyFlag();
            }


            // Set all our targets as Dirty (so Unity wills save Unity types).
            for (int x = 0; x < m_Targets.Length; x++)
            {
                if(m_Targets[x] is Object)
                {
                    EditorUtility.SetDirty(m_Targets[x] as Object);
                }
            }
        }

        public void MarkDirty()
        {
            m_IsDiry = true;
            ApplyModifiedChanges();
        }

        /// <summary>
        /// Removes an aspect to the list of watched aspects. 
        /// </summary>
        internal void AddAspect(MemberAspect aspect)
        {
            m_Aspects.Add(aspect);
        }

        /// <summary>
        /// Adds an aspect to the list of watched aspects. 
        /// </summary>
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
                    //EditorGUI.showMixedValue = m_Members[i].hasMixedValues;
                    m_Members[i].OnGUI();
                }
            }
            if (UnityEditor.EditorGUI.EndChangeCheck())
            {
                ApplyModifiedChanges();
            }
        }
    }
}
