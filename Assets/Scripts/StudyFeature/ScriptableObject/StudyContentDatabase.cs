using UnityEngine;

namespace AR
{
    [CreateAssetMenu(
        fileName = "StudyContentDatabase",
        menuName = "Study System/Study Content Database"
    )]
    public class StudyContentDatabase : ScriptableObject
    {
        [Header("Image Lessons")]
        public StudyImageData[] imageLessons;

        [Header("PDF Lessons")]
        public StudyPdfData[] pdfLessons;

        [Header("Video Lessons")]
        public StudyVideoData[] videoLessons;

        [Header("Quizzes")]
        public StudyQuizData[] quizzes;
    }
}