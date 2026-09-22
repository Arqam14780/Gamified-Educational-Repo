using System;
using System.Collections.Generic;
using UnityEngine;
namespace AR
{
 [Serializable] public sealed class StudyQuestion { public string prompt; public string[] options; public int correct; public string source; }
 [Serializable] public sealed class StudyLesson { public string subject; public string title; public string imageResource; public string[] pdfPages; public string pdfFile; public StudyQuestion[] questions; }
 [Serializable] public sealed class StudyLessonCatalog
 {
  public StudyLesson[] lessons;
  public static StudyLessonCatalog Load() { var asset=Resources.Load<StudyLessonCatalogAsset>("Lessons/StudyLessonCatalog"); if(asset==null) throw new InvalidOperationException("Missing StudyLessonCatalog.asset in Assets/Resources/Lessons."); StudyContent.Root=null; return asset.ToCatalog(); }
  public static StudyLessonCatalog FromJson(string json) { var c=JsonUtility.FromJson<StudyLessonCatalog>(json); if(c==null||c.lessons==null||c.lessons.Length==0) throw new InvalidOperationException("The lesson catalog is empty."); var subjects=new HashSet<string>(StringComparer.OrdinalIgnoreCase); foreach(var l in c.lessons) { if(l==null||string.IsNullOrWhiteSpace(l.subject)||string.IsNullOrEmpty(l.imageResource)||l.pdfPages==null||l.pdfPages.Length==0||l.questions==null||l.questions.Length==0) throw new InvalidOperationException("A lesson is missing its image, PDF pages or quiz."); if(!subjects.Add(l.subject)) throw new InvalidOperationException("Subject names must be unique: "+l.subject); } return c; }
  public StudyLesson Find(string subject) { foreach(var l in lessons) if(l.subject==subject) return l; throw new InvalidOperationException("No learning pack for "+subject); }
 }
 [CreateAssetMenu(menuName="Learning Academy/Study Lesson Catalog",fileName="StudyLessonCatalog")]
 public sealed class StudyLessonCatalogAsset : ScriptableObject { public StudyLesson[] lessons; public StudyLessonCatalog ToCatalog() { var c=new StudyLessonCatalog{lessons=lessons}; StudyLessonCatalog.FromJson(JsonUtility.ToJson(c)); return c; } }
}
