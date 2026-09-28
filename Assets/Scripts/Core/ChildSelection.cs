using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace AR
{
    public class ChildSelection : MonoBehaviour
    {
        [SerializeField] private SoundManager soundManager;
        public GameObject[] stagePointers;
        public GameObject selectBtnPanel;
        public GameObject rewardedActivityPanel;

        private List<GameObject> children = new List<GameObject>();
        private int currentActiveChild = 0;

        void Awake()
        {
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(false);
                children.Add(child.gameObject);
                AcademyCharacterMaterials.Prepare(child.gameObject);
            }
        }

        private void OnEnable()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }

        private void Start()
        {
            if (children.Count == 0) { Debug.LogError("Add learner models to ChildSelection.", this); return; }
            currentActiveChild = Mathf.Clamp(PlayerPrefs.GetInt("ChildIndex", 0), 0, children.Count - 1);
            children[currentActiveChild].SetActive(true);
            var menu = FindFirstObjectByType<AcademyMenu>();
            if (menu != null) menu.Initialize(this, currentActiveChild);
            else Debug.LogError("MainMenu needs its saved Academy UI. Use Tools > Academy > Save menu UI to scene.", this);

            foreach (GameObject pointer in stagePointers) pointer.SetActive(false);
            int stageNum = PlayerPrefs.GetInt("Stage", 1);
            stagePointers[stageNum - 1].SetActive(true);
            if (PlayerPrefs.GetInt("UnlockAllStages", 0) == 1)
            {
                rewardedActivityPanel.SetActive(true);
                selectBtnPanel.SetActive(true);
            }
        }

        public void SelectChild(int num)
        {
            soundManager.PlayBtnSound();
            if (num < 0 || num >= children.Count) return;
            children[currentActiveChild].SetActive(false);

            children[num].SetActive(true);

            currentActiveChild = num;
        }

        public void Play()
        {
            soundManager.PlayBtnSound();
            PlayerPrefs.SetInt("ChildIndex", currentActiveChild);
            PlayerPrefs.Save();
            SceneManager.LoadScene(/*"StudyRoom"*/"GamePlay");
        }

        public void PlayTicTacToe()
        {
            soundManager.PlayBtnSound();
            SceneManager.LoadScene("TicTacToe");
        }

        public void PlayDotAndBox()
        {
            soundManager.PlayBtnSound();
            SceneManager.LoadScene("DotAndBox");
        }

        public void PlayActivityThird()
        {
            soundManager.PlayBtnSound();
            SceneManager.LoadScene("BirdSort");
        }

        public void SelectActivity(int ind)
        {
            soundManager.PlayBtnSound();
            foreach (GameObject pointer in stagePointers) pointer.SetActive(false);
            stagePointers[ind].SetActive(true);
            PlayerPrefs.SetInt("Stage", ind + 1);
        }


    }
}
