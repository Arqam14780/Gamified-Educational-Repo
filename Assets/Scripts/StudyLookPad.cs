using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace AR
{
    public sealed class StudyLookPad : MonoBehaviour, IDragHandler
    {
        public Action<Vector2> Look;
        public void OnDrag(PointerEventData data) { Look?.Invoke(data.delta); }
    }
}
