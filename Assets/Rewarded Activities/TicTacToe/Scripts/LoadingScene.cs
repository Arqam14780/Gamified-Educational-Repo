using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{

    AsyncOperation ref_Loading_Sync;
    bool canLoad = false;
    public string next_Scene;

    public Slider ref_Slider;
    public Text ref_loadingBarText;



    void Start()
    {
        Time.timeScale = 1;
        Invoke("startnextScene", 1f);
    }

    void startnextScene()
    {

        ref_Loading_Sync = SceneManager.LoadSceneAsync("" + next_Scene);
        canLoad = true;
    }

    void Update()
    {

        if (canLoad)
        {

            ref_Slider.value = ref_Loading_Sync.progress;

            float val = ref_Loading_Sync.progress * 100;
            ref_loadingBarText.text = (Mathf.Round(val) + " %");
        }

    }
}
