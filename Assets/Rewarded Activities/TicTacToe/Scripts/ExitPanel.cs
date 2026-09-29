using System.Collections;
using UnityEngine;

public class ExitPanel : MonoBehaviour
{
    public GameObject loadingPanel;
    public void ButtonClick(string btnName)
    {
        StartCoroutine(btnClick(btnName));
    }

    IEnumerator btnClick(string btnName)
    {
        GameController.Instance.ref_SoundController._BtnSound();

        switch (btnName)
        {
            case "Yes":
                loadingPanel.SetActive(true);
                break;
            case "No":
                gameObject.SetActive(false);
                break;
        }
        yield return null;
    }

}
