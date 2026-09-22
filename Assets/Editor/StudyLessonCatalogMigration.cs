#if UNITY_EDITOR
using UnityEditor; using UnityEngine; using AR;
[InitializeOnLoad] public static class StudyLessonCatalogMigration {
 static StudyLessonCatalogMigration(){EditorApplication.delayCall+=CreateOrRefresh;}
 [MenuItem("Learning Academy/Create or Refresh Study Lesson Catalog")]
 public static void CreateOrRefresh(){const string j="Assets/Resources/Lessons/catalog.json", a="Assets/Resources/Lessons/StudyLessonCatalog.asset"; var t=AssetDatabase.LoadAssetAtPath<TextAsset>(j); if(t==null){Debug.LogError("Missing "+j);return;} var source=JsonUtility.FromJson<StudyLessonCatalog>(t.text); var asset=AssetDatabase.LoadAssetAtPath<StudyLessonCatalogAsset>(a); if(asset==null){asset=ScriptableObject.CreateInstance<StudyLessonCatalogAsset>();AssetDatabase.CreateAsset(asset,a);} asset.lessons=source.lessons; EditorUtility.SetDirty(asset); AssetDatabase.SaveAssets(); AssetDatabase.Refresh(); Debug.Log("Study Lesson Catalog ready: "+a); }
}
#endif
