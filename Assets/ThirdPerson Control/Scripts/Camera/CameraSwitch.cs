using UnityEngine;

namespace AR
{
    public class CameraSwitch : MonoBehaviour
    {
        public enum CameraType { TopDown, SmoothFollow};
        public CameraType cameraType;

        [SerializeField]
        private TP_Motor charaMotor;
        [Space(5)]
        [SerializeField]
        private Transform smoothCamera;
        [SerializeField]
        private Transform topDownCamera;
        [Space(5)]
        public KeyCode cameraSwitchkey = KeyCode.None;

        // Start is called before the first frame update
        void Start()
        {
            transform.parent = null;
            if (charaMotor == null)
                charaMotor = FindObjectOfType<TP_Motor>();

            SwitchCam();
        }

        // Change camera 
        public void SwitchCam()
        {
            if(cameraType.Equals(CameraType.TopDown))
            {
                cameraType = CameraType.SmoothFollow;
                charaMotor.cameraTransform = smoothCamera;
                smoothCamera.gameObject.SetActive(true);
                topDownCamera.gameObject.SetActive(false);
            }
            else if (cameraType.Equals(CameraType.SmoothFollow))
            {
                cameraType = CameraType.TopDown;
                charaMotor.cameraTransform = topDownCamera;
                topDownCamera.gameObject.SetActive(true);
                smoothCamera.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(this.cameraSwitchkey))
                SwitchCam();
        }

    }
}
