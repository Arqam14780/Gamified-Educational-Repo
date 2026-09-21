using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using UnityEngine.Events;

namespace AR
{
    public class LearningController : MonoBehaviour
    {
        public static LearningController Instance;
        public TMP_Text scoreTxt;
        public GameObject mainBg;
        public GameObject imageViewer;
        public GameObject pdfViewer;
        public GameObject videoViewer;
        public GameObject quizViewer;
        [Header("Study UI Data")]
        public ImageData imageData;
        [Space(5)]
        public PdfData pdfData;
        [Space(5)]
        public VideoData videoData;
        [Space(5)]
        public QuizData quizData;

        [Serializable]
        public class ImageData
        {
            public TMP_Text imgHeadingTxt;
            public RectTransform viewport;
            public RectTransform imageRect;
            public Image imgSprite;
        }
        [Serializable]
        public class PdfData
        {
            public TMP_Text PdfHeadingTxt;
            public RectTransform pdfViewport;
            public RectTransform pdfRect;
            public Image pdfSprite;
            public TMP_Text PdfPageCounterTxt;
        }
        [Serializable]
        public class VideoData
        {
            public TMP_Text videoHeadingTxt;
            public VideoPlayer videoPlayer;
            public TMP_Text videoTypeTxt;
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
            public GameObject showResultBtn;
            [Header("Quiz Result Screen section")]
            public GameObject quizResultScreen;
            public TMP_Text quizResultTxt;
            public GameObject playRewardBtn;
            public GameObject tryAgainBtn;
        }

        [Header("Zoom Settings")]
        [SerializeField] private float zoomStep = 0.25f;
        [SerializeField] private float minZoom = 0.5f;
        [SerializeField] private float maxZoom = 4f;
        public UnityEvent onQuizAction;

        private float currentZoom = 1f;
        private int totalPdfPage = 1;
        private int currentPageNum = 1;
        private Sprite[] pdfPages;
        private QuizInfo[] quizesInfo;
        private int currentQuizInd = 0;
        private Color color;
        private int correctQuizNum = 0;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
        }

        #region ImgRelatedContent
        public void ViewImgData(string txt, Sprite sprite)
        {
            mainBg.SetActive(true);
            imageViewer.SetActive(true);
            imageData.imgHeadingTxt.text = txt;
            imageData.imgSprite.sprite = sprite;
        }

        public void ZoomInImg()
        {
            currentZoom += zoomStep;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            ApplyZoom(imageData.imageRect);
        }
        public void ZoomoutImg()
        {
            currentZoom -= zoomStep;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            ApplyZoom(imageData.imageRect);
        }
        public void FitImg()
        {
            CalculateFitZoom(imageData.imageRect, imageData.viewport);
        }

        public void CloseImgData()
        {
            mainBg.SetActive(false);
            imageViewer.SetActive(false);
            imageData.imgHeadingTxt.text = "";
            imageData.imgSprite.sprite = null;
            currentZoom = 1f;
        }
        #endregion

        #region PdfRelatedContent
        public void ViewPdfData(string txt, Sprite[] sprite)
        {
            totalPdfPage = sprite.Length;
            pdfPages = new Sprite[] { };
            pdfPages = sprite;
            mainBg.SetActive(true);
            pdfViewer.SetActive(true);
            pdfData.PdfHeadingTxt.text = txt;
            pdfData.pdfSprite.sprite = pdfPages[currentPageNum];
            UpdatePdfPageNum(currentPageNum);
        }

        public void NextPage()
        {
            if (currentPageNum + 1 <= totalPdfPage)
            {
                pdfData.pdfSprite.sprite = pdfPages[currentPageNum];
                currentPageNum++;
                UpdatePdfPageNum(currentPageNum);
            }
        }

        public void PreviousPage()
        {
            if (currentPageNum - 1 > 0)
            {
                currentPageNum--;
                pdfData.pdfSprite.sprite = pdfPages[currentPageNum];
                UpdatePdfPageNum(currentPageNum);
            }
        }
        private void UpdatePdfPageNum(int pageNum)
        {
            pdfData.PdfPageCounterTxt.text = "Pdf page " + currentPageNum + " / " + totalPdfPage;
        }

        public void ZoomInPdf()
        {
            currentZoom += zoomStep;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            ApplyZoom(pdfData.pdfRect);
        }
        public void ZoomoutPdf()
        {
            currentZoom -= zoomStep;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

            ApplyZoom(pdfData.pdfRect);
        }
        public void FitPdf()
        {
            CalculateFitZoom(pdfData.pdfRect, pdfData.pdfViewport);
        }

        public void ClosePdfData()
        {
            mainBg.SetActive(false);
            pdfViewer.SetActive(false);
            pdfData.PdfHeadingTxt.text = "";
            pdfData.pdfSprite.sprite = null;
            currentZoom = 1f;
            totalPdfPage = 1;
            currentPageNum = 1;
            pdfPages = null;
        }

        #endregion

        private void ApplyZoom(RectTransform rect)
        {
            rect.localScale = Vector3.one * currentZoom;
        }
        private void CalculateFitZoom(RectTransform rect, RectTransform viewport)
        {
            float viewportWidth = viewport.rect.width;
            float viewportHeight = viewport.rect.height;

            float imageWidth = imageData.imageRect.rect.width;
            float imageHeight = imageData.imageRect.rect.height;

            if (imageWidth <= 0 || imageHeight <= 0)
            {
                currentZoom = 1f;
                ApplyZoom(rect);
                return;
            }

            float widthScale = viewportWidth / imageWidth;
            float heightScale = viewportHeight / imageHeight;

            currentZoom = Mathf.Min(widthScale, heightScale);
            ApplyZoom(rect);
        }

