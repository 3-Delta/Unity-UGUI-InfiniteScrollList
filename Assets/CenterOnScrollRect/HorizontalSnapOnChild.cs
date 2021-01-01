using System;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalSnapOnChild : SnapOnChild
{
    public override void TrySnapOn(Transform target) {
        if (target == null) {
            focus = null;
        }
        else {
            // 设置聚焦点
            focus = target;

            // 将center转换为 以content为基准的情形下的一个localPosition
            Vector3 relativeLocalPositionTarget = scrollRect.content.InverseTransformPoint(target.position);
            Vector3 relativeLocalPositionCenter = scrollRect.content.InverseTransformPoint(center.position);
            // 都在content下, 和center和item的位置差值
            Vector3 localOffset = relativeLocalPositionTarget - relativeLocalPositionCenter;
            localOffset.z = 0f;
            localOffset.y = 0f;

            springTo = SpringTo.Begin(scrollRect.content, scrollRect.content.transform.position - localOffset, () => {
                onSnaped?.Invoke(focus);
            });
        }
    }
}
