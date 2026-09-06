using UnityEngine;

namespace AR
{
    public class GameManager : MonoBehaviour
    {
        [Tooltip("Assign At Run time")]
        public GameObject activePlayer;

        [SerializeField]
        private Transform CharacterContainer;
        [SerializeField]
        private ThirdPersonCamera thirdPersonCam;


        private void Awake()
        {
            activePlayer = CharacterContainer.GetChild(PlayerPrefs.GetInt("ChildIndex", 0)).gameObject;
            activePlayer.SetActive(true);
            thirdPersonCam.target = activePlayer.transform;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }


  
    }
}
