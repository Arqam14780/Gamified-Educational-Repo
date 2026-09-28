using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{    
    public static GameController Instance;
    public SequenceElements ref_SequenceElements;
    public AIScript ref_AIScript;
    public ControllerScript ref_ControllerScript;
    public GameplayUI ref_GameplayUI;
    public Constants ref_Constants;
    public SoundController ref_SoundController;
    public MainMenu ref_MainMenu;
    public SetSelectionInGameplay ref_SetSelectionInGameplay;

    public GameObject gamePlayCanvas, menuCanvas;
    public bool isMultiPlayerModeActive = false;
    public static int GridSize=1;
    

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        //else
        //{
        //    Destroy(this.gameObject);
        //}
    }

    private void OnEnable()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void OnDisable()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

}
