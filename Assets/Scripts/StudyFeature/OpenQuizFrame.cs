using UnityEngine;
using System;
using TMPro;

namespace AR
{
    public class OpenQuizFrame : MonoBehaviour
    {
        [SerializeField]
        private StudyContentDatabase[] studyContent;
        [Header("Quiz Related Content")]
        public QuizFrame[] quizFrame;

        private int stageNum = 1;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            stageNum = PlayerPrefs.GetInt("Stage", 1);
            if (quizFrame.Length > studyContent[stageNum - 1].quizzes.Length)
            {
                for (int i = studyContent[stageNum - 1].quizzes.Length; i < quizFrame.Length; i++)
                    quizFrame[i].quizFrameScreen.SetActive(false);
            }

            for (int i = 0; i < studyContent[stageNum - 1].quizzes.Length; i++)
            {
                quizFrame[i].quizTypeTxt.text = studyContent[stageNum - 1].quizzes[i].quizType;
            }
        }

        public void OpenQuizData(int frameInd)
        {
            LearningController.Instance.ViewQuizData(
                studyContent[stageNum - 1].quizzes[frameInd].quizType,
                studyContent[stageNum - 1].quizzes[frameInd].quizInfo);
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