        #region VideoRelatedContent
        public void PlayVideoData(string txt, VideoClip clip, string videoLessonType)
        {
            mainBg.SetActive(true);
            videoViewer.SetActive(true);
            videoData.videoHeadingTxt.text = txt;
            videoData.videoPlayer.clip = clip;
            videoData.videoPlayer.Play();
            videoData.videoTypeTxt.text = videoLessonType;
        }

        public void CloseVideo()
        {
            mainBg.SetActive(false);
            videoViewer.SetActive(false);
            videoData.videoHeadingTxt.text = "";
            videoData.videoTypeTxt.text = "";
            videoData.videoPlayer.Stop();
            videoData.videoPlayer.clip = null;
            videoData.videoPlayer.targetTexture.Release();
        }
        #endregion

        #region QuizRelatedContent
        public void ViewQuizData(string headingTxt, QuizInfo[] quizInformation)
        {
            quizData.quizHeadingTxt.text = headingTxt;
            quizesInfo = new QuizInfo[] { };
            quizesInfo = quizInformation;
            mainBg.SetActive(true);
            quizViewer.SetActive(true);
            UpdateQuiz();
        }

        private void UpdateQuiz()
        {
            quizData.ansResultTxt.text = "";
            quizData.nextBtn.SetActive(false);

            quizData.quizQuestionTxt.text = quizesInfo[currentQuizInd].question;
            quizData.quizAnswerTxts[0].text = quizesInfo[currentQuizInd].options[0];
            quizData.quizAnswerTxts[1].text = quizesInfo[currentQuizInd].options[1];
            quizData.quizAnswerTxts[2].text = quizesInfo[currentQuizInd].options[2];
            quizData.quizAnswerTxts[3].text = quizesInfo[currentQuizInd].options[3];
            foreach (var btns in quizData.quizAnswerBtns) btns.GetComponent<Button>().interactable = true;
        }

        public void CheckQuizAns(int ansInd)
        {
            foreach (var btns in quizData.quizAnswerBtns) btns.GetComponent<Button>().interactable = false;

            if ((currentQuizInd + 1) >= quizesInfo.Length)
            {
                quizData.showResultBtn.SetActive(true);
                quizData.quizCloseBtn.SetActive(false);
            }
            else
                quizData.nextBtn.SetActive(true);

            if (ansInd == quizesInfo[currentQuizInd].correctAnswer)
            {
                correctQuizNum++;
                quizData.ansResultTxt.text = "Correct Answer!";

                ColorUtility.TryParseHtmlString("#007100", out Color color);
                quizData.quizAnswerBtns[quizesInfo[currentQuizInd].correctAnswer].GetComponent<Image>().color = color;
            }
            else
            {
                quizData.ansResultTxt.text = "Wrong Answer. Correct Answer is " + quizesInfo[currentQuizInd].options[quizesInfo[currentQuizInd].correctAnswer];
                
                quizData.quizAnswerBtns[ansInd].GetComponent<Image>().color = Color.red;
                ColorUtility.TryParseHtmlString("#007100", out Color color);
                quizData.quizAnswerBtns[quizesInfo[currentQuizInd].correctAnswer].GetComponent<Image>().color = color;
            }

            quizData.quizRecordTxt.text = "Question " + (currentQuizInd + 1)
                + " / " + quizesInfo.Length + " | Score : " + correctQuizNum;
        }

        public void NextQuiz()
        {
            currentQuizInd++;
            UpdateQuiz();

            ColorUtility.TryParseHtmlString("#0F6E78", out Color color);
            foreach (var btns in quizData.quizAnswerBtns)
                btns.GetComponent<Image>().color = color;
        }

        public void ShowQuizResult()
        {
            quizData.quizResultScreen.SetActive(true);
            if (correctQuizNum == quizesInfo.Length)
            {
                quizData.quizResultTxt.text = "You Successfully completed your quiz and got " + correctQuizNum +
                    " out of " + correctQuizNum;
                quizData.playRewardBtn.SetActive(true);
            }
            else
            {
                quizData.quizResultTxt.text = "Nice try! You got " + correctQuizNum + " out of " + quizesInfo.Length;
                quizData.tryAgainBtn.SetActive(true);
            }
            quizData.ansResultTxt.text = "";
            quizData.nextBtn.SetActive(false);
            quizData.showResultBtn.SetActive(false);
        }

        public void PlayRewardbtnClicked()
        {
            QuizCloseBtnClicked();
            onQuizAction?.Invoke();
        }
        public void TryAgainQuiz()
        {
            ResetQuizData();
            UpdateQuiz();
        }

        public void QuizCloseBtnClicked()
        {
            ResetQuizData();
            quizViewer.SetActive(false);
            mainBg.SetActive(false);
            quizesInfo = null;
        }

        private void ResetQuizData()
        {
            currentQuizInd = correctQuizNum = 0;
            quizData.quizResultScreen.SetActive(false);
            quizData.quizResultTxt.text = "";
            quizData.playRewardBtn.SetActive(false);
            quizData.tryAgainBtn.SetActive(false);
            quizData.quizCloseBtn.SetActive(true);
            quizData.quizRecordTxt.text = "Question " + (currentQuizInd + 1)
                + " / " + quizesInfo.Length + " | Score : " + correctQuizNum;

            ColorUtility.TryParseHtmlString("#0F6E78", out Color color);
            foreach (var btns in quizData.quizAnswerBtns)
                btns.GetComponent<Image>().color = color;
        }

        #endregion

    }
}