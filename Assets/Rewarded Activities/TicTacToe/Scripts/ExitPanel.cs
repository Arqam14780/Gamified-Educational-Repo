using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPanel : MonoBehaviour
{

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
                SceneManager.LoadScene("MainMenu");
                //Application.Quit();
                break;
            case "No":
                gameObject.SetActive(false);
                break;
        }
        yield return null;
    }

}
