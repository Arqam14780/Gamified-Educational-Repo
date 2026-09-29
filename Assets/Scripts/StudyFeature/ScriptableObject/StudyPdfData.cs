using UnityEngine;

namespace AR
{
    [CreateAssetMenu(
        fileName = "PdfLesson",
        menuName = "Study System/PDF Lesson"
    )]
    public class StudyPdfData : ScriptableObject
    {
        [Header("Lesson Information")]
        public string lessonType;

        [Header("PDF Thumbnail")]
        public Sprite pdfIcon;

        [Header("PDF Pages")]
        public Sprite[] pages;

        [TextArea(3, 10)]
        public string lessonText;
    }
}