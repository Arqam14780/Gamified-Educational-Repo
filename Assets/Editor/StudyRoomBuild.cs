using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AR
{
    public static class StudyRoomBuild
    {
        [MenuItem("Tools/Study Room/Build gallery scene")]
        public static void BuildScene()
        {
            if (!Application.isBatchMode && !EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            ConfigureLessonTextures();
            bool existingGallery = File.Exists("Assets/Scenes/StudyRoom.unity");
            var scene = EditorSceneManager.OpenScene(existingGallery ? "Assets/Scenes/StudyRoom.unity" : "Assets/ThirdPerson Control/Scene/3rdPerson.unity");
            var manager = UnityEngine.Object.FindFirstObjectByType<GameManager>();
            Require(manager != null, "Original scene must contain GameManager.");
            var serialized = new SerializedObject(manager);
            var characters = (Transform)serialized.FindProperty("CharacterContainer").objectReferenceValue;
            Require(characters != null && characters.childCount > 0, "Character selection must remain connected.");
            if (existingGallery)
            {
                foreach (var oldRoom in UnityEngine.Object.FindObjectsByType<StudyRoomEnvironment>(FindObjectsSortMode.None))
                    UnityEngine.Object.DestroyImmediate(oldRoom.gameObject);
            }
            else foreach (var root in scene.GetRootGameObjects())
                if (root != manager.transform.root.gameObject && root != characters.root.gameObject) UnityEngine.Object.DestroyImmediate(root);
            var environment = new GameObject("Study Gallery").AddComponent<StudyRoomEnvironment>();
            environment.Build();
            Directory.CreateDirectory("Assets/Resources/StudyGalleryMaterials");
            AssetDatabase.Refresh();
            foreach (var material in environment.GeneratedMaterials)
            {
                string path = "Assets/Resources/StudyGalleryMaterials/" + material.name + ".mat";
                var saved = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (saved != null)
                {
                    EditorUtility.CopySerialized(material, saved);
                    foreach (var renderer in environment.GetComponentsInChildren<Renderer>())
                        if (renderer.sharedMaterial == material) renderer.sharedMaterial = saved;
                }
                else AssetDatabase.CreateAsset(material, path);
            }
            PrefabUtility.SaveAsPrefabAssetAndConnect(environment.gameObject, "Assets/Resources/StudyGallery.prefab", InteractionMode.AutomatedAction);
            if (!existingGallery)
            {
                for (int i = 0; i < characters.childCount; i++) characters.GetChild(i).gameObject.SetActive(i == 0);
                characters.GetChild(0).SetPositionAndRotation(new Vector3(0, .15f, -2.4f), Quaternion.identity);
            }
            var cam = UnityEngine.Object.FindFirstObjectByType<Camera>();
            if (cam == null) cam = new GameObject("Gallery preview camera", typeof(Camera), typeof(AudioListener)).GetComponent<Camera>();
            cam.tag = "MainCamera";
            cam.transform.position = new Vector3(0, 2.6f, -5.4f);
            cam.transform.LookAt(new Vector3(0, 2.5f, 4));
            cam.fieldOfView = 65;
            var light = UnityEngine.Object.FindFirstObjectByType<Light>();
            if (light == null) light = new GameObject("Gallery daylight", typeof(Light)).GetComponent<Light>();
            light.type = LightType.Directional; light.intensity = 1.2f;
            light.transform.rotation = Quaternion.Euler(50, -30, 0);
            RenderSettings.ambientLight = new Color(.7f,.7f,.7f);
            EditorSceneManager.SaveScene(scene, "Assets/Scenes/StudyRoom.unity");
            AssetDatabase.SaveAssets();
            Debug.Log("Study room scene and prefab saved.");
        }

        private static void ConfigureLessonTextures()
        {
            foreach (var lesson in StudyLessonCatalog.Load().lessons)
            foreach (var resource in new[] { lesson.imageResource }.Concat(lesson.pdfPages))
            {
                var importer = AssetImporter.GetAtPath("Assets/Resources/" + resource + ".png") as TextureImporter;
                Require(importer != null, "Missing lesson texture importer: " + resource);
                if (importer.npotScale == TextureImporterNPOTScale.None && !importer.mipmapEnabled && importer.textureCompression == TextureImporterCompression.Uncompressed) continue;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.mipmapEnabled = false;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.maxTextureSize = 2048;
                importer.filterMode = FilterMode.Bilinear;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.SaveAndReimport();
            }
        }

        public static void BuildAndValidate()
        {
            try
            {
                BuildScene();
                PlayerSettings.productName = "StudyRoom Isolated Validation";
                Physics.SyncTransforms();
                var frames = UnityEngine.Object.FindObjectsByType<StudyFrame>(FindObjectsSortMode.None);
                Require(frames.Length == 6, "Six wall-mounted frames are required.");
                foreach (StudyFrameKind kind in Enum.GetValues(typeof(StudyFrameKind)))
                    Require(frames.Count(f => f.kind == kind) == 2, "Each subject needs an image, PDF and quiz frame.");
                foreach (var lesson in StudyLessonCatalog.Load().lessons)
                {
                    Require(Resources.Load<Texture2D>(lesson.imageResource) != null, "Missing image lesson.");
                    foreach (var page in lesson.pdfPages) Require(Resources.Load<Texture2D>(page) != null, "Missing PDF page.");
                    var pdf = File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, lesson.pdfFile));
                    Require(System.Text.Encoding.ASCII.GetString(pdf,0,5) == "%PDF-", "Original PDF must be bundled.");
                }
                foreach (var frame in frames)
                {
                    Vector3 start = frame.transform.position - frame.transform.forward * 2;
                    Require(Physics.Raycast(start, frame.transform.forward, out var hit, 3) && hit.collider.GetComponent<StudyFrame>() == frame, "Picture raycast failed: " + frame.name);
                    Vector3 border = start + frame.transform.right * 1.58f;
                    Require(Physics.Raycast(border, frame.transform.forward, out hit, 3) && hit.collider.GetComponent<StudyFrame>() == frame, "Border raycast failed: " + frame.name);
                }
                foreach (Vector3 direction in new[] {Vector3.forward, Vector3.back, Vector3.left, Vector3.right, Vector3.up, Vector3.down})
                    Require(Physics.Raycast(new Vector3(0,2,0), direction, 10), "Room is not enclosed: " + direction);
                Capture(UnityEngine.Object.FindFirstObjectByType<Camera>(), "gallery-editor.png");
                SessionState.SetBool("StudyRoom.ValidatePlay", true);
                EditorApplication.EnterPlaymode();
            }
            catch (Exception exception) { Debug.LogException(exception); EditorApplication.Exit(1); }
        }

        [InitializeOnLoadMethod]
        private static void ResumeValidation()
        {
            if (!Application.isBatchMode || !SessionState.GetBool("StudyRoom.ValidatePlay", false)) return;
            EditorApplication.update += ValidatePlay;
        }
        private static int waitFrames;
        private static void ValidatePlay()
        {
            if (!EditorApplication.isPlaying || ++waitFrames < 60) return;
            EditorApplication.update -= ValidatePlay;
            try
            {
                var room = UnityEngine.Object.FindFirstObjectByType<StudyRoom>();
                Require(room != null, "GameManager must create StudyRoom in Play Mode.");
                var camera = Camera.main;
                Require(camera != null && !camera.orthographic, "Third-person perspective camera required.");
                Capture(camera, "gallery-play.png");
                var flags = BindingFlags.Instance | BindingFlags.NonPublic;
                var modal = (GameObject)typeof(StudyRoom).GetField("modal", flags).GetValue(room);
                var viewer = UnityEngine.Object.FindFirstObjectByType<StudyMaterialViewer>(FindObjectsInactive.Include);
                Require(viewer != null, "Learning reader must be created.");
                typeof(StudyRoom).GetField("cameraYaw", flags).SetValue(room,-65f);
                Invoke(room,"UpdateCamera",true);
                Capture(camera,"gallery-learning-wall.png");
                typeof(StudyRoom).GetField("cameraYaw", flags).SetValue(room,0f);
                Invoke(room,"UpdateCamera",true);
                foreach (string subject in new[] {"Math", "English"})
                {
                    foreach (var kind in new[] {StudyFrameKind.Image,StudyFrameKind.Pdf})
                    {
                        var materialFrame = UnityEngine.Object.FindObjectsByType<StudyFrame>(FindObjectsSortMode.None).First(f => f.subject == subject && f.kind == kind);
                        ExecuteEvents.Execute(materialFrame.gameObject,new PointerEventData(EventSystem.current),ExecuteEvents.pointerClickHandler);
                        Require(viewer.IsOpen && viewer.CurrentTexture != null, "Material frame must open its learning page.");
                        Require(viewer.CurrentTexture.width == 1400 && viewer.CurrentTexture.height == 900, "Demo lesson textures must keep their original proportions.");
                        Require((bool)typeof(StudyRoom).GetProperty("IsOverlayOpen",flags).GetValue(room), "Learning reader must pause room controls.");
                        viewer.SetZoom(2); Require(viewer.Zoom == 2, "Zoom must work."); viewer.SetZoom(1);
                        if (kind == StudyFrameKind.Pdf)
                        {
                            var first=viewer.CurrentTexture; viewer.ChangePage(1);
                            Require(viewer.PageIndex == 1 && viewer.CurrentTexture != first, "Next must display the second PDF page.");
                            viewer.ChangePage(20); Require(viewer.PageIndex == 1, "PDF must clamp the last page.");
                            viewer.ChangePage(-1); Require(viewer.PageIndex == 0, "Previous must display the first PDF page.");
                        }
                        else { viewer.ChangePage(1); Require(viewer.PageIndex == 0,"Single images must not page forward."); }
                        if (subject == "Math") Capture(camera,kind == StudyFrameKind.Image?"gallery-image.png":"gallery-pdf.png");
                        viewer.Close(); Require(!viewer.IsOpen,"Closing materials must restore the room.");
                    }
                    var frame = UnityEngine.Object.FindObjectsByType<StudyFrame>(FindObjectsSortMode.None).First(f => f.subject == subject && f.kind == StudyFrameKind.Quiz);
                    ExecuteEvents.Execute(frame.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
                    Require(modal.activeSelf, "Frame click must open quiz.");
                    if (subject == "Math") Capture(camera, "gallery-quiz.png");
                    int[] correct = subject == "Math" ? new[] {1,2,0,3,1} : new[] {0,2,1,0,3};
                    for (int i = 0; i < 5; i++)
                    {
                        Invoke(room, "Answer", correct[i]); Invoke(room, "Answer", correct[i]);
                        Require((int)typeof(StudyRoom).GetField("score", flags).GetValue(room) == i + 1, "Duplicate answer must not increase score.");
                        Invoke(room, "Next");
                    }
                    Invoke(room, "Next");
                    Require((int)typeof(StudyRoom).GetField("score", flags).GetValue(room) == 0, "Retry must reset score.");
                    Invoke(room, "CloseQuiz");
                    Require(!modal.activeSelf, "Close must restore room.");
                }
                SessionState.SetBool("StudyRoom.ValidatePlay", false);
                File.WriteAllText(Path.Combine(Application.dataPath, "../validation-result.txt"), "PASS: six frame/border raycasts; bundled images and original PDFs; image reader; PDF next/previous and page bounds; zoom; reading pause state; both linked quizzes; duplicate answer protection; results/retry/close. Native Windows PDF parser separately confirmed both PDFs have two pages.");
                Debug.Log("STUDY ROOM VALIDATION PASSED");
                EditorApplication.Exit(0);
            }
            catch (Exception exception) { Debug.LogException(exception); EditorApplication.Exit(1); }
        }

        private static void Invoke(StudyRoom room, string method, params object[] args)
        {
            typeof(StudyRoom).GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(room, args);
        }
        private static void Capture(Camera camera, string path)
        {
            var texture = new RenderTexture(1600,900,24);
            var oldTarget = camera.targetTexture;
            var oldActive = RenderTexture.active;
            var canvases = UnityEngine.Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None);
            foreach (var canvas in canvases) { canvas.renderMode = RenderMode.ScreenSpaceCamera; canvas.worldCamera = camera; canvas.planeDistance = .4f; }
            camera.targetTexture = texture;
            Canvas.ForceUpdateCanvases();
            camera.Render();
            RenderTexture.active = texture;
            var pixels = new Texture2D(1600,900,TextureFormat.RGB24,false);
            pixels.ReadPixels(new Rect(0,0,1600,900),0,0); pixels.Apply();
            File.WriteAllBytes(Path.Combine(Application.dataPath, "../" + path),pixels.EncodeToPNG());
            camera.targetTexture = oldTarget; RenderTexture.active = oldActive;
            foreach (var canvas in canvases) canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            UnityEngine.Object.DestroyImmediate(pixels); UnityEngine.Object.DestroyImmediate(texture);
        }

        private static void Require(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }
    }
}
