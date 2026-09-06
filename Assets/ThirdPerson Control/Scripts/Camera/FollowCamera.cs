using UnityEngine;

namespace AR {
    public class FollowCamera : MonoBehaviour
    {
        public Transform targetTransform;
        public Transform camTarget;
        public float smoothingTime = 0.1f;
        public float perspZoomOutOffset = 10.0f, orthoZoomInSize = 2, orthoZoomOutSize = 5;

        private Vector3 targetOfs, smoothPosVel;
        private float camZoomFactor;
        private Camera cam;

        // -------------------
        void OnEnable()
        {
            this.cam = this.GetComponent<Camera>();

            this.transform.position = camTarget.position;
            this.transform.rotation = camTarget.rotation;

            if ((this.cam != null) && (this.cam.orthographic))
                this.camZoomFactor = 0.5f;

            if (this.targetTransform != null)
                this.targetOfs = this.transform.position - this.targetTransform.position;
        }

        // ------------------
        void Update()
        {
            this.camZoomFactor += Input.GetAxis("Mouse ScrollWheel");

            this.camZoomFactor = Mathf.Clamp01(this.camZoomFactor);
        }

        // --------------------
        void FixedUpdate()
        {
            if (this.targetTransform == null)
                return;

            Vector3 pos =
                this.targetTransform.position + this.targetOfs;


            if ((this.cam != null) && cam.orthographic)
            {
                cam.orthographicSize = AR_Utlis.SmoothTowards(this.cam.orthographicSize,
                    Mathf.Lerp(this.orthoZoomInSize, this.orthoZoomOutSize, this.camZoomFactor), this.smoothingTime, AR_Utlis.realDeltaTimeClamped, 0.0001f);
            }

            else
                pos -= (this.transform.forward * (this.camZoomFactor * this.perspZoomOutOffset));

            if (this.smoothingTime > 0.001f)
                pos = Vector3.SmoothDamp(this.transform.position, pos, ref this.smoothPosVel, this.smoothingTime);

            this.transform.position = pos;
        }


    }
}

