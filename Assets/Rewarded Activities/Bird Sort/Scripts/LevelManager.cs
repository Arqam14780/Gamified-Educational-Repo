using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public GameObject PauseScreen;

    [System.Serializable]
    public class LevelHandler
    {
        public string LevelName;
        public GameObject levelContainer;
        public int totalCombination;
    }

    public LevelHandler[] Levels;
    [Space(5)]
    public AudioClip[] birdSounds;
    [Space(5)]
    public AudioClip rewardSound;

    public int combinationCounter = 0;

    public Text levelNumber;
    public int currentLevel;

    public GameObject levelCompletePanel;

    public bool isTesting = false;

    private AudioSource audioComponent;

    private static bool isRestart = false;
    private static int selectedLvl = 0;

    private void OnEnable()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void OnDisable()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    void Start()
    {
        Time.timeScale = 1;
        GameController2.instance.ref_LevelManager = this;
        audioComponent = GetComponent<AudioSource>();

        if (isTesting)
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);

        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        if (currentLevel >= Levels.Length)
        {
            if (isRestart)
            {
                Debug.Log("currentLevel: " + currentLevel);
                currentLevel = selectedLvl;
            }
            else
            {
                int _ran = Random.Range(5, 9);
                currentLevel = _ran;
            }
        }

        levelNumber.text = "Level : " + ((PlayerPrefs.GetInt("CurrentLevel", 0)) + 1).ToString();

        Levels[currentLevel].levelContainer.SetActive(true);

        InvokeRepeating("BirdSound", 0.5f, Random.Range(6, 10));
        //FbLogs.ins.CustomLog("current_level_" + currentLevel);
    }

    void BirdSound()
    {
        if (!audioComponent.isPlaying)
            audioComponent.PlayOneShot(birdSounds[Random.Range(0, birdSounds.Length - 1)]);
    }

    public IEnumerator LevelCompleted()
    {
        if (PlayerPrefs.GetInt("CurrentLevel", 0) > 9)
            isRestart = false;

        GameLogic.instance.birdsContainer.Clear();
        yield return new WaitForSeconds(2f);
        combinationCounter = 0;
        Time.timeScale = 0;
        if (PlayerPrefs.GetInt("Vibration", 0) == 0)
            Handheld.Vibrate();

        levelCompletePanel.SetActive(true);
    }

    public void NextLevel()
    {

        if (PlayerPrefs.GetInt("CurrentLevel", 0) <= 9)
            currentLevel++;
        else
            currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0) + 1;

        PlayerPrefs.SetInt("CurrentLevel", currentLevel);

        SceneManager.LoadScene("BirdSort");
    }

    public void GamePause()
    {
        PauseScreen.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseScreen.SetActive(false);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void RestartLevel()
    {
        if (PlayerPrefs.GetInt("CurrentLevel", 0) > 9)
        {
            isRestart = true;
            selectedLvl = currentLevel;
        }
        else
            isRestart = false;

        SceneManager.LoadScene("BirdSort");
    }

    public void SkipCurrentLevel()
    {
        audioComponent.PlayOneShot(rewardSound);
        currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        StartCoroutine(LoadSceneMode());
    }
    IEnumerator LoadSceneMode()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        SceneManager.LoadScene("Gameplay");
    }

}
