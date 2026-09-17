using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AR
{
    [Serializable]
    public sealed class StudyQuestion
    {
        public string prompt;
        public string[] options;
        public int correct;
        public string source;
    }

    [Serializable]
    public sealed class StudyLesson
    {
        public string subject;
        public string title;
        public string imageResource;
        public string[] pdfPages;
        public string pdfFile;
        public StudyQuestion[] questions;
    }

    [Serializable]
    public sealed class StudyLessonCatalog
    {
        public StudyLesson[] lessons;

        public static StudyLessonCatalog Load()
        {
            string external = Path.Combine(Application.persistentDataPath, "Academy", "catalog.json");
            if (File.Exists(external))
            {
                try
                {
                    var supplied = FromJson(File.ReadAllText(external));
                    StudyContent.Root = Path.GetDirectoryName(external);
                    return supplied;
                }
                catch (Exception error) { Debug.LogWarning("Academy content could not load; using bundled lessons. " + error.Message); }
            }
            StudyContent.Root = null;
            var asset = Resources.Load<TextAsset>("Lessons/catalog");
            if (asset == null) throw new InvalidOperationException("Missing Lessons/catalog learning content.");
            return FromJson(asset.text);
        }

        public static StudyLessonCatalog FromJson(string json)
        {
            var catalog = JsonUtility.FromJson<StudyLessonCatalog>(json);
            if (catalog == null || catalog.lessons == null || catalog.lessons.Length == 0)
                throw new InvalidOperationException("The lesson catalog is empty.");
            var subjects = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var lesson in catalog.lessons)
            {
                if (lesson == null || string.IsNullOrWhiteSpace(lesson.subject) || string.IsNullOrEmpty(lesson.imageResource) ||
                    lesson.pdfPages == null || lesson.pdfPages.Length == 0 || lesson.questions == null || lesson.questions.Length == 0)
                    throw new InvalidOperationException("A lesson is missing its image, PDF pages or quiz.");
                if (!subjects.Add(lesson.subject)) throw new InvalidOperationException("Subject names must be unique: " + lesson.subject);
                if (string.IsNullOrWhiteSpace(lesson.title)) throw new InvalidOperationException("Every lesson needs a title.");
                foreach (var page in lesson.pdfPages)
                    if (string.IsNullOrWhiteSpace(page)) throw new InvalidOperationException("PDF page paths cannot be empty.");
                foreach (var question in lesson.questions)
                {
                    if (question == null || string.IsNullOrWhiteSpace(question.prompt) || question.options == null || question.options.Length != 4 || question.correct < 0 || question.correct >= 4 || string.IsNullOrEmpty(question.source))
                        throw new InvalidOperationException("Every question needs four answers and a learning source.");
                    foreach (var option in question.options)
                        if (string.IsNullOrWhiteSpace(option)) throw new InvalidOperationException("Quiz answers cannot be empty.");
                }
            }
            return catalog;
        }

        public StudyLesson Find(string subject)
        {
            foreach (var lesson in lessons) if (lesson.subject == subject) return lesson;
            throw new InvalidOperationException("No learning pack for " + subject);
        }
    }
}
