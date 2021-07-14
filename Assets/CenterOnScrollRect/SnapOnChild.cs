using System;
using System.Collections.Generic;
using UnityEngine;

public class SnapOnChild : MonoBehaviour {
    public enum ESnapStatus {
        None = 0,
        Moving = 1,
    }

    public ESnapStatus SnapStatus { get; protected set; } = ESnapStatus.None;

    public Vector2 MinScale = new Vector2(0.8f, 0.8f);
    public Vector2 Shrinkage = new Vector2(0.005f, 0.005f);

    [Header("低于该速度，才会开始centerOn流程")] public float stopSpeed = 150f;

    [Header("居中点")] public RectTransform center;

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

    // 会在边界情况下出现不居中的情况，目前做法是合理设置center的位置
    public virtual bool CanSnap {
        // 拖拽过程中不snap,snap过程中不snap
        get { return !scrollRect.IsDraging && SpeedReadySnapOn && SnapStatus == ESnapStatus.None; }
    }

    protected IList<Transform> contentChildren = new List<Transform>();

    private Transform nearest;

    protected float[] distReposition;
    protected float[] distance;

    private void Awake() {
        // 开始拖拽的时候，停止snap
        scrollRect.onBeginDrag += () => {
            if (enabled) {
                EndSnap();
            }
        };
    }

    private void Start() {
        Collect();
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

    public virtual void TrySnapOn(Transform target) { }

    private void Update() {
        CtrlScale(out nearest);

        if (CanSnap) {
            BeginSnap();
            TrySnapOn(nearest);
        }
    }

    private void BeginSnap() {
        SnapStatus = ESnapStatus.Moving;
    }

    protected void EndSnap() {
        SnapStatus = ESnapStatus.None;
        if (springTo != null) {
            springTo.enabled = false;
        }
    }
}