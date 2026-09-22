using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

namespace AR
{
    public class StudyPdfFrames : MonoBehaviour
    {
        [SerializeField]
        private StudyContentDatabase[] studyContent;
        [Header("Pdf Related Content")]
        public PdfFrame[] pdfFrames;

        private int stageNum = 1;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            stageNum = PlayerPrefs.GetInt("Stage", 1);
            if (pdfFrames.Length > studyContent[stageNum - 1].pdfLessons.Length)
            {
                for (int i = studyContent[stageNum - 1].pdfLessons.Length; i < pdfFrames.Length; i++)
                    pdfFrames[i].pdfFrameScreen.SetActive(false);
            }

            for (int i = 0; i < studyContent[stageNum - 1].pdfLessons.Length; i++)
            {
                pdfFrames[i].pdfLessonTxt.text = studyContent[stageNum - 1].pdfLessons[i].lessonText;
                pdfFrames[i].pdfSprite.sprite = studyContent[stageNum - 1].pdfLessons[i].pages[0];
                pdfFrames[i].lessonTypeTxt.text = studyContent[stageNum - 1].pdfLessons[i].lessonType;
            }
        }

        public void OpenPdfFrame(int frameInd)
        {
            LearningController.Instance.ViewPdfData(
                studyContent[stageNum - 1].pdfLessons[frameInd].lessonText,
                studyContent[stageNum - 1].pdfLessons[frameInd].pages);
        }

        public void DisablePdfFrame()
        {
            for (int i = 0; i < pdfFrames.Length; i++)
                pdfFrames[i].pdfFrameScreen.SetActive(false);
        }

    }

    [Serializable]
    public class PdfFrame
    {
        public string name;
        public GameObject pdfFrameScreen;
        public TMP_Text pdfLessonTxt;
        public Image pdfSprite;
        public TMP_Text lessonTypeTxt;
    }
}
