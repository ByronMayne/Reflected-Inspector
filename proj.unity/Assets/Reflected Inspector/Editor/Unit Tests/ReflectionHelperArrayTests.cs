using NUnit.Framework;
using ReflectedInspector;
using System.Collections.Generic;

public class ReflectionHelperArrayTests
{
    public class Person
    {
        public string Name;
    }

    public class Classroom
    {
        public List<Person> m_Students = new List<Person>()
        {
            new Person() { Name  = "Frank" },
            new Person() { Name  = "Joan" },
            new Person() { Name  = "Mike" }
        };

        public Person[] Teachers = new Person[]
        {
            new Person() { Name = "Mrs Henry" }
        };
    }

    [Category("Array Access")]
    [Test(Description = "Tests to check if we can get an index from a path")]
    public void Test_GetIndexFromPath()
    {
        string path = SequenceHelper.AppendListEntryToSequence("gameObject.transform.children.", 2) + ".Name";
        int index = SequenceHelper.GetArrayIndex(path);
        Assert.AreEqual(2, index);
    }

    [Category("Array Access")]
    [Test(Description = "Tests to check if we can get an index from a path")]
    public void Test_GettingListEntry()
    {
        Classroom classroom = new Classroom();

        string path = SequenceHelper.AppendListEntryToSequence("m_Students", 0) + ".Name";
        string name = ReflectionHelper.GetFieldValue<string>(path, classroom);
        Assert.AreEqual("Frank", name);
    }

    [Category("Array Access")]
    [Test(Description = "Tests to check if we can get an index from a path")]
    public void Test_GettingArrayElement()
    {
        Classroom classroom = new Classroom();

        string path = SequenceHelper.AppendListEntryToSequence("Teachers", 0) + ".Name";
        string name = ReflectionHelper.GetFieldValue<string>(path, classroom);
        Assert.AreEqual("Mrs Henry", name);
    }
}
