using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AR
{
    public class LearningController : MonoBehaviour
    {
        public TMP_Text scoreTxt;
        public GameObject mainBg;
        public GameObject imageViewer;
        public GameObject pdfViewer;
        public GameObject quizViewer;
        [Header("Study UI Data")]
        public ImageData imageData;
        [Space(5)]
        public PdfData pdfData;
        [Space(5)]
        public QuizData quizData;

        [Serializable]
        public class ImageData
        {
            public TMP_Text imgHeadingTxt;
            public Image imgSprite;
        }
        [Serializable]
        public class PdfData
        {
            public TMP_Text PdfHeadingTxt;
            public Image pdfSprite;
            public TMP_Text PdfPageCounterTxt;
        }
        [Serializable]
        public class QuizData
        {
            public GameObject quizCloseBtn;
            public TMP_Text quizRecordTxt;
            public TMP_Text quizHeadingTxt;
            public TMP_Text quizQuestionTxt;
            public TMP_Text[] quizAnswerTxts;
            public GameObject[] quizAnswerBtns;
            public TMP_Text ansResultTxt;
            public GameObject nextBtn;
            [Header("Quiz Result Screen section")]
            public GameObject quizResultScreen;
            public TMP_Text quizResultTxt;
            public GameObject playRewardBtn;
            public GameObject tryAgainBtn;
        }

    
    }
}
