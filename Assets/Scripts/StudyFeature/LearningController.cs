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
        public GameObject landscapeLoading, PotraitLoading;
        public GameObject visibleContent;
        public GameObject confettiEffect;
        public SoundManager soundManager;
        public TMP_Text scoreTxt;
        public GameObject mainBg;
        public GameObject imageViewer;
        public GameObject pdfViewer;
        public GameObject videoViewer;
        public GameObject quizViewer;
        [HideInInspector]
        public bool isFrameOpened = false;
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

        public UnityEvent enableQuiz;
        [Header("Zoom Settings")]
        [SerializeField] private float zoomStep = 0.25f;
        [SerializeField] private float minZoom = 0.5f;
        [SerializeField] private float maxZoom = 4f;
        public UnityEvent onQuizAction;

        private float currentZoom = 1f;
        private int totalPdfPage = 1;
        private int currentPageNum = 1;
        public Sprite[] pdfPages;
        private QuizInfo[] quizesInfo;
        private int currentQuizInd = 0;
        private Color color;
        private int correctQuizNum = 0;
        private bool[] imageViewed;
        private bool[] pdfViewed;
        private bool[] videoViewed;
        private Camera mainCam;
        private Canvas canvas;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
        }

        private void Start()
        {
            canvas = this.gameObject.GetComponent<Canvas>();
            mainCam = Camera.main;
        }

        #region ImgRelatedContent
        public void InitializeImageProgress(int imageCount)
        {
            imageViewed = new bool[imageCount];
        }
        public void ViewImgData(int imgIndex, string txt, Sprite sprite)
        {
            isFrameOpened = true;
            mainBg.SetActive(true);
            imageViewer.SetActive(true);
            imageData.imgHeadingTxt.text = txt;
            imageData.imgSprite.sprite = sprite;

            if (!imageViewed[imgIndex])
                imageViewed[imgIndex] = true;

            if (CheckStudyProgress())
            {
                enableQuiz?.Invoke();
            }
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
            isFrameOpened = false;
            mainBg.SetActive(false);
            imageViewer.SetActive(false);
            imageData.imgHeadingTxt.text = "";
            imageData.imgSprite.sprite = null;
            currentZoom = 1f;
        }
        #endregion

        #region PdfRelatedContent
        public void InitializePdfProgress(int pdfCount)
        {
            pdfViewed = new bool[pdfCount];
        }
        public void ViewPdfData(int pdfIndex, string txt, Sprite[] sprite)
        {
            isFrameOpened = true;
            totalPdfPage = sprite.Length;
            pdfPages = new Sprite[] { };
            pdfPages = sprite;
            mainBg.SetActive(true);
            pdfViewer.SetActive(true);
            pdfData.PdfHeadingTxt.text = txt;
            pdfData.pdfSprite.sprite = pdfPages[currentPageNum - 1];
            UpdatePdfPageNum(currentPageNum);

            if (!pdfViewed[pdfIndex])
                pdfViewed[pdfIndex] = true;

            if (CheckStudyProgress())
            {
                enableQuiz?.Invoke();
            }
        }

        public void NextPage()
        {
            if (currentPageNum + 1 <= totalPdfPage)
            {
                pdfData.pdfSprite.overrideSprite = pdfPages[currentPageNum];
                currentPageNum++;
                UpdatePdfPageNum(currentPageNum);
            }
        }

        public void PreviousPage()
        {
            if (currentPageNum - 1 > 0)
            {
                currentPageNum--;
                pdfData.pdfSprite.overrideSprite = pdfPages[currentPageNum - 1];
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
            isFrameOpened = false;
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
        public void InitializeVideoProgress(int vidCount)
        {
            videoViewed = new bool[vidCount];
        }
        public void PlayVideoData(int vidIndex, string txt, VideoClip clip, string videoLessonType)
        {
            isFrameOpened = true;
            mainBg.SetActive(true);
            videoViewer.SetActive(true);
            videoData.videoHeadingTxt.text = txt;
            videoData.videoPlayer.clip = clip;
            videoData.videoPlayer.Play();
            videoData.videoTypeTxt.text = videoLessonType;
  
            if (!videoViewed[vidIndex])
                videoViewed[vidIndex] = true;

            if (CheckStudyProgress())
            {
                enableQuiz?.Invoke();
            }
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
            isFrameOpened = false;
        }
        #endregion

        private bool CheckStudyProgress()
        {
            return AreAllViewed(imageViewed) &&
                   AreAllViewed(pdfViewed) &&
                   AreAllViewed(videoViewed);
        }
        private bool AreAllViewed(bool[] items)
        {
            if (items == null || items.Length == 0)
                return true;

            for (int i = 0; i < items.Length; i++)
            {
                if (!items[i])
                    return false;
            }

            return true;
        }

        #region QuizRelatedContent
        public void ViewQuizData(string headingTxt, QuizInfo[] quizInformation)
        {
            isFrameOpened = true;
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
            soundManager.PlayBtnSound();
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
            soundManager.PlayBtnSound();
            currentQuizInd++;
            UpdateQuiz();

            ColorUtility.TryParseHtmlString("#0F6E78", out Color color);
            foreach (var btns in quizData.quizAnswerBtns)
                btns.GetComponent<Image>().color = color;
        }

        public void ShowQuizResult()
        {
            soundManager.PlayBtnSound();
            quizData.quizResultScreen.SetActive(true);
            if (correctQuizNum == quizesInfo.Length)
            {
                quizData.quizResultTxt.text = "You Successfully completed your quiz and got " + correctQuizNum +
                    " out of " + correctQuizNum;
                quizData.playRewardBtn.SetActive(true);
                visibleContent.SetActive(false);
                confettiEffect.SetActive(true);
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = mainCam;

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
            soundManager.PlayBtnSound();
            QuizCloseBtnClicked();
            onQuizAction?.Invoke();
            visibleContent.SetActive(true);
            confettiEffect.SetActive(false);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        public void TryAgainQuiz()
        {
            soundManager.PlayBtnSound();
            ResetQuizData();
            UpdateQuiz();
        }

        public void QuizCloseBtnClicked()
        {
            isFrameOpened = false;
            soundManager.PlayBtnSound();
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