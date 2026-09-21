using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

namespace AR
{
    public class StudyPdfFrames : MonoBehaviour
    {
        [SerializeField]
        private StudyContentDatabase studyContent;
        [Header("Pdf Related Content")]
        public PdfFrame[] pdfFrames;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (pdfFrames.Length > studyContent.pdfLessons.Length)
            {
                for (int i = studyContent.pdfLessons.Length; i < pdfFrames.Length; i++)
                    pdfFrames[i].pdfFrameScreen.SetActive(false);
            }

            for (int i = 0; i < studyContent.pdfLessons.Length; i++)
            {
                pdfFrames[i].pdfLessonTxt.text = studyContent.pdfLessons[i].lessonText;
                pdfFrames[i].pdfSprite.sprite = studyContent.pdfLessons[i].pages[0];
                pdfFrames[i].lessonTypeTxt.text = studyContent.pdfLessons[i].lessonType;
            }
        }

        public void OpenPdfFrame(int frameInd)
        {
            LearningController.Instance.ViewPdfData(
                studyContent.pdfLessons[frameInd].lessonText,
                studyContent.pdfLessons[frameInd].pages);
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
