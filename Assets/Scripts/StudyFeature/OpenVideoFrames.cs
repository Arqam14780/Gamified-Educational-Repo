using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

namespace AR
{
    public class OpenVideoFrames : MonoBehaviour
    {
        [SerializeField]
        private StudyContentDatabase studyContent;
        [Header("Pdf Related Content")]
        public VideoFrame[] videoFrames;

        void Start()
        {
            if (videoFrames.Length > studyContent.videoLessons.Length)
            {
                for (int i = studyContent.videoLessons.Length; i < videoFrames.Length; i++)
                    videoFrames[i].videoFrameScreen.SetActive(false);
            }

            for (int i = 0; i < studyContent.videoLessons.Length; i++)
            {
                videoFrames[i].videoLessonTxt.text = studyContent.videoLessons[i].lessonText;
                videoFrames[i].videoSprite.sprite = studyContent.videoLessons[i].videoThumbnail;
                videoFrames[i].lessonTypeTxt.text = studyContent.videoLessons[i].lessonType;
            }
        }

        public void OpenVideoFrame(int frameInd)
        {
            LearningController.Instance.PlayVideoData(
                studyContent.videoLessons[frameInd].lessonText,
                studyContent.videoLessons[frameInd].videoClip);
        }

    }
    [Serializable]
    public class VideoFrame
    {
        public string name;
        public GameObject videoFrameScreen;
        public TMP_Text videoLessonTxt;
        public Image videoSprite;
        public TMP_Text lessonTypeTxt;
    }
}
