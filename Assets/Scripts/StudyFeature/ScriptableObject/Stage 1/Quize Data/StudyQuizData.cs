using UnityEngine;
using System;

namespace AR
{
    [CreateAssetMenu(
        fileName = "QuizLesson",
        menuName = "Study System/Quiz Lesson"
    )]
    public class StudyQuizData : ScriptableObject
    {
        [Header("Quiz Information")]
        public string quizType;

        [Header("Questions")]
        public QuizInfo[] quizInfo;
    }

    [Serializable]
    public class QuizInfo
    {
        public string name;
        [TextArea(2, 5)]
        public string question;

        [Header("Four Options")]
        public string[] options;
        [Header("Correct Answer")]
        [Range(0, 3)]
        public int correctAnswer;
    }

}