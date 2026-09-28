using UnityEngine;

namespace AR {
    public class SoundManager : MonoBehaviour
    {
        public AudioClip btnSound;
        public AudioSource btn_AS;
        void Start()
        {

        }

        public void PlayBtnSound()
        {
            btn_AS.Play();
        }

    }
}
