using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public static GameLogic instance;
    public int totalCombination = 3;

    public int combinationCounter = 0;
    public bool isSelected;
    public List<GameObject> selectedItem;
    [Space(5)]
    public List<GameObject> birdsContainer;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this.gameObject);
    }

}
