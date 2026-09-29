using UnityEngine;
using UnityEngine.SceneManagement;
namespace AR
{
    public class GameHandler : MonoBehaviour
    {
        [Tooltip("Assign At Run time")]
        public GameObject activePlayer;
        [SerializeField] private Transform CharacterContainer;
        [SerializeField] private ThirdPersonOrbitCamBasic thirdPersonOrbitCam;
        private void Awake()
        {
            if (CharacterContainer == null || CharacterContainer.childCount == 0)
            {
                Debug.LogError("GameManager needs a character container with at least one player.", this);
                enabled = false;
                return;
            }
            int index = Mathf.Clamp(PlayerPrefs.GetInt("ChildIndex", 0), 0, CharacterContainer.childCount - 1);
            for (int i = 0; i < CharacterContainer.childCount; i++) CharacterContainer.GetChild(i).gameObject.SetActive(i == index);
            activePlayer = CharacterContainer.GetChild(index).gameObject;
            thirdPersonOrbitCam.player = activePlayer.transform;
            thirdPersonOrbitCam.enabled = true;
        }

        private void OnEnable()
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }

        public void SetPlayerRotation()
        {
            activePlayer.transform.position = new Vector3(0.71f, -0.066f, 2f);
            activePlayer.transform.rotation = Quaternion.Euler(activePlayer.transform.rotation.x, 
                180f, activePlayer.transform.rotation.z);

            thirdPersonOrbitCam.enabled = false;
            thirdPersonOrbitCam.transform.position = new Vector3(0.68f, 1.473f, 3.945f);
            thirdPersonOrbitCam.transform.rotation = Quaternion.Euler(12.042f, 179.973f, 0f);
            thirdPersonOrbitCam.enabled = true;
        }

        public void GoToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

    }
}
