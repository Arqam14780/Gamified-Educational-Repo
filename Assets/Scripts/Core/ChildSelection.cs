using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace AR
{
    public class ChildSelection : MonoBehaviour
    {
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

        private void Start()
        {
            if (children.Count == 0) { Debug.LogError("Add learner models to ChildSelection.", this); return; }
            currentActiveChild = Mathf.Clamp(PlayerPrefs.GetInt("ChildIndex", 0), 0, children.Count - 1);
            children[currentActiveChild].SetActive(true);
            var menu = FindFirstObjectByType<AcademyMenu>();
            if (menu != null) menu.Initialize(this, currentActiveChild);
            else Debug.LogError("MainMenu needs its saved Academy UI. Use Tools > Academy > Save menu UI to scene.", this);
        }

        public void SelectChild(int num)
        {
            if (num < 0 || num >= children.Count) return;
            children[currentActiveChild].SetActive(false);

            children[num].SetActive(true);

            currentActiveChild = num;
        }

        public void Play()
        {
            PlayerPrefs.SetInt("ChildIndex", currentActiveChild);
            PlayerPrefs.Save();
            SceneManager.LoadScene("StudyRoom");
        }



    }
}
