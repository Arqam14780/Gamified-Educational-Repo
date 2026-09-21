using UnityEngine;


namespace AR
{
    [CreateAssetMenu(
    fileName = "ImageLesson",
    menuName = "Study System/Image Lesson"
)]
    public class StudyImageData : ScriptableObject
    {
        [Header("Lesson Information")]
        public string lessonType;
        [Header("Image")]
        public Sprite lessonImage;
        [TextArea(3, 10)]
        public string lessonText;
    }
}
