using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


public class SceneViewWindow : EditorWindow
{

    private Vector2 scrollPos;

    [MenuItem("Tools/Delete Prefs")]

    public static void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Window/Scene View")]
    internal static void Init()
    {
        var window = (SceneViewWindow)GetWindow(typeof(SceneViewWindow), false, "Scene Switch");
        window.position = new Rect(window.position.xMin + 100f, window.position.yMin + 100f, width: 200f, height: 400f);
    }

    internal void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos, false, false);

        GUILayout.Label("Scenes In Build", EditorStyles.boldLabel);
        for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            var scene = EditorBuildSettings.scenes[i];
            if (scene.enabled)
            {
                var sceneName = Path.GetFileNameWithoutExtension(scene.path);
                var pressed = GUILayout.Button(i + ": " + sceneName, new GUIStyle(GUI.skin.GetStyle("Button"))
                { alignment = TextAnchor.MiddleLeft });

                if (pressed)
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scene.path);
                    }
                }
            }
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

}
