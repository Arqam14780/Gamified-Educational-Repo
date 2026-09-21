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
        public QuizQuestion[] questions;
    }

    [Serializable]
    public class QuizQuestion
    {
        [TextArea(2, 5)]
        public string question;

        [Header("Four Options")]
        public string optionA;
        public string optionB;
        public string optionC;
        public string optionD;

        [Header("Correct Answer")]
        [Range(0, 3)]
        public int correctAnswer;
    }

}