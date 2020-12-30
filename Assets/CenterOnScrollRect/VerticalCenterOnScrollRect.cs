using System;
using UnityEngine;
using UnityEngine.UI;

public class VerticalCenterOnScrollRect : CenterOnScrollRect
{
    protected override bool ReadyCenterOn {
        get { return scrollRect.velocity.y <= StopSpeed; }
    }
    protected override bool ReadyStop {
        get {
            return Mathf.Abs(scrollRect.velocity.y) == 0f;
        }
    }

    protected override void OnBtnLeftClicked() {
        int index = NearestIndex;
        index = Math.Max(0, index - 1);
        Transform t = scrollRect.content.GetChild(index);
        CenterOn(t);
    }

    protected override void OnBtnRightClicked() {
        int index = NearestIndex;
        index = Math.Min(index + 1, contentChildren.Count - 1);
        Transform t = scrollRect.content.GetChild(index);
        CenterOn(t);
    }

    private void Update() {
        float nearestDistance = float.MaxValue;
        for (int i = 0, length = contentChildren.Count; i < length; ++i) {
            // 世界坐标的差值
            distReposition[i] = center.position.y - contentChildren[i].position.y;
            distance[i] = Mathf.Abs(distReposition[i]);
            // 获取最小距离
            if (distance[i] < nearestDistance) {
                nearestDistance = distance[i];
                Nearest = contentChildren[i];
            }

            // 控制缩放
            Vector2 scale = new Vector2(1 / (1 + distance[i] * Shrinkage.x), (1 / (1 + distance[i] * Shrinkage.y)));
            scale = Vector2.Max(MinScale, scale);
            contentChildren[i].transform.localScale = new Vector3(scale.x, scale.y, 1f);
        }
        
        // 不拖拽 并且 速度低于阈值的时候，开始centerOn
        // scroll slowly to nearest element when not dragged
        if (Nearest != Focused && !Input.GetMouseButton(0) && ReadyCenterOn) {
            CenterOnNearest();
            
#if UNITY_EDITOR
            center.name = "Center on: " + Focused;
#endif
        }

        // 控制enable
        enabled = ReadyStop;
    }
}
