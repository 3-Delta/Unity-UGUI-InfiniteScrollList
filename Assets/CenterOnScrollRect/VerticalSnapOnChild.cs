using System;
using UnityEngine;
using UnityEngine.UI;

public class VerticalSnapOnChild : SnapOnChild
{
    protected override bool SpeedReadySnapOn {
        get { return Mathf.Abs(scrollRect.velocity.y) <= stopSpeed; }
    }
    
    public override bool CanSnap {
        get {
            bool ret = base.CanSnap;
            if (ret) {
                // 处理边界情况
                RectTransform rt = scrollRect.content;
                float maxY = rt.offsetMax.y;
                if (maxY < 0) {
                    return false;
                }
                float minY = rt.offsetMin.y;
                if (minY > 0) {
                    return false;
                }
            }
            return ret;
        }
    }
    
    protected override void CtrlScale(out Transform nearest) {
        nearest = null;
        float nearestDistance = float.MaxValue;
        for (int i = 0, length = contentChildren.Count; i < length; ++i) {
            if (!contentChildren[i].gameObject.activeInHierarchy) {
                continue;
            }

            // 世界坐标的差值
            distReposition[i] = center.position.y - contentChildren[i].position.y;
            distance[i] = Mathf.Abs(distReposition[i]);
            // 获取最小距离
            if (distance[i] < nearestDistance) {
                nearestDistance = distance[i];
                nearest = contentChildren[i];
            }

            // 控制缩放
            Vector2 scale = new Vector2(1 / (1 + distance[i] * Shrinkage.x), (1 / (1 + distance[i] * Shrinkage.y)));
            scale = Vector2.Max(MinScale, scale);
            contentChildren[i].transform.localScale = new Vector3(scale.x, scale.y, 1f);
        }
    }
    
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
            localOffset.x = 0f;

            springTo = SpringTo.Begin(scrollRect.content, scrollRect.content.transform.position - localOffset, () => {
                EndSnap();
                onSnaped?.Invoke(focus);
            });
        }
    }
}
