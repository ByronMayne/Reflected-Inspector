using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class ReflectionHelpersGetTests
{
    private const string FIRST_NAME = "Byron";
    private const string LAST_NAME = "Mayne";
    private const float HEIGHT = 6.1f;
    private const double HAIR_LENGTH = 10;
    private const float THICKNESS = 1.0f;

    public class Human
    {
        public string FirstName = FIRST_NAME;
        private string m_LastName = LAST_NAME;
        protected float m_Height = HEIGHT;
        private Head m_Head = new Head();

    }

    public class Head
    {
        private Color m_HairColor = Color.red;
        private double m_HairLength = HAIR_LENGTH;
        public float Thicknes = THICKNESS;
        public Object EyePatch = null;
    }

    #region -= Root Level Tests =-
    [Test( Description = "Loads a public value from our root class") ]
    public void Test_PublicRootLevelValue()
    {
        Human human = new Human();
        bool reachedEndOfPath = false;
        string name = (string)ReflectionHelper.GetFieldValue("FirstName", human, out reachedEndOfPath);
        Assert.AreEqual(name, FIRST_NAME);
    }

    [Test( Description = "Loads a private value from our root class")]
    public void Test_PrivateRootLevelValue()
    {
        Human human = new Human();
        bool reachedEndOfPath = false;
        string name = (string)ReflectionHelper.GetFieldValue("m_LastName", human, out reachedEndOfPath);
        Assert.AreEqual(name, LAST_NAME);
    }

    [Test( Description = "Loads a protected value from our root class")]
    public void Test_ProtectedRootLevelValue()
    {
        Human human = new Human();
        bool reachedEndOfPath = false;
        float height = (float)ReflectionHelper.GetFieldValue("m_Height", human, out reachedEndOfPath);
        Assert.AreEqual(height, HEIGHT);
    }
    #endregion

    #region -= Second Level Tests =-
    [Test( Description = "Loads a public value from the child of our root class")]
    public void Test_PublicSecondLevelValue()
    {
        Human human = new Human();
        bool reachedEndOfPath = false;
        float thickness = (float)ReflectionHelper.GetFieldValue("m_Head.Thicknes", human, out reachedEndOfPath);
        Assert.AreEqual(thickness, THICKNESS, "Found m_Head.Thickness");
    }

    [Test( Description = "Loads a private variable from a child of our root object")]
    public void Test_PrivateSecondLevelValue()
    {
        Human human = new Human();
        bool reachedEndOfPath = false;
        double length = (double)ReflectionHelper.GetFieldValue("m_Head.m_HairLength", human, out reachedEndOfPath);
        Assert.AreEqual(length, HAIR_LENGTH, "Found m_Head.m_HairLength");
    }

    [Test(Description = "Loads a Color object from the child of our root object and casts to see if it has the same value")]
    public void Test_SecondLevelComplexType()
    {
        Human human = new Human();
        bool reachedEndOfPath = false;
        Color color = (Color)ReflectionHelper.GetFieldValue("m_Head.m_HairColor", human, out reachedEndOfPath);
        Assert.AreEqual(color, Color.red, "Found m_Head.m_HairColor");
    }
    #endregion

    #region -= REACHED END OF PATH FLAG TESTS =-
    [Test( Description = "This test is used to check to that when  given a full path with no null except for the end it returns null and reachedEndOfPath is true.")]
    public void Test_EndOfPathNull()
    {
        Human human = new Human();
        bool reachedEndOfPath = false;
        Object @object = (Object)ReflectionHelper.GetFieldValue("m_Head.EyePatch", human, out reachedEndOfPath);
        Assert.AreEqual(null, @object);
        Assert.AreEqual(true, reachedEndOfPath, "End of null path was reached and should be marked as reachedEndOfPath");
    }

    [Test(Description = "This test is used to check to see if when given a full path if one of the object in the middle is null. In this case the function should return null and set reachedEndOfPath to false")]
    public void Test_MiddleOfPathNull()
    {
        Human human = new Human();
        bool reachedEndOfPath = false;
        Object @object = (Object)ReflectionHelper.GetFieldValue("m_Head.EyePatch.m_Name", human, out reachedEndOfPath);
        Assert.AreEqual(null, @object);
        Assert.AreEqual(false, reachedEndOfPath, "There was a null value in the middle of the path (EyePatch) reached end of path should be false.");
    }
    #endregion


    #region -= EXCETIONS TEST =-
    [Test(Description = "Checks to make sure a null root object will toss an exception")]
    public void Test_NullArgumentException()
    {
        bool reachedEndOfPath = false;
        try
        {
            ReflectionHelper.GetFieldValue("m_Head.value", null, out reachedEndOfPath);
        }
        catch
        {
            // We hit an exception so we are good.
            return;
        }
        Assert.Fail("If a null value is sent in as our root object it should get an exception");

    }

    [Test(Description = "Checks to make sure  that if requested to find a field that does not exist it throws an exception")]
    public void Test_FieldDoesNotExistException()
    {
        bool reachedEndOfPath = false;
        try
        {
            ReflectionHelper.GetFieldValue("m_Head.fakefield", null, out reachedEndOfPath);
        }
        catch
        {
            // We hit an exception so we are good.
            return;
        }
        Assert.Fail("If a null value is sent in as our root object it should get an exception");

    }
    #endregion
}
