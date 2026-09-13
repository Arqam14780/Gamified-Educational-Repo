using System;
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
            var asset = Resources.Load<TextAsset>("Lessons/catalog");
            if (asset == null) throw new InvalidOperationException("Missing Lessons/catalog learning content.");
            var catalog = JsonUtility.FromJson<StudyLessonCatalog>(asset.text);
            if (catalog == null || catalog.lessons == null || catalog.lessons.Length == 0)
                throw new InvalidOperationException("The lesson catalog is empty.");
            foreach (var lesson in catalog.lessons)
            {
                if (string.IsNullOrEmpty(lesson.subject) || string.IsNullOrEmpty(lesson.imageResource) ||
                    lesson.pdfPages == null || lesson.pdfPages.Length == 0 || lesson.questions == null || lesson.questions.Length == 0)
                    throw new InvalidOperationException("A lesson is missing its image, PDF pages or quiz.");
                foreach (var question in lesson.questions)
                    if (question.options == null || question.options.Length != 4 || question.correct < 0 || question.correct >= 4 || string.IsNullOrEmpty(question.source))
                        throw new InvalidOperationException("Every question needs four answers and a learning source.");
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
