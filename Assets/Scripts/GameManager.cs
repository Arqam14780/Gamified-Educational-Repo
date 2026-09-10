using UnityEngine;
namespace AR
{
    public class GameManager : MonoBehaviour
    {
        [Tooltip("Assign At Run time")]
        public GameObject activePlayer;
        [SerializeField] private Transform CharacterContainer;
        [SerializeField] private ThirdPersonCamera thirdPersonCam;
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
            if (thirdPersonCam != null) thirdPersonCam.target = activePlayer.transform;
        }
        private void Start()
        {
            var room = new GameObject("Learning Academy - Study Room").AddComponent<StudyRoom>();
            room.Initialize(activePlayer);
        }
    }
}
