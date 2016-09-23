using TinyJSON;
using UnityEditor;
using Type = System.Type;


namespace ReflectedInspector
{
    public class StringAspect : MemberAspect
    {
        /// <summary>
        /// The true value that this class holds.
        /// </summary>
        [Include]
        private string m_Value;

        /// <summary>
        /// Returns back typeof(String) since this aspect is of that type. 
        /// </summary>
        public override Type aspectType
        {
            get
            {
                return typeof(string);
            }
        }

        /// <summary>
        /// Gets or sets the string value of this aspect.
        /// </summary>
        public override string stringValue
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
            get { return m_Value != null; }
        }

        /// <summary>
        /// Is this object a value type?
        /// </summary>
        protected override bool isValueType
        {
            get { return false; }
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

        public StringAspect(ReflectedAspect objectAspect, string aspectPath) : base(objectAspect, aspectPath)
        {

        }


        /// <summary>
        /// Called when this object should be loaded from disk.
        /// </summary>
        protected override void LoadValue()
        {
            if (!string.IsNullOrEmpty(aspectPath))
            {
                m_Value = ReflectionHelper.GetFieldValue<string>(aspectPath, reflectedAspect.targets[0]);
            }
        }


        /// <summary>
        /// A function used to copy all members to a copy of this class. 
        /// </summary>
        protected override void CloneMembers(MemberAspect clone)
        {
            StringAspect stringAspect = clone as StringAspect;
            stringAspect.m_Value = m_Value;
        }

        public override void OnGUILayout()
        {
            EditorGUILayout.BeginHorizontal();
            {
                m_Value = EditorGUILayout.TextField(memberName, m_Value);
                base.OnGUILayout();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
