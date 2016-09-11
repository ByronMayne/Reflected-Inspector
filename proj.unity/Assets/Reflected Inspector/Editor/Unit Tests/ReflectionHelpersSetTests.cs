using UnityEngine;
using System.Collections;
using NUnit.Framework;

public class ReflectionHelpersSetTests
{
    private const string FIRST_NAME = "Byron";
    private const string LAST_NAME = "Mayne";
    private const float HEIGHT = 6.1f;
    private const double HAIR_LENGTH = 10;
    private const float THICKNESS = 1.0f;

    public class Human
    {
        public string FirstName;
        private string m_LastName;
        protected float m_Height;
        private Head m_Head;

        public string GetFirstName()
        {
            return FirstName;
        }

        public string GetLastName()
        {
            return m_LastName;
        }

        public float GetHeight()
        {
            return m_Height;
        }

        public Head GetHead()
        {
            return m_Head;
        }
    }

    public class TColor
    {
        public float a;
        public float r;
        public float g;
        public float b;
    }


    public class Head
    {
        private TColor m_HairColor;
        private double m_HairLength;
        public float Thicknes;
        public Object EyePatch;

        public TColor GetColor()
        {
            return m_HairColor;
        }

        public double GetHairLength()
        {
            return m_HairLength;
        }

        public float GetThickness()
        {
            return Thicknes;
        }

        public Object GetEyePatch()
        {
            return EyePatch;
        }
    }

    #region -= Root Level Tests =-
    [Test(Description = "Sets a public value from our root class")]
    public void Test_PublicRootLevelValue()
    {
        Human human = new Human();
        ReflectionHelper.SetFieldValue("FirstName", human, FIRST_NAME);
        Assert.AreEqual(human.GetFirstName(), FIRST_NAME, "First name was unable to be set correctly");
    }

    [Test(Description = "Sets a private value from our root class")]
    public void Test_PrivateRootLevelValue()
    {
        Human human = new Human();
        ReflectionHelper.SetFieldValue("m_LastName", human, LAST_NAME);
        Assert.AreEqual(human.GetLastName(), LAST_NAME, "Last name was unable to be set correctly");
    }

    [Test(Description = "Sets a protected value from our root class")]
    public void Test_ProtectedRootLevelValue()
    {
        Human human = new Human();
        ReflectionHelper.SetFieldValue("m_Height", human, HEIGHT);
        Assert.AreEqual(human.GetHeight(), HEIGHT, "Last name was unable to be set correctly");
    }
    #endregion

    #region -= CHILD TEST =-
    [Test(Description = "Sets a class to a new instance on the root object")]
    public void Test_CreateChildClass()
    {
        Human human = new Human();
        ReflectionHelper.SetFieldValue("m_Head", human, new Head());
        Assert.NotNull(human.GetHead(), "Last name was unable to be set correctly");
    }

    [Test(Description = "Sets a value on a child class that is null, this tests to make sure that class is created.")]
    public void Test_SetComplexObject()
    {
        Human human = new Human();
        Head head = new Head();
        TColor color = new TColor();
        ReflectionHelper.SetFieldValue("m_Head", human, head);
        ReflectionHelper.SetFieldValue("m_Head.m_HairColor", human, color);
        Assert.AreEqual(color, human.GetHead().GetColor(), "Color was unable to be set correctly");
    }
    #endregion

}
