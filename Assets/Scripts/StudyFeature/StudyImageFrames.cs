using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

namespace AR {
    public class StudyImageFrames : MonoBehaviour
    {
        [SerializeField]
        private StudyContentDatabase studyContent;
        [Header("Images Related Content")]
        public ImageFrame[] imageFrames;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (imageFrames.Length > studyContent.imageLessons.Length)
            {
                for (int i = studyContent.imageLessons.Length; i < imageFrames.Length; i++)
                    imageFrames[i].imageFrameScreen.SetActive(false);
            }

            for (int i=0; i<studyContent.imageLessons.Length; i++)
            {
                imageFrames[i].imgLessonTxt.text = studyContent.imageLessons[i].lessonText;
                imageFrames[i].imgSprite.sprite = studyContent.imageLessons[i].lessonImage;
                imageFrames[i].lessonTypeTxt.text = studyContent.imageLessons[i].lessonType;
            }
        }

        public void OpenImageFrame(int frameInd)
        {
            LearningController.Instance.ViewImgData(
                studyContent.imageLessons[frameInd].lessonText, 
                studyContent.imageLessons[frameInd].lessonImage);
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
