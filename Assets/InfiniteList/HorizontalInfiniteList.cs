using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalInfiniteList : InfiniteList {
    public float width {
        get { return groupProto.rectTransform.rect.width; }
    }

    public override uint LINE {
        get { return (uint) Mathf.CeilToInt(scrollRect.viewport.rect.width / width) + 1; }
    }

    protected override void Awake() {
        base.Awake();

        scrollRect.horizontal = true;
        scrollRect.vertical = false;
    }

    protected override void SetFirstAnchoredPosition(RectTransform cloneGroupRect, int groupIndex) {
        float x = groupIndex * width + width / 2;
        cloneGroupRect.anchoredPosition = new Vector3(x, cloneGroupRect.anchoredPosition.y);
    }

    protected override void SetContentWH() {
        float newContentWidth = width * realLineCount;
        // 当minX == 0时maxX的位置
        float newMaxX = newContentWidth - scrollRect.viewport.rect.width;
        float minX = scrollRect.content.offsetMin.x;
        newMaxX += minX;

        scrollRect.content.offsetMax = new Vector2(newMaxX, 0);
    }

    public override void ResetPosition(float normalizedPosition = 0f) {
        scrollRect.horizontalNormalizedPosition = normalizedPosition;
    }

    protected override void OnScrollRectNormalizedPositionChange(Vector2 normalizedPosition) {
        for (int i = 0, length = groups.Count; i < length; ++i) {
            InfiniteListItemGroup group = groups[i];
            RectTransform groupRect = group.rectTransform;
            float distance = scrollRect.content.offsetMin.x + groupRect.anchoredPosition.x;
            float minLeft = -width / 2;
            float maxRight = (LINE * width) - width / 2;

            if (distance < minLeft) {
                float newX = groupRect.anchoredPosition.x + (LINE) * width;
                // 保证cell的anchoredPosition只在content的高的范围内活动
                if (newX < scrollRect.content.rect.width) {
                    // 重复利用cell，重置位置到视野范围内
                    groupRect.anchoredPosition = new Vector3(newX, groupRect.anchoredPosition.y);
                    GroupRefresh(group);
                }
            }
            else if (distance > maxRight) {
                // 保证cell的anchoredPosition只在content的高的范围内活动
                float newX = groupRect.anchoredPosition.x - (LINE) * width;
                if (newX > 0) {
                    groupRect.anchoredPosition = new Vector3(newX, groupRect.anchoredPosition.y);
                    GroupRefresh(group);
                }
            }
        }
    }

    private void GroupRefresh(InfiniteListItemGroup group) {
        RectTransform groupRect = group.rectTransform;
        int index = Mathf.FloorToInt((groupRect.anchoredPosition.x) / width);
        group.Refresh(index);
    }

    public override void MoveTo(int groupIndex, float duration = 0.3f) {
        // scroll如果垂直： 从上到下1 ~ 0
        // scroll如果水平： 从左到右0 ~ 1
        // 和anchor以及layotGroup的设置没有关系
        float toRate = 1f * (groupIndex + 1) / realLineCount;
        float fromRate = scrollRect.horizontalNormalizedPosition;

        timer?.Cancel();
        timer = MonoTimer.Register(duration, null, () => false, (dt) => {
            float rate = Mathf.Lerp(fromRate, toRate, dt / duration);
            ResetPosition(rate);
        });
    }
}