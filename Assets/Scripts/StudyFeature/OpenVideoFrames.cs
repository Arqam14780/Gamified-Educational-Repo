using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

namespace AR
{
    public class OpenVideoFrames : MonoBehaviour
    {
        [SerializeField]
        private StudyContentDatabase[] studyContent;
        [Header("Pdf Related Content")]
        public VideoFrame[] videoFrames;

        private int stageNum = 1;

        void Start()
        {
            stageNum = PlayerPrefs.GetInt("Stage", 1);
            int NumOfVidContent = studyContent[stageNum - 1].videoLessons.Length;

            if (videoFrames.Length > NumOfVidContent)
            {
                for (int i = studyContent[stageNum - 1].videoLessons.Length; i < videoFrames.Length; i++)
                    videoFrames[i].videoFrameScreen.SetActive(false);
            }

            for (int i = 0; i < studyContent[stageNum - 1].videoLessons.Length; i++)
            {
                videoFrames[i].videoLessonTxt.text = studyContent[stageNum - 1].videoLessons[i].lessonText;
                videoFrames[i].videoSprite.sprite = studyContent[stageNum - 1].videoLessons[i].videoThumbnail;
                videoFrames[i].lessonTypeTxt.text = studyContent[stageNum - 1].videoLessons[i].lessonType;
            }

            LearningController.Instance.InitializeVideoProgress(NumOfVidContent);
        }

        public void OpenVideoFrame(int frameInd)
        {
            LearningController.Instance.PlayVideoData(
                frameInd,
                studyContent[stageNum - 1].videoLessons[frameInd].lessonText,
                studyContent[stageNum - 1].videoLessons[frameInd].videoClip,
                studyContent[stageNum - 1].videoLessons[frameInd].lessonType);
        }

        public void DisableVideoFrame()
        {
            for (int i = 0; i < videoFrames.Length; i++)
                videoFrames[i].videoFrameScreen.SetActive(false);
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
