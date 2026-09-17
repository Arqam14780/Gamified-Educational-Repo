using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace AR
{
    public static class AcademyMenuBuild
    {
        private const string ScenePath = "Assets/Scenes/MainMenu.unity";
        private const string SpritePath = "Assets/UI/Academy/RoundedPanel.asset";

        [MenuItem("Tools/Academy/Save menu UI to scene")]
        public static void Build()
        {
            if (!Application.isBatchMode && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var selection = UnityEngine.Object.FindFirstObjectByType<ChildSelection>();
            if (selection == null) throw new InvalidOperationException("MainMenu needs ChildSelection.");

            // Keep learner models, cameras and input when removing the legacy UI.
            var canvases = UnityEngine.Object.FindObjectsByType<Canvas>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var canvas in canvases)
            {
                if (canvas == null) continue;
                foreach (var camera in canvas.GetComponentsInChildren<Camera>(true)) camera.transform.SetParent(null, true);
                foreach (var input in canvas.GetComponentsInChildren<EventSystem>(true)) input.transform.SetParent(null, true);
                if (selection.transform.IsChildOf(canvas.transform)) selection.transform.SetParent(null, true);
                UnityEngine.Object.DestroyImmediate(canvas.gameObject);
            }
            foreach (var old in UnityEngine.Object.FindObjectsByType<AcademyMenu>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                UnityEngine.Object.DestroyImmediate(old.gameObject);
            if (UnityEngine.Object.FindFirstObjectByType<EventSystem>() == null)
                new GameObject("Menu EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));

            for (int i = 0; i < selection.transform.childCount; i++) selection.transform.GetChild(i).gameObject.SetActive(i == 0);
            var menu = new GameObject("Academy Welcome UI").AddComponent<AcademyMenu>();
            menu.BuildForEditor(selection);
            var buttons = menu.GetComponentsInChildren<Button>();
            foreach (var button in buttons) button.onClick = new Button.ButtonClickedEvent();
            UnityEventTools.AddPersistentListener(buttons.First(b => b.name == "<").onClick, menu.PreviousLearner);
            UnityEventTools.AddPersistentListener(buttons.First(b => b.name == ">").onClick, menu.NextLearner);
            UnityEventTools.AddPersistentListener(buttons.First(b => b.name == "Enter academy  >").onClick, menu.EnterAcademy);

            Directory.CreateDirectory("Assets/UI/Academy");
            AssetDatabase.Refresh();
            var sprite = AssetDatabase.LoadAllAssetsAtPath(SpritePath).OfType<Sprite>().FirstOrDefault();
            if (sprite == null)
            {
                sprite = menu.GetComponentInChildren<Image>().sprite;
                AssetDatabase.CreateAsset(sprite.texture, SpritePath);
                sprite.name = "Rounded panel";
                AssetDatabase.AddObjectToAsset(sprite, SpritePath);
            }
            foreach (var image in menu.GetComponentsInChildren<Image>()) image.sprite = sprite;
            AssetDatabase.SaveAssets();
            EditorSceneManager.SaveScene(scene, ScenePath);
            Debug.Log("Academy UI saved to MainMenu. Legacy canvases removed; three persistent button events connected.");
        }

        public static void BuildAndValidate()
        {
            try
            {
                Build();
                // Reopen to prove all UI references survive serialization.
                EditorSceneManager.OpenScene(ScenePath);
                var menu = UnityEngine.Object.FindFirstObjectByType<AcademyMenu>();
                var canvases = UnityEngine.Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None);
                if (menu == null || canvases.Length != 1) throw new InvalidOperationException("Exactly one saved menu canvas is required.");
                foreach (var button in menu.GetComponentsInChildren<Button>())
                    if (button.onClick.GetPersistentEventCount() != 1 || button.onClick.GetPersistentTarget(0) != menu)
                        throw new InvalidOperationException("Missing saved button connection: " + button.name);
                foreach (var image in menu.GetComponentsInChildren<Image>())
                    if (image.sprite == null || !AssetDatabase.Contains(image.sprite))
                        throw new InvalidOperationException("Menu sprites must be saved assets.");
                Debug.Log("SAVED MENU SERIALIZATION PASSED");
                StudyRoomBuild.BuildAndValidate();
            }
            catch (Exception error) { Debug.LogException(error); EditorApplication.Exit(1); }
        }
    }
}
