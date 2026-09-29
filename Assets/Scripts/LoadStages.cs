using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AR
{
    public class LoadStages : MonoBehaviour
    {
        public enum OrientationType { Potrait, Landscape }
        public OrientationType orientationType;

        public Image ref_Slider;
        public Text ref_loadingBarText;

        AsyncOperation ref_Loading_Sync;
        bool canLoad = false;
        private int sceneNumber = 0;

        void Start()
        {
            Time.timeScale = 1;
            if (orientationType.Equals(OrientationType.Potrait))
                Screen.orientation = ScreenOrientation.Portrait;
        }

        public void LoadActivity(int sceneIndex)
        {
            sceneNumber = sceneIndex;
            Invoke("startnextScene", 1f);
        }

        void startnextScene()
        {

            ref_Loading_Sync = SceneManager.LoadSceneAsync(sceneNumber);
            canLoad = true;
        }


        void Update()
        {
            if (canLoad)
            {
                ref_Slider.fillAmount = ref_Loading_Sync.progress;

                float val = ref_Loading_Sync.progress * 100;
                ref_loadingBarText.text = (Mathf.Round(val) + " %");
            }
        }


    }
}
