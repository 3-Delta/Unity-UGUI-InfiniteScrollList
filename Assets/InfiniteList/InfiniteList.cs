using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// github: UGUIScrollGrid-master
public abstract class InfiniteList : MonoBehaviour {
    public InfiniteListItemGroup groupProto;
    public Transform groupProtoParent;

    public virtual uint LINE {
        get { return 1; }
    }

    [Header("每行/列 x个元素")] public uint countPerLine = 1;

    #region 组件

    private ActivateRegistry _toggleCollector;

    public ActivateRegistry toggleCollector {
        get {
            if (_toggleCollector == null) {
                _toggleCollector = GetComponentInChildren<ActivateRegistry>();
            }

            return _toggleCollector;
        }
    }

    private ScrollRect _scrollRect;

    public ScrollRect scrollRect {
        get {
            if (_scrollRect == null) {
                _scrollRect = GetComponentInChildren<ScrollRect>();
            }

            return _scrollRect;
        }
    }

    #endregion

    public Action<InfiniteListItem> OnItemCreated;

    // groupIndex:index:id:InfiniteListItem
    public Action<int, int, InfiniteListItem> OnItemRefreshed;
    public Action<int, int, InfiniteListItem> OnItemSelected;

    // 当前位置移动到特定位置
    protected MonoTimer timer;

    [Header("调试显示")] public List<InfiniteListItemGroup> groups = new List<InfiniteListItemGroup>();

    public int DataCount { get; protected set; }

    public int realLineCount {
        get { return Mathf.CeilToInt(1f * DataCount / (int) countPerLine); }
    }

    public bool IsIndexValid(int index) {
        return 0 <= index && index < DataCount;
    }

    protected virtual void Awake() {
        groupProto.gameObject.SetActive(false);

        scrollRect.content.offsetMax = Vector2.zero;
        scrollRect.content.offsetMin = Vector2.zero;

        // 设置scrollrect的content的anchor为min(0, 0), max(1, 1)
        scrollRect.content.localScale = Vector3.one;
        scrollRect.content.offsetMax = Vector2.zero;
        scrollRect.content.offsetMin = Vector2.zero;
        scrollRect.content.anchorMin = Vector2.zero;
        scrollRect.content.anchorMax = Vector2.one;

        scrollRect.viewport.localScale = Vector3.one;
        scrollRect.viewport.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        scrollRect.viewport.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        scrollRect.viewport.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);
        scrollRect.viewport.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
        scrollRect.viewport.anchorMin = Vector2.zero;
        scrollRect.viewport.anchorMax = Vector2.one;

        scrollRect.onValueChanged.AddListener(OnScrollRectNormalizedPositionChange);

        // 关闭gridLayoutGroup,否则在滚动scrollrect的时候,重新计算item的anchorPosition的时候会受到影响
        GridLayoutGroup gridLayoutGroup = transform.GetComponentInChildren<GridLayoutGroup>();
        if (gridLayoutGroup != null) {
            gridLayoutGroup.enabled = false;
        }
    }

    protected virtual void Start() {
    }

    protected virtual void SetFirstAnchoredPosition(RectTransform cloneGroupRect, int groupIndex) {
    }

    private InfiniteListItemGroup TryLoadOne() {
        InfiniteListItemGroup clone = GameObject.Instantiate<InfiniteListItemGroup>(groupProto, groupProtoParent);
        // 不能设置Vector3.zero,需要保持原始位置
        // clone.transform.localPosition = Vector3.zero;
        clone.transform.localEulerAngles = Vector3.zero;
        clone.transform.localScale = Vector3.one;

        // 开始分桢加载的时机
        clone.startFrameIndex = Time.frameCount + groups.Count - 1;

        // 锚点强制设为左上角，方便算出位置。
        RectTransform cloneGroupRect = clone.GetComponent<RectTransform>();
        cloneGroupRect.anchorMin = new Vector2(0, 1);
        cloneGroupRect.anchorMax = new Vector2(0, 1);
        // 必须为中心点,否则后续计算位置会出错
        cloneGroupRect.pivot = new Vector2(0.5f, 0.5f);
        SetFirstAnchoredPosition(cloneGroupRect, groups.Count);

        clone.gameObject.SetActive(true);
        groups.Add(clone);
        return clone;
    }

    private void TryLoad(bool refresh) {
        int factLineCount = (int) Mathf.Min(LINE, realLineCount);
        while (factLineCount > groups.Count) {
            var group = TryLoadOne();
            if (refresh) {
                group.Refresh(groups.Count - 1);
            }
        }

        if (refresh) {
            int realLineCount = this.realLineCount;
            for (int i = 0, length = groups.Count; i < length; ++i) {
                if (i < realLineCount) {
                    groups[i].gameObject.SetActive(true);
                    groups[i].Refresh(groups[i].groupIndex);
                }
                else {
                    groups[i].gameObject.SetActive(false);
                }
            }
        }
    }

    public void SetCount(int count, bool refresh = true) {
        DataCount = count;
        TryLoad(refresh);
        SetContentWH();
    }

    protected virtual void SetContentWH() {
    }

    protected virtual void OnScrollRectNormalizedPositionChange(Vector2 normalizedPosition) {
    }

    public virtual void ResetPosition(float normalizedPosition = 0f) {
    }

    public virtual void MoveTo(int groupIndex, float duration = 0.3f) {
    }
}