using System.Reflection;
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

        public StringAspect(ReflectedObject reflectedObject, FieldInfo field) : base(reflectedObject, field)
        {
        }

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
        /// Does this object have value?
        /// </summary>
        public override bool hasValue
        {
            get { return m_Value != null; }
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
                if (m_Value != value)
                {
                    m_Value = value;
                    m_IsDirty = true;
                }
            }
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

        /// <summary>
        /// Is this object a value type?
        /// </summary>
        protected override bool isValueType
        {
            get { return false; }
        }

        public override void OnGUI()
        {
            stringValue = EditorGUILayout.TextField(memberName, m_Value);
        }

        /// <summary>
        /// A function used to copy all members to a copy of this class. 
        /// </summary>
        protected override void CloneMembers(MemberAspect clone)
        {
            StringAspect stringAspect = clone as StringAspect;
            stringAspect.m_Value = m_Value;
        }

        /// <summary>
        /// Loads a value from a target object if a field name
        /// with the same type exists on that object. 
        /// </summary>
        /// <param name="loadFrom">The object you want to load this members value from.</param>
        internal override void LoadValue(object loadFrom)
        {
            if (loadFrom == null)
            {
                throw new System.NullReferenceException("Reflected Inspector: Can't load value from a null object");
            }

            m_Value = (string)fieldInfo.GetValue(loadFrom);
            base.LoadValue(loadFrom);
        }
    }
}
