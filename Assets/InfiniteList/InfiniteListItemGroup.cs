using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class InfiniteListItemGroup : MonoBehaviour {
    public InfiniteListItem cellProto;

    [Header("默认为transform")]
    [SerializeField]
    private Transform _cellProtoParent;
    public Transform cellProtoParent {
        get {
            if(_cellProtoParent == null) {
                _cellProtoParent = transform;
            }
            return _cellProtoParent;
        }
    }

    [Header("几帧加载一个")]
    public uint countXFrame = 1;

    #region 组件
    public InfiniteList infiniteList;

    private RectTransform _rectTransform;
    public RectTransform rectTransform {
        get {
            if (_rectTransform == null) {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }
    #endregion

    [Header("调试显示")]
    public List<InfiniteListItem> items = new List<InfiniteListItem>(1);
    public int groupIndex = 0;

    [System.NonSerialized]
    public int startFrameIndex;

    public bool hasLoaded {
        get {
            return items.Count >= infiniteList.countPerLine;
        }
    }
    public int startIndex {
        get {
            return groupIndex * (int)infiniteList.countPerLine;
        }
    }
    public int endIndex {
        get {
            return (groupIndex + 1) * (int)infiniteList.countPerLine - 1;
        }
    }

    private bool forceRefresh = false;

    private void Awake() {
        cellProto.gameObject.SetActive(false);
    }
    private InfiniteListItem TryLoadOne() {
        InfiniteListItem clone = GameObject.Instantiate<InfiniteListItem>(cellProto, cellProtoParent);
        // todo: 是否强制设置为0位置
        clone.transform.localPosition = Vector3.zero;
        clone.transform.localEulerAngles = Vector3.zero;
        clone.transform.localScale = Vector3.one;
        clone.rectTransform.pivot = new Vector2(0.5f, 0.5f);

        clone.gameObject.SetActive(true);
        items.Add(clone);
        return clone;
    }
    private void Update() {
        if (!hasLoaded) {
            if (Time.frameCount - startFrameIndex >= countXFrame) {
                startFrameIndex = Time.frameCount;

                InfiniteListItem clone = TryLoadOne();
                infiniteList?.OnItemCreated?.Invoke(clone);

                // 只有执行了Refresh才能刷新
                if (forceRefresh) {
                    int index = startIndex + items.Count - 1;
                    if (infiniteList.IsIndexValid(index)) {
                        clone.gameObject.SetActive(true);
                        clone.Refresh(groupIndex, index);
                    }
                    else {
                        clone.gameObject.SetActive(false);
                    }
                }
            }
        }
        else {
            // 只有执行了Refresh才能刷新
            if (forceRefresh) {
                for (int i = 0; i < infiniteList.countPerLine; ++i) {
                    int index = startIndex + i;
                    if (infiniteList.IsIndexValid(index)) {
                        items[i].gameObject.SetActive(true);
                        items[i].Refresh(groupIndex, index);
                    }
                    else {
                        items[i].gameObject.SetActive(false);
                    }
                }

                forceRefresh = false;
            }
        }
    }

    public void Refresh(int groupIndex) {
        this.groupIndex = groupIndex;
        forceRefresh = true;

#if UNITY_EDITOR
        gameObject.name = groupIndex.ToString();
#endif
    }
}
