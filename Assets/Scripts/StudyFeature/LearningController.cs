using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

namespace AR
{
    public class LearningController : MonoBehaviour
    {
        public static LearningController Instance;
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

        [Header("Zoom Settings")]
        [SerializeField] private float zoomStep = 0.25f;
        [SerializeField] private float minZoom = 0.5f;
        [SerializeField] private float maxZoom = 4f;
        private float currentZoom = 1f;
        private int totalPdfPage = 1;
        private int currentPageNum = 1;
        private Sprite[] pdfPages;

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
            pdfData.PdfPageCounterTxt.text = "Pdf page " + currentPageNum + " / "  + totalPdfPage;
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


        public void PlayVideoData(string txt, VideoClip clip)
        {

        }

    }
}