using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollView : ScrollRect
{
    public bool IsDraging { get; protected set; } = false;

    public Action onBeginDrag;
    public Action onEndDrag;

    public override void OnBeginDrag(PointerEventData eventData) {
        base.OnBeginDrag(eventData);
        IsDraging = true;
        onBeginDrag?.Invoke();
    }

    public override void OnEndDrag(PointerEventData eventData) {
        base.OnEndDrag(eventData);
        IsDraging = false;
        onEndDrag?.Invoke();
    }
}
