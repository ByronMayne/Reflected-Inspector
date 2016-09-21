using UnityEditor;
using UnityEngine;
using Type = System.Type;


namespace ReflectedInspector
{
    public abstract class MemberAspect
    {
        /// <summary>
        /// The string name of this member.
        /// </summary>
        private string m_MemberName;

        /// <summary>
        /// The root object that owns this property.
        /// </summary>
        private ReflectedAspect m_ReflectedAspect;

        /// <summary>
        /// The path to the aspect from our root object.
        /// </summary>
        private string m_AspectPath;

        #region -= Accessors =-
        /// <summary>
        /// If a dictionary returns the value at that key otherwise throws an exception. 
        /// </summary>
        public virtual MemberAspect this[MemberAspect key]
        {
            get
            {
                throw new System.NotImplementedException("String indexer has not been implemented for " + GetType().Name);
            }
            protected set
            {
                throw new System.NotImplementedException("String indexer has not been implemented for " + GetType().Name);
            }
        }

        /// <summary>
        /// If this class is an array returns the element at that index. If a dictionary returns the value at that key, otherwise
        /// throws an exception. 
        /// </summary>
        public virtual MemberAspect this[int index]
        {
            get
            {
                throw new System.NotImplementedException("Int indexer has not been implemented for " + GetType().Name);
            }
            protected set
            {
                throw new System.NotImplementedException("Int indexer has not been implemented for " + GetType().Name);
            }
        }

        /// <summary>
        /// Value of this integer property
        /// </summary>
        public virtual int intValue
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type int it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type int it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Value of this long property
        /// </summary>
        public virtual long longValue
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type long it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type long it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Value of this bool property.
        /// </summary>
        public virtual bool boolValue
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type Integer it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type bool it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Value of this float property.
        /// </summary>
        public virtual float floatValue
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type float it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type float it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Value of this double property.
        /// </summary>
        public virtual double doubleValue
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type double it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type double it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Value of this string property.
        /// </summary>
        public virtual string stringValue
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type string it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type string it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Value of this color property.
        /// </summary>
        public virtual Color colorValue
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type color it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type color it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Value of this animation curve property.
        /// </summary>
        public virtual AnimationCurve animationCurveValue
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type animation curve it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type animation curve it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Value of this gradient property.
        /// </summary>
        public virtual Gradient gradientValue
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type gradient  it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type gradient it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Value of the object reference value of this property.
        /// </summary>
        public virtual Object objectReferenceValue
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type Object it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type Object it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Gets the value of this enum property.
        /// </summary>
        public virtual int enumValueIndex
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type enum it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type enum it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Value of the 2D vector property.
        /// </summary>
        public virtual Vector2 vector2Value
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type Vector2 it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type Vector2 it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Value of the #D vector property.
        /// </summary>
        public virtual Vector3 vector3Value
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type Vector3 it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type Vector3 it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Value of the 4D vector property.
        /// </summary>
        public virtual Vector4 vector4Value
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type Vector4 it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type Vector4 it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Value of the rect property.
        /// </summary>
        public virtual Rect rectValue
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type Rect it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type Rect it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Value of the bounds property.
        /// </summary>
        public virtual Bounds boundsValue
        {
            get
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type Bounds it's a " + aspectType.ToString());
            }
            set
            {
                throw new System.InvalidCastException(m_MemberName + " is not of type Bounds it's a " + aspectType.ToString());
            }
        }

        /// <summary>
        /// Finds a relative of this class. This only works if the aspect type is <see cref="ObjectAspect"/>
        /// </summary>
        public virtual MemberAspect FindFieldRelative(string relativeName)
        {
            throw new System.NotImplementedException("This class does not contain sub elements");
        }
        #endregion

        /// <summary>
        /// Returns true if this value is an array.
        /// </summary>
        public virtual bool IsArray
        {
            get { return false; }
        }

        /// <summary>
        /// Returns the size of this array if it's not an array will return 0.
        /// </summary>
        public virtual int elementCount
        {
            get { return 0; }
            set { }
        }

        /// <summary>
        /// Returns the name of the member that this aspect is controlling. This
        /// is the name that will be displayed in the inspector.
        /// </summary>
        public string memberName
        {
            get { return m_MemberName; }
            protected set { m_MemberName = value; }
        }

