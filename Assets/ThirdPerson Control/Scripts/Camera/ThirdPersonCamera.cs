using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace AR
{
    public class ThirdPersonCamera : MonoBehaviour
    {
        [Header("Target")]
        public Transform target;

        [Header("Input")]
        public InputActionReference lookAction;

        [Header("Camera")]
        public float distance = 6.0f;
        public float height = 2.0f;

        [Header("Rotation")]
        public float horizontalSensitivity = 180.0f;
        public float verticalSensitivity = 100.0f;
        public float minPitch = -30.0f;
        public float maxPitch = 60.0f;

        [Header("Smoothing")]
        public float rotationSmoothTime = 0.05f;
        public float positionSmoothTime = 0.05f;

        private float yaw;
        private float pitch;

        private float currentYaw;
        private float currentPitch;

        private Vector3 positionVelocity;

        private void OnEnable()
        {
            if (lookAction != null)
                lookAction.action.Enable();

            Vector3 angles = transform.eulerAngles;

            yaw = currentYaw = angles.y;
            pitch = currentPitch = NormalizeAngle(angles.x);
        }

        private void OnDisable()
        {
            if (lookAction != null)
                lookAction.action.Disable();
        }

        private void LateUpdate()
        {
            if (target == null)
                return;

            Vector2 lookInput = Vector2.zero;

            if (lookAction != null && !EventSystem.current.IsPointerOverGameObject())
                lookInput = lookAction.action.ReadValue<Vector2>();

            // Camera rotation
            yaw += lookInput.x * horizontalSensitivity * Time.deltaTime;
            pitch -= lookInput.y * verticalSensitivity * Time.deltaTime;

            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            currentYaw = Mathf.LerpAngle(
                currentYaw,
                yaw,
                1.0f - Mathf.Exp(-Time.deltaTime / rotationSmoothTime)
            );

            currentPitch = Mathf.Lerp(
                currentPitch,
                pitch,
                1.0f - Mathf.Exp(-Time.deltaTime / rotationSmoothTime)
            );

            Quaternion rotation =
                Quaternion.Euler(currentPitch, currentYaw, 0);

            Vector3 targetPosition =
                target.position + Vector3.up * height;

            Vector3 desiredPosition =
                targetPosition - rotation * Vector3.forward * distance;

            transform.position = Vector3.SmoothDamp(
                transform.position,
                desiredPosition,
                ref positionVelocity,
                positionSmoothTime
            );

            transform.rotation = rotation;
        }

        private float NormalizeAngle(float angle)
        {
            if (angle > 180.0f)
                angle -= 360.0f;

            return angle;
        }


    }
}