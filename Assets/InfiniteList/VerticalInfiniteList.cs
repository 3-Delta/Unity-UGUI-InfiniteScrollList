using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class VerticalInfiniteList : InfiniteList {
    public float height {
        get {
            return groupProto.rectTransform.rect.height;
        }
    }
    public override uint LINE {
        get {
            return (uint)Mathf.CeilToInt(scrollRect.viewport.rect.height / height) + 1;
        }
    }

    protected override void Awake() {
        base.Awake();

        scrollRect.horizontal = false;
        scrollRect.vertical = true;
    }
    protected override void SetFirstAnchoredPosition(RectTransform cloneGroupRect, int groupIndex) {
        float y = -groupIndex * height - height / 2;
        cloneGroupRect.anchoredPosition = new Vector3(cloneGroupRect.anchoredPosition.x, y);
    }
    // 只是
    protected override void SetContentWH() {
        float newContentHeight = height * realLineCount;
        // 获得offsetMin.y以（0， 0）为基准的offset
        // 确保content的最小height也是viewport.height
        // 也就是 保证不小于viewport的高度
        float newMinY = -newContentHeight + scrollRect.viewport.rect.height;
        float maxY = scrollRect.content.offsetMax.y;
        // 获得offsetMin.y以（0， offsetMax.y）为基准的offset, 目的是在刷新数据的时候，scrollrect的normalization不改变
        newMinY += maxY;
        
        scrollRect.content.offsetMin = new Vector2(0, newMinY);
    }
    public override void ResetPosition(float normalizedPosition = 0f) {
        scrollRect.verticalNormalizedPosition = normalizedPosition;
    }
    protected override void OnScrollRectNormalizedPositionChange(Vector2 normalizedPosition) {
        for (int i = 0, length = groups.Count; i < length; ++i) {
            InfiniteListItemGroup group = groups[i];
            RectTransform groupRect = group.rectTransform;
            // content的top距离每个item的y距离
            // scrollrect移动的时候, item的anchoredPosition不会变化,除非layoutgroup或者手动代码修改
            float distance = scrollRect.content.offsetMax.y + groupRect.anchoredPosition.y;
            float maxTop = height / 2;
            float minBottom = -((LINE) * height) + height / 2;

            if (distance > maxTop) {
                // content向上滚动,顶部向下弥补
                float newY = groupRect.anchoredPosition.y - (LINE) * height;
                // 保证cell的anchoredPosition只在content的高的范围内活动
                if (newY > -scrollRect.content.rect.height) {
                    // 重复利用cell，重置位置到视野范围内
                    groupRect.anchoredPosition = new Vector3(groupRect.anchoredPosition.x, newY);
                    GroupRefresh(group);
                }
            }
            else if (distance < minBottom) {
                // content向下滚动,底部向上弥补
                float newY = groupRect.anchoredPosition.y + (LINE) * height;
                // 保证cell的anchoredPosition只在content的高的范围内活动
                if (newY < 0) {
                    groupRect.anchoredPosition = new Vector3(groupRect.anchoredPosition.x, newY);
                    GroupRefresh(group);
                }
            }
        }
    }

    private void GroupRefresh(InfiniteListItemGroup group) {
        RectTransform groupRect = group.rectTransform;
        int index = Mathf.Abs(Mathf.CeilToInt((groupRect.anchoredPosition.y) / height));
        group.Refresh(index);
    }

    public override void MoveTo(int groupIndex, float duration = 0.3f) {
        // scroll如果垂直： 从上到下1 ~ 0
        // scroll如果水平： 从左到右0 ~ 1
        // 和anchor以及layotGroup的设置没有关系
        float toRate = 1f * (groupIndex + 1) / realLineCount;
        toRate = 1 - toRate;
        float fromRate = scrollRect.verticalNormalizedPosition;

        // Debug.LogError(groupIndex + " fromRate:" + fromRate + " toRate: " + toRate + " realLineCount:" + realLineCount);
        timer?.Cancel();
        timer = MonoTimer.Register(duration, null, () => false, (dt) => {
            float rate = Mathf.Lerp(fromRate, toRate, dt / duration);
            ResetPosition(rate);
        });
    }
}
