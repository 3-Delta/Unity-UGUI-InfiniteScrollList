using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteListItem : MonoBehaviour {
    public int groupIndex;
    public int index;

    #region 组件

    [Header("控制高亮")] [SerializeField] private Activate _toggle;

    public Activate toggle {
        get {
            if (_toggle == null) {
                _toggle = GetComponent<Activate>();
            }

            return _toggle;
        }
    }

    private InfiniteList _infiniteList;

    public InfiniteList infiniteList {
        get {
            if (_infiniteList == null) {
                _infiniteList = GetComponentInParent<InfiniteList>();
            }

            return _infiniteList;
        }
    }

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

    protected virtual void Awake() {
    }

    public void Refresh(int groupIndex, int index) {
        this.groupIndex = groupIndex;
        this.index = index;

#if UNITY_EDITOR
        gameObject.name = string.Format("{0}-{1}", groupIndex, index);
#endif

        infiniteList?.OnItemRefreshed?.Invoke(groupIndex, index, this);
    }
}