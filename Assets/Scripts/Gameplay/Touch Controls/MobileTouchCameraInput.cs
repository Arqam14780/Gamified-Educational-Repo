using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace AR {
    public class MobileTouchCameraInput : MonoBehaviour
    {
        [Header("Camera Look")]
        [SerializeField] private float touchSensitivity = 0.15f;
        [SerializeField] private float mouseSensitivity = 2f;

        [Header("Editor Mouse")]
        [SerializeField] private int mouseButton = 1; // Right mouse button

        [Header("Invert")]
        [SerializeField] private bool invertX = false;
        [SerializeField] private bool invertY = false;
        [SerializeField] private LayerMask uiLayer;

        private int cameraFingerId = -1;

        private void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            HandleMouseInput();
#else
        HandleTouchInput();
#endif
        }

        // =========================================================
        // MOBILE
        // =========================================================

        private void HandleTouchInput()
        {
            if (Input.touchCount == 0)
            {
                cameraFingerId = -1;

                SendLookInput(Vector2.zero);
                return;
            }

            // Find a finger that did NOT start on UI
            if (cameraFingerId == -1)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    if (touch.phase != TouchPhase.Began)
                        continue;

                    // Don't use UI touches for camera
                    if (IsTouchOverUI(touch.fingerId) || LearningController.Instance.isFrameOpened)
                        continue;
                    //// Don't rotate camera if mouse is over UI
                    //if (IsTouchOverUI(touch.fingerId) || LearningController.Instance.isFrameOpened)
                    //{
                    //    SendLookInput(Vector2.zero);
                    //    return;
                    //}

                    cameraFingerId = touch.fingerId;

                    break;
                }
            }

            // Process the camera finger
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.fingerId != cameraFingerId)
                    continue;

                // Safety check:
                // if touch is over UI, don't rotate camera
                if (IsTouchOverUI(touch.fingerId))
                {
                    SendLookInput(Vector2.zero);
                    return;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 delta = touch.deltaPosition;

                    delta *= touchSensitivity;

                    ApplyInversion(ref delta);

                    SendLookInput(delta);
                }
                else
                {
                    SendLookInput(Vector2.zero);
                }

                if (touch.phase == TouchPhase.Ended ||
                    touch.phase == TouchPhase.Canceled)
                {
                    cameraFingerId = -1;
                }

                return;
            }
        }

        // =========================================================
        // EDITOR / PC
        // =========================================================

        private void HandleMouseInput()
        {
            // No mouse button pressed
            if (!Input.GetMouseButton(mouseButton))
            {
                SendLookInput(Vector2.zero);
                return;
            }

            // Don't rotate camera if mouse is over UI
            if (IsMouseOverUI() || LearningController.Instance.isFrameOpened)
            {
                SendLookInput(Vector2.zero);
                return;
            }

            float mouseX =
                Input.GetAxis("Mouse X") * mouseSensitivity;

            float mouseY =
                Input.GetAxis("Mouse Y") * mouseSensitivity;

            Vector2 delta = new Vector2(
                mouseX,
                mouseY
            );

            ApplyInversion(ref delta);

            SendLookInput(delta);
        }

        // =========================================================
        // UI CHECK
        // =========================================================

        //private bool IsTouchOverUI(int fingerId)
        //{
        //    if (EventSystem.current == null)
        //        return false;

        //    return EventSystem.current.IsPointerOverGameObject(
        //        fingerId
        //    );
        //}
        private bool IsTouchOverUI(int fingerId)
        {
            if (EventSystem.current == null)
                return false;

            PointerEventData pointerData =
                new PointerEventData(EventSystem.current);

            // Get the position of this finger
            foreach (Touch touch in Input.touches)
            {
                if (touch.fingerId == fingerId)
                {
                    pointerData.position = touch.position;
                    break;
                }
            }

            List<RaycastResult> results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                GameObject obj = result.gameObject;

                // Check if this UI GameObject is on the selected layer
                if (((1 << obj.layer) & uiLayer.value) != 0)
                {
                    return true;
                }
            }

            return false;
        }


        private bool IsMouseOverUI()
        {
            if (EventSystem.current == null)
                return false;

            PointerEventData pointerData =
                new PointerEventData(EventSystem.current);

            pointerData.position = UnityEngine.InputSystem.Mouse.current.position.ReadValue();

            List<RaycastResult> results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                GameObject obj = result.gameObject;

                if (((1 << obj.layer) & uiLayer.value) != 0)
                {
                    return true;
                }
            }

            return false;
        }

        // =========================================================
        // INVERSION
        // =========================================================

        private void ApplyInversion(ref Vector2 input)
        {
            if (invertX)
                input.x *= -1f;

            if (invertY)
                input.y *= -1f;
        }

        // =========================================================
        // SEND INPUT
        // =========================================================

        private void SendLookInput(Vector2 input)
        {
            if (MobileInputManager.Instance != null)
            {
                MobileInputManager.Instance.SetLookInput(input);
            }
        }


    }
}