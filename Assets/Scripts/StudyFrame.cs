using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace AR
{
    public enum StudyFrameKind { Quiz, Image, Pdf }

    public sealed class StudyFrame : MonoBehaviour, IPointerClickHandler
    {
        public string subject = "Math";
        public StudyFrameKind kind = StudyFrameKind.Quiz;
        public Action Activate;
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) Activate?.Invoke();
        }
    }
}
