using UnityEngine;

namespace AR
{
    public class SoundManager : MonoBehaviour
    {
        public AudioClip btnSound;
        public AudioSource btn_AS;
        void Start()
        {

        }

        public void PlayBtnSound()
        {
            int soundFlag = PlayerPrefs.GetInt("SoundFlag", 1);
            if (soundFlag == 1)
                btn_AS.Play();
        }

    }
}
