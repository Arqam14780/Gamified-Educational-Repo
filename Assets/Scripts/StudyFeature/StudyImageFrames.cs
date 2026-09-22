using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

namespace AR {
    public class StudyImageFrames : MonoBehaviour
    {
        [SerializeField]
        private StudyContentDatabase[] studyContent;
        [Header("Images Related Content")]
        public ImageFrame[] imageFrames;

        private int stageNum = 1;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            stageNum = PlayerPrefs.GetInt("Stage", 1);
            if (imageFrames.Length > studyContent[stageNum-1].imageLessons.Length)
            {
                for (int i = studyContent[stageNum - 1].imageLessons.Length; i < imageFrames.Length; i++)
                    imageFrames[i].imageFrameScreen.SetActive(false);
            }

            for (int i=0; i<studyContent[stageNum - 1].imageLessons.Length; i++)
            {
                imageFrames[i].imgLessonTxt.text = studyContent[stageNum - 1].imageLessons[i].lessonText;
                imageFrames[i].imgSprite.sprite = studyContent[stageNum - 1].imageLessons[i].lessonImage;
                imageFrames[i].lessonTypeTxt.text = studyContent[stageNum - 1].imageLessons[i].lessonType;
            }
        }

        public void OpenImageFrame(int frameInd)
        {
            LearningController.Instance.ViewImgData(
                studyContent[stageNum - 1].imageLessons[frameInd].lessonText, 
                studyContent[stageNum - 1].imageLessons[frameInd].lessonImage);
        }

        public void DisableImageFrame()
        {
            for (int i = 0; i < imageFrames.Length; i++)
                imageFrames[i].imageFrameScreen.SetActive(false);
        }

    }

    [Serializable]
    public class ImageFrame
    {
        public string name;
        public GameObject imageFrameScreen;
        public TMP_Text imgLessonTxt;
        public Image imgSprite;
        public TMP_Text lessonTypeTxt;
    }
}
