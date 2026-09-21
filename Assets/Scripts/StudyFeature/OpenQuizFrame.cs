using UnityEngine;
using System;
using TMPro;

namespace AR
{
    public class OpenQuizFrame : MonoBehaviour
    {
        [SerializeField]
        private StudyContentDatabase studyContent;
        [Header("Quiz Related Content")]
        public QuizFrame[] quizFrame;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (quizFrame.Length > studyContent.quizzes.Length)
            {
                for (int i = studyContent.quizzes.Length; i < quizFrame.Length; i++)
                    quizFrame[i].quizFrameScreen.SetActive(false);
            }

            for (int i = 0; i < studyContent.quizzes.Length; i++)
            {
                quizFrame[i].quizTypeTxt.text = studyContent.quizzes[i].quizType;
            }
        }

        public void OpenQuizData(int frameInd)
        {
            LearningController.Instance.ViewQuizData(
                studyContent.quizzes[frameInd].quizType,
                studyContent.quizzes[frameInd].quizInfo);
        }

        public void DisableQuizFrame()
        {
            for (int i = 0; i < quizFrame.Length; i++)
                quizFrame[i].quizFrameScreen.SetActive(false);
        }
    }

    [Serializable]
    public class QuizFrame
    {
        public string name;
        public GameObject quizFrameScreen;
        public TMP_Text quizTypeTxt;
    }

}
