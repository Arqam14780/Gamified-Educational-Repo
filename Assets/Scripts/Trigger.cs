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
            stageNum = stageNum + SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(stageNum);

            if (stageNum < 3)
                PlayerPrefs.SetInt("Stage", stageNum + 1);
            else
                PlayerPrefs.SetInt("Stage", 1);
        }


    }
}
