using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace AR {
    public class VideoProgressBar : MonoBehaviour
    {
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private Slider progressBar;

        private void OnDisable()
        {
            progressBar.value = 0;
        }
        private void Update()
        {
            if (videoPlayer == null || progressBar == null)
                return;

            if (!videoPlayer.isPrepared)
                return;

            if (videoPlayer.length <= 0)
                return;

            // 0 = beginning, 1 = end
            progressBar.value =
                (float)(videoPlayer.time / videoPlayer.length);
        }

    }
}