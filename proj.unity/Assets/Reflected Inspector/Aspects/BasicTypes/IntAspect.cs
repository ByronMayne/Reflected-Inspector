using TinyJSON;
using UnityEditor;
using UnityEngine;
using Type = System.Type;


namespace ReflectedInspector
{
    public class IntAspect : MemberAspect
    {
        /// <summary>
        /// The true value that this class holds.
        /// </summary>
        [Include]
        private int m_Value;

        /// <summary>
        /// Returns back typeof(int) since this aspect is of that type. 
        /// </summary>
        public override Type aspectType
        {
            get
            {
                return typeof(int);
            }
        }

        /// <summary>
        /// Gets or sets the int value of this property.
        /// </summary>
        public override int intValue
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

        public IntAspect(ReflectedAspect objectAspect, string aspectPath) : base(objectAspect, aspectPath)
        {

        }

        /// <summary>
        /// Called when this object should be loaded from disk.
        /// </summary>
        protected override void LoadValue()
        {
            m_Value = ReflectionHelper.GetFieldValue<int>( fieldSequence:aspectPath, @object: reflectedAspect.targets[0]);
        }

        /// <summary>
        /// A function used to copy all members to a copy of this class. 
        /// </summary>
        protected override void CloneMembers(MemberAspect clone)
        {
            IntAspect intAspect = clone as IntAspect;
            intAspect.m_Value = m_Value;
        }

        public override void OnGUILayout()
        {
            GUILayout.BeginHorizontal();
            {
                m_Value = EditorGUILayout.IntField(memberName, m_Value);
                base.OnGUILayout();
            }
            GUILayout.EndHorizontal();

        }
    }
}
