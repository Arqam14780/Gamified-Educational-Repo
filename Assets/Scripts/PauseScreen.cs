using UnityEngine;
using UnityEngine.UI;

namespace AR
{
    public class PauseScreen : MonoBehaviour
    {
        public GameObject pauseScreen;
        [SerializeField] private AudioSource bgMusic;
        [Range(0, 1)]
        [SerializeField] private float defaultVolume = 0.2f;
        [SerializeField] private Toggle soundToggle;
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Slider volumeSlider;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            bgMusic.volume = defaultVolume;
            SetStatus();
        }

        private void SetStatus()
        {
            float soundVal = PlayerPrefs.GetFloat("SoundValue", defaultVolume);
            bgMusic.volume = soundVal;
            volumeSlider.value = soundVal;

            int soundFlag = PlayerPrefs.GetInt("SoundFlag", 1);
            UpdateToggle(soundToggle, soundFlag == 1 ? true : false);

            int musicFlag = PlayerPrefs.GetInt("MusicFlag", 1);
            if (musicFlag != 1) bgMusic.Stop();
            UpdateToggle(musicToggle, musicFlag == 1 ? true : false);
        }

        private void UpdateToggle(Toggle toggle, bool toggleFlag)
        {
            toggle.isOn = toggleFlag;
        }

        public void OpenSettingPanel()
        {
            LearningController.Instance.soundManager.PlayBtnSound();
            pauseScreen.SetActive(true);
        }

        public void ToggleSoundPressed()
        {
            UpdateToggle(soundToggle, soundToggle.isOn);
            PlayerPrefs.SetInt("SoundFlag", soundToggle.isOn == true ? 1 : 0);  
        }

        public void ToggleMusicPressed()
        {
            UpdateToggle(musicToggle, musicToggle.isOn);
            if (musicToggle.isOn)
                bgMusic.Play();
            else
                bgMusic.Stop();
            PlayerPrefs.SetInt("MusicFlag", musicToggle.isOn == true ? 1 : 0);
        }

        public void AdjustVolume()
        {
            PlayerPrefs.SetFloat("SoundValue", volumeSlider.value);
            bgMusic.volume = volumeSlider.value;
        }

        public void GoToMenu()
        {
            LearningController.Instance.soundManager.PlayBtnSound();
            LearningController.Instance.landscapeLoading.SetActive(true);
            LearningController.Instance.landscapeLoading.GetComponent<LoadStages>().LoadActivity(0);
        }

        public void Continue()
        {
            LearningController.Instance.soundManager.PlayBtnSound();
            pauseScreen.SetActive(false);
        }

    }
}
