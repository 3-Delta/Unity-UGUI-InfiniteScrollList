using System;
using System.Collections.Generic;
using UnityEngine;

public class SnapOnChild : MonoBehaviour
{
    public Vector2 MinScale = new Vector2(0.8f, 0.8f);
    public Vector2 Shrinkage = new Vector2(0.005f, 0.005f);
    
    [Header("居中点")] 
    public RectTransform center;

    [SerializeField] private ScrollView _scrollRect;
    public ScrollView scrollRect {
        get {
            if (_scrollRect == null) {
                _scrollRect = GetComponent<ScrollView>();
            }

            return _scrollRect;
        }
    }
    
    protected Transform focus;
    protected SpringTo springTo;

    public Action<Transform> onSnaped;
    
    protected virtual bool SpeedReadySnapOn {
        get { return true; }
    }

    protected virtual bool ReadyStop {
        get { return false; }
    }

    public virtual bool CanSnap {
        get { return true; }
    }

    private bool hasStarted = false;
    protected IList<Transform> contentChildren = new List<Transform>();

    private Transform nearest;
    
    protected float[] distReposition;
    protected float[] distance;
    
    private void Awake() {
        // 开始拖拽的时候，停止snap
        scrollRect.onBeginDrag += () => {
            if (enabled && springTo != null) {
                springTo.enabled = false;
            }
        };
        // 停止拖拽的时候，执行snap
        scrollRect.onEndDrag += () => {
            if (enabled) {
                TrySnap();
            }
        };
    }
    private void OnEnable() {
        if (hasStarted) {
            TrySnap();
        }
    }
    private void Start() {
        Collect();
        hasStarted = true;
        
        TrySnap();
    }

    protected void Collect() {
        contentChildren.Clear();
        int length = scrollRect.content.childCount;
        for (int i = 0; i < length; ++i) {
            Transform child = scrollRect.content.GetChild(i);
            contentChildren.Add(child);
        }

        distance = new float[length];
        distReposition = new float[length];
    }

    protected virtual void CtrlScale(out Transform target) {
        target = null;
    }
    public virtual void TrySnapOn(Transform target) {
    }
    // start/dragend/call 三种方式调用
    public void TrySnap() {
        if (nearest == null) {
            CtrlScale(out nearest);
        }

        TrySnapOn(nearest);
    }

    private void Update() {
        CtrlScale(out nearest);
    }
}
