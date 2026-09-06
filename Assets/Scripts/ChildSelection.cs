using UnityEngine;
using System.Collections.Generic;

namespace AR
{
    public class ChildSelection : MonoBehaviour
    {
        public GameObject[] childSelectionUI;

        private List<GameObject> children = new List<GameObject>();
        private int currentActiveChild = 0;

        void Awake()
        {
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(false);
                children.Add(child.gameObject);
            }
        }

        private void Start()
        {
            currentActiveChild = PlayerPrefs.GetInt("ChildIndex", 0);
            children[currentActiveChild].SetActive(true);
            childSelectionUI[currentActiveChild].SetActive(true);
        }

        public void SelectChild(int num)
        {
            children[currentActiveChild].SetActive(false);
            childSelectionUI[currentActiveChild].SetActive(false);

            children[num].SetActive(true);
            childSelectionUI[num].SetActive(true);

            currentActiveChild = num;
        }

        public void Play()
        {
            PlayerPrefs.SetInt("ChildIndex", currentActiveChild);
        }


    }
}
