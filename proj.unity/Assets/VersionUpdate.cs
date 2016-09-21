using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System.Linq;
using Version = System.Version;
using UnityEditor.Callbacks;

public class VersionUpdate
{
    private const string PLAYER_SETTINGS_ASSEMBLY_NAME = "UnityEditor.PlayerSettings";

    [MenuItem("Version/Bump")]
    [PostProcessBuild]
    public static void BumpVersionNumber()
    {
        // Did we reuse or load ourself?
        bool didReuse = false;

        // Create a holder
        Object projectSettings = null;

        // Make our path.
        string settingsPath = Application.dataPath.Replace("/Assets", "/ProjectSettings/ProjectSettings.asset");

        // Find if it's already loaded
        projectSettings = Resources.FindObjectsOfTypeAll<Object>().Where(a => string.Compare(a.GetType().FullName, PLAYER_SETTINGS_ASSEMBLY_NAME) == 0).FirstOrDefault();

        if (projectSettings == null)
        {

            Object[] loadedObjects = InternalEditorUtility.LoadSerializedFileAndForget(settingsPath);

            // For funzies print out each object defined in the ProjectSettings.asset yaml.
            for (int i = 0; i < loadedObjects.Length; i++)
            {
                if (string.Compare(loadedObjects[i].GetType().FullName, PLAYER_SETTINGS_ASSEMBLY_NAME) == 0)
                {
                    projectSettings = loadedObjects[i];
                    Debug.Log("We loaded a new instance of: " + loadedObjects[i].GetType().FullName);
                    didReuse = false;
                    break;
                }

            }
        }
        else
        {
            Debug.Log("We reused a current instance of " + projectSettings.GetType().FullName);
            didReuse = true;
        }

        // Create a way to edit it
        SerializedObject serializedProjectSettings = new SerializedObject(projectSettings);
        // Get our bundle version field. 
        SerializedProperty bundleVersion = serializedProjectSettings.FindProperty("bundleVersion");
        // Cast it as a version
        Version version = new Version(bundleVersion.stringValue);
        // Log our current.
        Debug.Log("Old Version: " + version.ToString());
        // Bump the build version
        version = new Version(version.Major, version.Minor, version.Build + 1);
        // Save it back.
        bundleVersion.stringValue = version.ToString();
        // Log new version
        Debug.Log("New Version: " + version.ToString());
        // Save serialized object
        serializedProjectSettings.ApplyModifiedProperties();

        if (!didReuse)
        {
            // Save back to disk. (Only if we are the only watcher)
            InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] { projectSettings }, settingsPath, true);
        }
    }

}
