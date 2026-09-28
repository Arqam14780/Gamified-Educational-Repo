using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    
    public static GameHandler Instance;

    public SlidersAndNodes ref_SlidersAndNodes;
    public AIPlayer ref_AIScript;
    public GetConnectedSliders ref_GetConnectedSliders;
    public LvlManager ref_LevelManager;
    public MultiPlayerModeController ref_MultiPlayerModeController;
    public SetSelectionsInGamePlay ref_SetSelectionsInGamePlay;
    public SoundHandler ref_SoundController;
    public SoundMusicController ref_SoundMusicController;
    public StartMenu ref_MainMenu;

    public bool PlayerTurn = true;
    public bool isMultiPlayerModeActive = false;
    public bool isDirectPlayGame = false;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartGame()
    {
        isDirectPlayGame = false;
        StartCoroutine(StartGameLogic());
    }

    IEnumerator StartGameLogic()
    {
        yield return new WaitForSeconds(0.1f);
        ref_LevelManager.gameplayCanvas.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        ref_SetSelectionsInGamePlay.DrawGrid();
        ref_LevelManager.CheckPlayType();
        ref_AIScript.UpdateGridColor();
        ref_LevelManager.menuCanvas.SetActive(false);
    }

}
