using UnityEngine;
using UnityEngine.SceneManagement;

namespace AR
{
    public class Trigger : MonoBehaviour
    {
        private bool justOnetime = true;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && justOnetime)
            {
                justOnetime = false;
                Invoke("LoadActivityScene", 0.2f);
            }
        }

        private void LoadActivityScene()
        {
            int stageNum = PlayerPrefs.GetInt("Stage", 1);
            int sceneIndex = stageNum + SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneIndex);

            if (stageNum < 3)
                PlayerPrefs.SetInt("Stage", stageNum + 1);
            else if (PlayerPrefs.GetInt("UnlockAllStages", 0) != 1)
            {
                PlayerPrefs.SetInt("UnlockAllStages", 1);
                PlayerPrefs.SetInt("Stage", 1);
            }
        }


    }
}
