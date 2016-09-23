using TinyJSON;
using UnityEditor;
using UnityEngine;
using Type = System.Type;


namespace ReflectedInspector
{
    public class AnimationCurveAspect : MemberAspect
    {
        /// <summary>
        /// The true value that this class holds.
        /// </summary>
        [Include]
        private AnimationCurve m_Value;

        /// <summary>
        /// Returns back typeof(float) since this aspect is of that type. 
        /// </summary>
        public override Type aspectType
        {
            get
            {
                return typeof(AnimationCurve);
            }
        }

        /// <summary>
        /// Gets the color value for this class.
        /// </summary>
        public override AnimationCurve animationCurveValue
        {
            get
            {
                return m_Value;
            }

            set
            {
                m_Value = value;
            }
        }

        /// <summary>
        /// Does this object have value?
        /// </summary>
        public override bool hasValue
        {
            get { return true; }
        }

        /// <summary>
        /// Is this object a value type?
        /// </summary>
        protected override bool isValueType
        {
            get { return true; }
        }

        /// <summary>
        /// Returns the raw value of the object.
        /// </summary>
        public override object value
        {
            get
            {
                return m_Value;
            }
        }

        public AnimationCurveAspect(ReflectedAspect objectAspect, string aspectPath) : base(objectAspect, aspectPath)
        {
        }


        /// <summary>
        /// Called when this object should be loaded from disk.
        /// </summary>
        protected override void LoadValue()
        {
            m_Value = ReflectionHelper.GetFieldValue<AnimationCurve>(aspectPath, reflectedAspect.targets[0]);
        }

        /// <summary>
        /// A function used to copy all members to a copy of this class. 
        /// </summary>
        protected override void CloneMembers(MemberAspect clone)
        {
            AnimationCurveAspect cruveAspect = clone as AnimationCurveAspect;
            cruveAspect.m_Value.keys = m_Value.keys;
            cruveAspect.m_Value.postWrapMode = m_Value.postWrapMode;
            cruveAspect.m_Value.preWrapMode = m_Value.preWrapMode;
        }

        public override void OnGUILayout()
        {
            EditorGUILayout.BeginHorizontal();
            {
                m_Value = EditorGUILayout.CurveField(memberName, m_Value);
                base.OnGUILayout();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
