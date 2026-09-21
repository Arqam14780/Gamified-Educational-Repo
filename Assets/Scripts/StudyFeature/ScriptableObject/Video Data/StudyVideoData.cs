using UnityEngine;
using UnityEngine.Video;

namespace AR
{
    [CreateAssetMenu(
        fileName = "VideoLesson",
        menuName = "Study System/Video Lesson"
    )]
    public class StudyVideoData : ScriptableObject
    {
        [Header("Lesson Information")]
        public string lessonType;

        [Header("Image")]
        public Sprite videoThumbnail;

        [TextArea(3, 10)]
        public string lessonText;

        [Header("Video")]
        public VideoClip videoClip;
        
    }
}