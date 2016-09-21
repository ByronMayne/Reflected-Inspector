 [![License](https://img.shields.io/badge/license-MIT-lightgrey.svg)](https://github.com/ByronMayne/Reflected-Inspector/blob/master/LICENSE)
 
 ### Heads Up
 This repo is under quite a big rework right now to make things that much better. 
 
# Reflected Inspector

Unity's [Serialized Object](https://docs.unity3d.com/ScriptReference/SerializedObject.html) and [Serialized Property](https://docs.unity3d.com/ScriptReference/SerializedProperty.html) system are really powerful things but have their limitions. Reflected Inspector is designed to overcome some of the problems that I have come across. At it's current state it lets you view and edit non-serialized types that Unity can't normally expose to use. This allows for quicker debugging.
 * Supports displaying and modifying [polymorphic types](https://unity3d.com/learn/tutorials/topics/scripting/polymorphism).  
 * Supports null values (Unity serializion can't have nulls). 
 * Has field drawers to allow for custom layouts. 
 * Method Drawers to all for invoking of methods with/without arguments.

## How to Use
To use Reflected Inspector does not require a lot of work. The whole point of the system is to keep things simple to the end user. There is a few ways that you can use the system to our advantage. 
#### The Basic Usage
To get your class to be drawn by Reflected Inspector is as easy has inheriting from ```ReflectedBehaviour```. That class has it's own custom editor which gets applied to all child classes. If you don't want to inherit from that class you can make your own custom editor. 
```csharp
[CustomEditor(typeof(MyType))]
public class MyTypeEditor : Editor
{
    // Our object that we use to display our class in the inspector.
    private ReflectedObject m_ReflectedObject; 
    
    // Called when the editor is first viewed. 
    public void OnEnable()
    {
        // We create a new reflected object and send in our list of targets
        // targets is a list of all GameObjects that we have selected. This 
        // allows us to support multi editing. 
        m_ReflectedObject = new ReflectedObject(targets);
    }
    
    // Called every event that happens in the Inspector window. 
    public override void OnInspectorGUI()
    {
        // Tells the reflected object to loop over all children recursively
        // and draw their default editor. 
        m_ReflectedObject.DoLayout();
    }
}
```
The example above is the most simple use case. This will now instead of drawing with Unity's serialized object system will used Reflected Object. 

### Intermediate 
The example above is great if you just want to draw everything but most of the time you don't want all your fields exposed. This can be a achieved in one of two ways. The first one being the ```[HideField]``` attribute. 
``` csharp
using ReflectedInspector;

public class MyClass
{
    public string isExposed; // Will show in the inspector
    [HideField]
    public string isNotExposed; // Will be hidden from the inspector.
}
```
Keep in mind if you hide a field all subclasses will also be hidden. If you just want to view the value but not edit it you can use the ```[ReadOnly]``` attribute. 
``` csharp
using ReflectedInspector;

public class MyClass
{
    public string isExposed; // Will show in the inspector
    [ReadOnly]
    public string isExposedButCantEdit; // Will be shown in the inspector but can't be edited.
}
```

If you would like more control then this we can go and handle the drawing of our inspector ourself. Let say we have the class below.

```csharp
public class Person
{
    public string firstName;
    public string lastName;
    public int age; 
}

public class ClassRoom
{
    public List<Person> students = new List<Person>();
    public teacher = new Person(); 
}
```
We want to handle this inspector in a unique way. Lets say we wanted a header that said "Students" and there we would draw the students with all their information. After that we would draw a second header 
that would say "Teacher" but only draw their last name. This problem could not be solved with the ```[HideField]``` attribute since we want to draw the fields sometimes. Below is a little diagram of what we are looking for.
```
Students
    Bob Jones 22
    Frank Miles 32
    Billy Jole 24
Teachers
    Professor Stuartson
```
This goal can be achieved with the help of the ```ReflectedObject.FindField(string name)``` function.
```csharp
using ReflectedInspector;


public class ClassRoomEditor
{
    private ReflectedObject m_ClassRoom; 
    
    public void OnEnable()
    {
        m_ClassRoom = new ReflectedObject(targets);
    }
    
    public override void OnInspectorGUI()
    {
        // Grab our teacher
        ReflectedField teacher = m_ClassRoom.FindField("teacher");
        // Grab our students. 
        ReflectedField students = m_ClassRoom.FindField("students");
        
        GUILayout.Label("Students");
        GUILayout.BeginHorizontal();
        {
            // Will draw their first name last name then their age. This
            // function draws their children too. 
            students.DoLayout();
        }
        GUILayout.EndHorizontal();
        
        GUILayout.Label("Teacher");
        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Professor");
            // We only want the last name.
            // We have to find it's relative or subobject in this case the field
            // called lastName inside the person class. 
            ReflectedField teachersLastName = teacher.FindFieldRelative("lastName");
            teachersLastName.DoLayout();
        }
        GUILayout.EndHorizontal();
    }
}
```

## Notes
1) Because of how the system works if you change the inspector mode to Debug you will not be able to see any non-serialized Unity types. 

## Meta
Created by Byron Mayne [[twitter](https://twitter.com/byMayne) &bull; [github](https://github.com/ByronMayne)]

Released under the [MIT License](http://www.opensource.org/licenses/mit-license.php).

