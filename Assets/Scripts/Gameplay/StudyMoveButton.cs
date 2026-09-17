using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace AR
{
    public sealed class StudyMoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public Action<bool> Changed;
        public void OnPointerDown(PointerEventData data) { Changed?.Invoke(true); }
        public void OnPointerUp(PointerEventData data) { Changed?.Invoke(false); }
        public void OnPointerExit(PointerEventData data) { Changed?.Invoke(false); }
        private void OnDisable() { Changed?.Invoke(false); }
    }
}