        /// <summary>
        /// Is this item expanded in the inspector? Used by class, array, and dictionary aspect.
        /// </summary>
        public virtual bool isExpanded
        {
            get { return false; }
        }

        /// <summary>
        /// Gets our owning object aspect
        /// </summary>
        public ReflectedAspect reflectedAspect
        {
            get { return m_ReflectedAspect; }
        }

        /// <summary>
        /// The path to our aspect from the root object.
        /// </summary>
        public string aspectPath
        {
            get { return m_AspectPath; }
        }

        /// <summary>
        /// Gets the value of the object.
        /// </summary>
        public abstract object value
        {
            get;
        }

        /// <summary>
        /// Returns that System.Type of this aspect.
        /// </summary>
        public abstract Type aspectType
        {
            get;
        }

        /// <summary>
        /// Does this object have value?
        /// </summary>
        protected abstract bool hasValue { get; }

        /// <summary>
        /// Is this object a value type?
        /// </summary>
        protected abstract bool isValueType { get; }

        /// <summary>
        /// Creates a new instance of a Member Aspect
        /// </summary>
        /// <param name="reflectedAspect"></param>
        /// <param name="aspectPath"></param>
        public MemberAspect(ReflectedAspect reflectedAspect, string aspectPath)
        {
            m_ReflectedAspect = reflectedAspect;
            SetAspectPath(aspectPath);
            LoadValue();
        }

        /// <summary>
        /// Inserts an element at the index if this class is an array.
        /// </summary>
        public virtual void DeleteArrayElmentAtIndex(int index)
        {
        }

        /// <summary>
        /// Deletes an element at the index if this class is an array.
        /// </summary>
        public virtual void InsetArrayElementAtIndex(int index)
        {

        }

        /// <summary>
        /// Changes the member name and aspect path of this element.
        /// </summary>
        internal virtual void SetAspectPath(string aspectPath)
        {
            m_AspectPath = aspectPath;
            m_MemberName = SequenceHelper.GetDisplayNameFromPath(aspectPath);
        }

        /// <summary>
        /// Called when this object should be loaded from disk.
        /// </summary>
        protected abstract void LoadValue();

        /// <summary>
        /// Called when this object should be saved to disk.
        /// </summary>
        public virtual void SaveValue()
        {
            ReflectionHelper.SetFieldValue(aspectPath, reflectedAspect.targets[0], value);

            for (int i = 0; i < elementCount; i++)
            {
                this[i].SaveValue();
            }
        }

        /// <summary>
        /// Copies on element into another.
        /// </summary>
        /// <returns></returns>
        public MemberAspect Copy()
        {
            return Internal_CreateClone();
        }

        /// <summary>
        /// Copies the root settings from this object to the next. 
        /// </summary>
        private MemberAspect Internal_CreateClone()
        {
            MemberAspect clone = reflectedAspect.CreateAspectForType(aspectType, aspectPath);
            clone.m_MemberName = m_MemberName;
            return clone;
        }

        /// <summary>
        /// A function used to copy all members to a copy of this class. 
        /// </summary>
        protected abstract void CloneMembers(MemberAspect clone);

        /// <summary>
        /// Invoked when this element should draw.
        /// </summary>
        public virtual void OnGUILayout()
        {
            // We can't do nulls with value types.
            if (!isValueType)
            {
                EditorGUI.BeginDisabledGroup(hasValue);
                if (GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.ExpandWidth(false)))
                {
                    OnNewValueLoaded();
                }
                EditorGUI.EndDisabledGroup();


                if (GUILayout.Button("&", EditorStyles.miniButtonMid, GUILayout.ExpandWidth(false)))
                {
                    OnPoloymorphicTypeAssigned();
                }

                EditorGUI.BeginDisabledGroup(!hasValue);
                if (GUILayout.Button("x", EditorStyles.miniButtonRight, GUILayout.ExpandWidth(false)))
                {
                    OnAspectSetToNull();
                }
                EditorGUI.EndDisabledGroup();
            }
        }

        protected virtual void OnNewValueLoaded()
        {

        }

        protected virtual void OnPoloymorphicTypeAssigned()
        {
        }

        protected virtual void OnAspectSetToNull()
        {
        }
    }
}
